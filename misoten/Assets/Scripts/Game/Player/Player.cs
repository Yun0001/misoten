using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;


public class Player : MonoBehaviour
{

    public enum PlayerStatus
    {
        Normal,             //通常
        Microwave,        //電子レンジ
        Pot,                   //鍋
        GrilledTable,       //焼き台
        MixerAccess,        // ミキサーアクセス状態
        MixerWait,          // ミキサー他プレイヤー待ち状態
        Mixer,                 // ミキサー
        IceBox,             // 冷蔵庫
        DastBox,            // ゴミ箱
        CateringIceEatoy,      // 冷凍イートイ
        Catering,           //イートイorチェンジイートイ
    }


    [SerializeField]
    private PlayerStatus playerStatus;

    [SerializeField]
    private int playerID;//プレイヤーID(インスペクターで設定)


    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private CookingMixer cookingMixer_cs;
    private PlayerMove playerMove_cs;
    private PlayerInput playerInput_cs;
    private PlayerHaveInEatoy haveInEatoy_cs;
    private PlayerAccessController playerAccessController_cs;
    private PlayerAccessPossiblAnnounce playerAccessPosssiblAnnounce_cs;
    private PlayerCollision collision_cs;
    private PlayerDastBox pDastbox_cs;
    private PlayerIceBox pIceBox_cs;
    private HaveEatoyCtrl haveEatoyCtrl_cs;

    [SerializeField]
    private GameObject timeManager;

   

    // Use this for initialization
    void Awake()
    {
        SetPlayerStatus(PlayerStatus.Normal);
        SetScript();
        playerInput_cs.Init();
        playerMove_cs.Init();

    }

    /// <summary>
    /// プレイヤー移動
    /// </summary>
    private void FixedUpdate() => playerMove_cs.Move();

    // Update is called once per frame
    void Update()
    {
        if (timeManager.GetComponent<GameTimeManager>().IsTimeUp())
        {
            StopMove();
            return;
        } 
        UpdateBranch();
        playerInput_cs.UpdateInput();
    }

    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
		if (!IsOffer())
		{
			// エイリアンのスクリプトを取得して料理を渡す
			haveInEatoy_cs.SetHaveInEatoyPosition(IsObjectCollision(PlayerCollision.hitObjName.Alien).transform.position);
            IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(haveInEatoy_cs.GetHaveInEatoy());
			GetComponent<PlayerAnimCtrl>().SetServing(false);
            // イートイを表示
            DisplayHaveInEatoy();


            SetPlayerStatus(PlayerStatus.Normal);
		}
    }

    public bool IsOffer()
    {
        return
            AlienStatus.GetCounterStatusChangeFlag(IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT) ||
            AlienStatus.GetCounterStatusChangeFlag(IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_BAD) ||
            AlienStatus.GetCounterStatusChangeFlag(IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD);
    }

    /// <summary>
    /// Bボタン入力
    /// </summary>
    public void ActionBranch()
    {
        foreach (var Name in playerAccessController_cs.GetAccessObjectName())
        {
            // アクセス可能なオブジェクトが見つかった！！
            if (playerAccessController_cs.IsAccessPossible(Name))
            {
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                StopMove(); // 移動値をリセット
                switch (Name)
                {
                    case PlayerAccessController.AccessObjectName.Microwave:      AccessMicrowave();       break;
                    case PlayerAccessController.AccessObjectName.Pot:                 AccessPot();                 break;
                    case PlayerAccessController.AccessObjectName.Flyingpan:        AccessFlyingpan();        break;
                    case PlayerAccessController.AccessObjectName.Mixer:              AccessMixer();              break;
                    case PlayerAccessController.AccessObjectName.IceBox:            AccessIceBox();            break;
                    case PlayerAccessController.AccessObjectName.DastBox:          AccessDastBox();         break;
                    case PlayerAccessController.AccessObjectName.Alien:              OfferCuisine();              break;
                }
                break;
            }
        }
    }


    private void UpdateBranch()
    {
        Mixer mixer_cs;
        switch (playerStatus)
        {
            case PlayerStatus.Microwave:
                playerInput_cs.InputMicrowave();
                cookingMicrowave_cs.UpdateCookingMicrowave();
                break;

            //茹で料理中更新処理
            case PlayerStatus.Pot:
                cookingPot_cs.UpdateCookingPot();
                break;

            // 焼き料理中更新処理
            case PlayerStatus.GrilledTable:
                playerInput_cs.InputGrilled();
                cookingGrilled_cs.UpdateCookingGrilled();
                break;

            case PlayerStatus.MixerAccess:
                mixer_cs = IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>();

                // ミキサーがオープン状態以降は入力不可
                if (mixer_cs.GetStatus() < Mixer.Status.Open)
                {
                    playerInput_cs.InputMixerAccess();
                }
           

                // 自分が3人目としてアクセスした時用
                if (IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.Open)
                {
                    SetPlayerStatus(PlayerStatus.Mixer);
                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                    HiddenAnnounceSprite();
                }
                break;

            case PlayerStatus.MixerWait:
                mixer_cs = IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>();
                // ミキサーがオープン状態以降は入力不可
                if (mixer_cs.GetStatus() < Mixer.Status.Open)
                {
                    playerInput_cs.InputMixerWait();
                }
               
                // 二人でミキサーを利用する時
                if (mixer_cs.GetStatus() == Mixer.Status.Play)
                {
                    SetPlayerStatus(PlayerStatus.Mixer);
                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                    transform.Find("Line").gameObject.SetActive(false);
         
                }
                break;

            case PlayerStatus.Mixer:
                playerInput_cs.InputMixer();
                if (IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                {
                    playerInput_cs.InputMixer();
                    SetPlayerStatus(PlayerStatus.Normal);
                }
                break;

            case PlayerStatus.IceBox:
                // 冷蔵庫状態更新処理
                pIceBox_cs.UpdateIceBox();
                break;

            case PlayerStatus.DastBox:
                // ゴミ箱状態更新処理
                pDastbox_cs.UpdateDastBox();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 調理中断
    /// </summary>
    public void CookingCancel()
    {
        switch (playerStatus)
        {
            case PlayerStatus.Microwave:
                cookingMicrowave_cs.CancelCooking();
                break;

            case PlayerStatus.GrilledTable:
                cookingGrilled_cs.CancelCooking();
                break;

            case PlayerStatus.Pot:
                cookingPot_cs.CancelCooking();
                break;

            default:
                return;
        }
        SetPlayerStatus(PlayerStatus.CateringIceEatoy);
    }

    public void HiddenAnnounceSprite() => playerAccessPosssiblAnnounce_cs.HiddenSprite();

    public void SetAnnounceSprite(int spriteID) => playerAccessPosssiblAnnounce_cs.SetSprite(spriteID);

    public GameObject IsObjectCollision(PlayerCollision.hitObjName ObjID) => collision_cs.GetHitObj(ObjID);

    public void SetHaveInEatoy(GameObject eatoy)
    {
        haveInEatoy_cs.SetEatoy(eatoy);
        SetHaveEatoyCtrlNum((int)GetHaveInEatoyColor());
    }

    public Eatoy.EEatoyColor GetHaveInEatoyColor() => haveInEatoy_cs.GetHaveInEatoy().GetComponent<Eatoy>().GetEatoyColor();

    public void RevocationHaveInEatoy(bool b) => haveInEatoy_cs.RevocationHaveInEatoy(b);

    public PlayerInput GetPlayerInput() => playerInput_cs;

    public int GetPlayerID() => playerID;

    public GameObject GetDastBoxUI() => pDastbox_cs.GetDastBoxUI();

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    private void AccessMicrowave() => cookingMicrowave_cs.CookingStart();


    /// <summary>
    /// 鍋へのアクション
    /// </summary>
    private void AccessPot()
    {
        if (IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>().IsCooking()) return;
        IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>().JoystickInit(playerID);
        cookingPot_cs.CookingStart();
    }

    /// <summary>
    /// フライパンへのアクション
    /// </summary>
    private void AccessFlyingpan() => cookingGrilled_cs.CookingStart();

    /// <summary>
    /// ミキサーへのアクション
    /// </summary>
    private void AccessMixer() => cookingMixer_cs.Preparation();

    private void AccessIceBox() => pIceBox_cs.AccessIceBox();

    private void AccessDastBox() => pDastbox_cs.AccessDastBox();

    public PlayerStatus GetPlayerStatus() => playerStatus;

    private void StopMove()
    {
        playerMove_cs.VelocityReset();
        GetComponent<PlayerAnimCtrl>().SetWalking(false);
    }

    private void SetScript()
    {
        cookingMicrowave_cs = GetComponent<CookingMicrowave>();
        cookingPot_cs = GetComponent<CookingPot>();
        cookingGrilled_cs = GetComponent<CookingGrilled>();
        cookingMixer_cs = GetComponent<CookingMixer>();
        playerMove_cs = GetComponent<PlayerMove>();
        playerInput_cs = GetComponent<PlayerInput>();
        haveInEatoy_cs = GetComponent<PlayerHaveInEatoy>();
        playerAccessController_cs = GetComponent<PlayerAccessController>();
        playerAccessPosssiblAnnounce_cs = GetComponent<PlayerAccessPossiblAnnounce>();
        collision_cs = GetComponent<PlayerCollision>();
        pDastbox_cs = GetComponent<PlayerDastBox>();
        pIceBox_cs = GetComponent<PlayerIceBox>();
        haveEatoyCtrl_cs = GetComponent<HaveEatoyCtrl>();
    }

    public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public void SetHaveEatoyCtrlNum(int num) => haveEatoyCtrl_cs.SetEatoyNum(num);

    private void DisplayHaveInEatoy() => haveInEatoy_cs.DisplayEatoy();

    public bool IsEatoyIceing() => haveInEatoy_cs.GetHaveInEatoy().GetComponent<Eatoy>().IsIcing();

    public void AllNull()
    {
        collision_cs.AllNull();
    }
}
