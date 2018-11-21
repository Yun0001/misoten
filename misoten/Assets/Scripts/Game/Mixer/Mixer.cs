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


    [SerializeField]
    private Status status = Status.Stand;

    private GameObject lastAccessPlayer;

    private MixerEatoyManager mixerEatoyM_cs;

    private int endFrame;


    // Use this for initialization
    void Awake () {
        miniGameUI = Instantiate(Resources.Load("Prefabs/MixerMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.SetActive(false);
        mixerEatoyM_cs = GetComponent<MixerEatoyManager>();
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
                    mixerEatoyM_cs.SetMixMode(MixerEatoyManager.MixMode.TwoParson);
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
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetBool(true);
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
            case Status.End:
                endFrame++;
                if (endFrame < 5)
                {
                    status = Status.Stand;
                    endFrame = 0;
                }
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
        endFrame = 0;

    }

    protected override void ResetMiniGameUI()
    {
        timeBorder = TIME * 60;
        time = 0;
        isEatoyPut = false;
        status = Status.Stand;
       endFrame = 0;
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

                            // イートイを生成し、最後にアクセスしたプレイヤーに渡す
                            lastAccessPlayer.GetComponent<PlayerHaveInEatoy>().SetEatoy(
                                GetComponent<MixerEatoyManager>().MixEatoy());
                            isEatoyPut = true;

                            // 全ての当たり判定を復活
                            GetComponent<MixerAccessPoint>().RevivalAllAccessPoint();
                            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetBool(false);
                        }
                    }                
                    
                }
                else
                {
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
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
    public bool Access(GameObject player)
    {
        if (status >= Status.AccessThree)
        {
            Debug.LogError("これ以上アクセスできません！");
            return false;
        }

        status++;
        lastAccessPlayer = player;
        GetComponent<MixerEatoyManager>().SetEatoy(player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy());

        // 三人アクセスした時
        if (status == Status.AccessThree)
        {
            accessNum = 3;
            mixerEatoyM_cs.SetMixMode(MixerEatoyManager.MixMode.ThreeParson);
            status = Status.Open;
            lastAccessPlayer.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Mixer);
            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
        }
        return true;
    }


    public void ReturnStatus() => status--;

    public void AddAccessNum() => accessNum++;

    public void SubAccessNum() => accessNum--;

    public Status GetStatus() => status;

    public bool OneRotation()
    {
        return miniGameUI.GetComponent<MixerMiniGame>().AddPowerPoint();
    }

    /// <summary>
    /// 使ってない
    /// </summary>
    /// <returns></returns>
    protected override int CalcEatoyPoint()
    {
        return 0;
    }

    public int GetMiniGamePoint() => miniGameUI.GetComponent<MixerMiniGame>().GetPowerPoint();
}
