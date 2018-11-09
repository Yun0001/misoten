using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : KitchenwareBase {

    public enum Status
    {
        Stand,
        AccessOne,
        AccessTwo,
        AccessThree,
        Start,
        Open,
        Play,
        Put,
        End
    }

    [SerializeField]
    private int TIME;

    private int timeBorder;

    [SerializeField]
    private int time;
    private float count;
    [SerializeField]
    private float animCount = 0;

    // 蓋が開いてるフレーム
    [SerializeField]
    private int openFrame;

    private bool isEatoyPut = false;

    [SerializeField]
    private int accessNum = 0;
    private bool efectFlg;
    private bool seFlg;
    private bool uiFlg;

    private GameObject mixerEatoyPrefab;


    [SerializeField]
    private Status status = Status.Stand;

    [SerializeField]
    private GameObject[] cuisines;



    // Use this for initialization
    void Awake () {
        mixerEatoyPrefab = Resources.Load("Prefabs/Eatoy/MixerEatoy") as GameObject;
        miniGameUI = Instantiate(Resources.Load("Prefabs/MixerMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.SetActive(false);
    }

    private void Update()
    {
        switch (status)
        {
            case Status.AccessTwo:

                if (accessNum == (int)Status.AccessTwo)
                {
                    // 調理開始
                    status = Status.Open;
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
                }
                break;

            case Status.Open:
                animCount++;
                if (animCount >= openFrame)
                {
                    InitMiniGameUI();
                    status = Status.Play;
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
                    miniGameUI.SetActive(true);
                    animCount = 0;
                }
                break;

            case Status.Play:
                UpdateMiniGame();
                break;

            case Status.Put:
                UpdateMiniGame();
                break;

        }



    }

    protected override void InstanceMiniGameUI()
    {
    }

    protected override void InitMiniGameUI()
    {
        timeBorder = TIME * 60;
        time = TIME;
        isEatoyPut = false;
        status = Status.Stand;
    }

    protected override void ResetMiniGameUI()
    {
        timeBorder = TIME * 60;
        time = 0;
        isEatoyPut = false;
        status = Status.Stand;
    }

    protected override GameObject SetCuisine()
    {
        return null;
    }

    protected override bool Cooking()
    {
        switch (status)
        {
            case Status.Play:

                time++;
                if (time >= timeBorder)
                {
                    status = Status.Put;
                }
                break;

            case Status.Put:
                // 皿が置かれているか判定
                // 置かれていればOpenアニメーションを再生
                if (transform.Find("mixer").GetComponent<mixerAnimCtrl>().GetIsOpen())
                {
                    animCount++;
                    if (animCount >= openFrame)
                    {
                        // 蓋を閉める
                        transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
                        // statusをEndに変更
                        status = Status.End;
                    }
                    else if (animCount >= openFrame / 2)
                    {
                        // イートイを出す
                        if (!isEatoyPut)
                        {
                            miniGameUI.SetActive(false);
                            GameObject eatoy = Instantiate(mixerEatoyPrefab, transform.position, Quaternion.identity);
                            Vector3 pos = eatoy.transform.position;
                            pos.x = 0.4f;
                            pos.y = 1.3f;
                            pos.z = -0.5f;
                            eatoy.transform.position = pos;
                            isEatoyPut = true;

                            // 料理を無くす
                            for (int i = 0; i < cuisines.Length; i++)
                            {
                                if (cuisines[i] != null)
                                {
                                    switch (cuisines[i].GetComponent<Food>().GetCategory())
                                    {
                                        case Food.Category.Grilled:
                                            CuisineManager.GetInstance().GetGrilledController().OfferCuisine(cuisines[i].GetComponent<Food>().GetFoodID());
                                            break;

                                        case Food.Category.Pot:
                                            CuisineManager.GetInstance().GetPotController().OfferCuisine(cuisines[i].GetComponent<Food>().GetFoodID());
                                            break;

                                        case Food.Category.Microwave:
                                            CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(cuisines[i].GetComponent<Food>().GetFoodID());
                                            break;
                                    }
                                    cuisines[i] = null;
                                }
                            }
                            // 当たり判定復活
                            BoxCollider[] bc = GetComponents<BoxCollider>();
                            for (int i = 0; i < bc.Length; i++)
                            {
                                bc[i].enabled = true;
                            }
                        }                 
                    }
                }
                else
                {
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
                }

                break;

            case Status.End:
                // サイズを初期状態に戻す
                return true;
        }
    
        return false;
    }

    public override void CookingInterruption()
    {

    }

    /// <summary>
    /// 調理準備
    /// </summary>
    public bool Access(Vector3 accesspos)
    {
        if (!DecisionAccessPoint(accesspos))
        {
            Debug.LogError("アクセスポイントがおかしい");
            return false;
        }

        if (status >= Status.AccessThree)
        {
            Debug.LogError("これ以上アクセスできません！");
            return false;
        }

        status++;
        if (status == Status.AccessThree)
        {
            accessNum = 3;
            status = Status.Open;
            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
        }
        return true;
    }

    /// <summary>
    /// アクセスポイント判定
    /// </summary>
    /// <param name="accesspos"></param>
    /// <returns></returns>
    public bool DecisionAccessPoint(Vector3 accesspos)
    {
        float border = transform.position.z - 0.5f;

        if (accesspos.z < border)
        {
            ChangeBoxColliderEnable(0);
            return true;
        }
        else if (accesspos.x > transform.position.x)
        {
            ChangeBoxColliderEnable(1);
            return true;
        }
        else if (accesspos.x < transform.position.x)
        {
            ChangeBoxColliderEnable(2);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 当たり判定のOnOffを切り替え
    /// </summary>
    /// <param name="element"></param>
    private void ChangeBoxColliderEnable(int element)
    {
        BoxCollider[] bc = GetComponents<BoxCollider>();
        bc[element].enabled = !bc[element].enabled;
    }

    public void ReturnStatus() => status--;

    public void AddAccessNum() => accessNum++;

    public void SubAccessNum() => accessNum--;

    public Status GetStatus() => status;

    public void PutCuisine(GameObject playerHaveCuisine)
    {
        for (int i = 0; i < cuisines.Length; i++)
        {
            if (cuisines[i] == null)
            {
                cuisines[i] = playerHaveCuisine;
                break;
            }
        }
    }

    public bool OneRotation()
    {
        return miniGameUI.GetComponent<MixerMiniGame>().AddPowerPoint();
    }

    public GameObject GetMiniGameUI() => miniGameUI;
}
