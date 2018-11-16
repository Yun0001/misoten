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

    private enum MixMode
    {
        None,
        TwoParson,
        ThreeParson
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

    [SerializeField]
    private GameObject eatoyPrefab;
    private Sprite[] eatoySprite;

    [SerializeField]
    private Status status = Status.Stand;

    [SerializeField]
    private GameObject[] eatoies;

    private MixMode mixMode;

    private GameObject lastAccessPlayer;


    // Use this for initialization
    void Awake () {
        mixMode = MixMode.None;
        miniGameUI = Instantiate(Resources.Load("Prefabs/MixerMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.SetActive(false);
        eatoySprite = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
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
                    mixMode = MixMode.TwoParson;
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
                            GameObject putEatoy = Instantiate(eatoyPrefab, transform.position, Quaternion.identity);

                            int eatoyID = 0;
                            // ToDo 人数と入れたイートイの判定をする
                            switch (mixMode)
                            {
                                case MixMode.TwoParson:
                                    eatoyID = DecisionTwoParonPutEatoyID();
                                    break;

                                case MixMode.ThreeParson:
                                    eatoyID = DecisionThreeParsonPutEatoyID();
                                    break;

                                default:
                                    Debug.LogError("不正な状態");
                                    break;
                            }

                            putEatoy.GetComponent<Eatoy>().Init(eatoyID, eatoySprite[eatoyID - 1]);
                            //
                            isEatoyPut = true;

                            Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
                            putEatoy.transform.localScale = scale;
                            putEatoy.transform.position = lastAccessPlayer.transform.position;
                            lastAccessPlayer.GetComponent<Player>().WithaCuisine(putEatoy);

                            // 料理を無くす
                            for (int i = 0; i < eatoies.Length; i++)
                            {
                                if (eatoies[i] != null)
                                {
                                    Destroy(eatoies[i]);
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
        // ミキサーはキャンセルなし
    }

    /// <summary>
    /// 調理準備
    /// </summary>
    public bool Access(Vector3 accesspos, GameObject player)
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
        lastAccessPlayer = player;
        PutCuisine(player.GetComponent<Player>().GetHaveInHandCuisine());
        if (status == Status.AccessThree)
        {
            accessNum = 3;
            mixMode = MixMode.ThreeParson;
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
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] == null)
            {
                eatoies[i] = playerHaveCuisine;
                break;
            }
        }
    }

    public bool OneRotation()
    {
        return miniGameUI.GetComponent<MixerMiniGame>().AddPowerPoint();
    }

    public GameObject GetMiniGameUI() => miniGameUI;

    protected override int CalcEatoyPoint()
    {
        int sum = 0;
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] != null)
            {
                sum += eatoies[i].GetComponent<Eatoy>().GetEatoyPoint();
            }
        }
        return miniGameUI.GetComponent<MixerMiniGame>().GetPowerPoint() * sum;
    }

    private int DecisionTwoParonPutEatoyID()
    {
        Eatoy.EEatoyColor[] eatoyColor = new Eatoy.EEatoyColor[2];
        for (int i = 0; i < 2; i++)
        {
            eatoyColor[i] = eatoies[i].GetComponent<Eatoy>().GetEatoyColor();
        }

        // 同じ色だった場合
        if (eatoyColor[0] == eatoyColor[1])
        {
            return (int)eatoyColor[0];
        }
        // 違う色
        else
        {
            switch ((int)eatoyColor[0] + (int)eatoyColor[1])
            {
                // オレンジ
                case 4:
                    return  (int)Eatoy.EEatoyColor.Orange;
                // 緑
                case 6:
                    return  (int)Eatoy.EEatoyColor.Green;
                // 紫
                case 8:
                    return  (int)Eatoy.EEatoyColor.Purple;
            }
        }

        Debug.LogError("不正な値");
        return 0;
    }

    private int DecisionThreeParsonPutEatoyID()
    {
        Eatoy.EEatoyColor[] eatoyColor = new Eatoy.EEatoyColor[3];
        for (int i = 0; i < 3; i++)
        {
            eatoyColor[i] = eatoies[i].GetComponent<Eatoy>().GetEatoyColor();
        }

        if ((eatoyColor[0] == eatoyColor[1]) && (eatoyColor[0] == eatoyColor[2]))
        {
            return (int)eatoyColor[0];
        }
        else
        {
            switch (eatoyColor[2])
            {
                case Eatoy.EEatoyColor.Yellow:
                    return (int)Eatoy.EEatoyColor.Orange;

                case Eatoy.EEatoyColor.Red:
                    return (int)Eatoy.EEatoyColor.Purple;

                case Eatoy.EEatoyColor.Bule:
                    return (int)Eatoy.EEatoyColor.Green;
            }
        }

        Debug.LogError("不正な値");
        return 0;
    }
}
