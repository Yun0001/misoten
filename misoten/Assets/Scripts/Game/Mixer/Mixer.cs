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


    [SerializeField]
    private Status status = Status.Stand;

    private GameObject lastAccessPlayer;

    private MixerEatoyManager mixerEatoyM_cs;

    private int endFrame;

    [SerializeField]
    private GameObject stickObj;

    [SerializeField]
    private GameObject complateEatoyAnnounce;


    // Use this for initialization
    void Awake () {
        miniGameUI.SetActive(false);
        mixerEatoyM_cs = GetComponent<MixerEatoyManager>();
    }

    private void Update()
    {
        switch (status)
        {

            case Status.Stand:
                if (transform.Find("mixer").GetComponent<mixerAnimCtrl>().GetIsOpen())
                {
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
                }
                break;
            case Status.AccessTwo:

                if (accessNum == (int)Status.AccessTwo)
                {
                    // 調理開始
                    status = Status.Open;
                    mixerEatoyM_cs.SetMixMode(MixerEatoyManager.MixMode.TwoParson);
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
                    mixerEatoyM_cs.HieenEatoySprite();
                }
                break;

            case Status.Open:
                animCount++;
                if (animCount >= openFrame)
                {
                    InitMiniGameUI();
                    status = Status.Play;
                    stickObj.SetActive(true);
                    stickObj.GetComponent<Animator>().Play("rSpin");
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
                if (endFrame > 5)
                {
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
                    status = Status.Stand;
                    endFrame = 0;
                    InitMiniGameUI();
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
        accessNum = 0;
        animCount = 0;
        miniGameUI.GetComponent<MixerMiniGame>().Init();
    }

    protected override void ResetMiniGameUI()
    {
        timeBorder = TIME * 60;
        time = 0;
        isEatoyPut = false;
        status = Status.Stand;
       endFrame = 0;
        accessNum = 0;
        animCount = 0;
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
                // 矢印UIとスティックの回転方向を合わせる
                if (miniGameUI.GetComponent<MixerMiniGame>().GetRotation())
                {
                    stickObj.GetComponent<Animator>().Play("lSpin");
                }

                // 一定時間になるとイートイを渡す
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
                    else if (animCount >= openFrame / 3)
                    {
                        // イートイを出す
                        if (!isEatoyPut)
                        {
                            // ミニゲームUI非表示
                            miniGameUI.SetActive(false);
                            // スティック非表示
                            stickObj.SetActive(false);

                            // イートイを生成し、最後にアクセスしたプレイヤーに渡す
                            lastAccessPlayer.GetComponent<Player>().SetHaveInEatoy(
                              mixerEatoyM_cs.MixEatoy());
                            isEatoyPut = true;

                            // 全ての当たり判定を復活
                            GetComponent<MixerAccessPoint>().RevivalAllAccessPoint();
                            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetBool(false);
                            Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Mixer), false, 7);
                            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Mixerend), 7);
                        }
                    }                
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
        mixerEatoyM_cs.SetEatoy(player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy());

        // アクセスしている人数が二人の時
        if (status == Status.AccessTwo)
        {
            // できるイートイの表示
            int colorId = mixerEatoyM_cs.DecisionTwoParonPutEatoyID();
            complateEatoyAnnounce.SetActive(true);



        }
        // 三人アクセスした時
        else if (status == Status.AccessThree)
        {
            accessNum = 3;
            mixerEatoyM_cs.SetMixMode(MixerEatoyManager.MixMode.ThreeParson);
            status = Status.Open;
            lastAccessPlayer.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Mixer);
            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
            mixerEatoyM_cs.HieenEatoySprite();
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
