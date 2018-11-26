using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;

/*
 * エイリアンにイートイを渡した時のスコア獲得演算式
 * イートイポイント＊エイリアン満足度＝獲得スコア
 * エイリアン満足度
 * ロウ１
 * ミドル２
 * ハイ５
 * オーバー１５
 * */



public class Player : MonoBehaviour
{

    public enum PlayerStatus
    {
        Normal,             //通常
        Microwave,        //電子レンジ
        Pot,                   //鍋
        GrilledTable,       //焼き台
        MixerAccess,
        MixerWait,
        Mixer,
        IceBox,
        DastBox,
        CateringIceEatoy,
        Catering,           //配膳
    }

    public enum hitObjName
    {
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
        Mixer,
        IceBox,
        DastBox,
        Alien,//宇宙人
        HitObjMax
    }

    [SerializeField]
    private PlayerStatus playerStatus;

    [SerializeField]
    private int playerID;//プレイヤーID(インスペクターで設定)

    [SerializeField]
    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 7).ToArray();// 現在プレイヤーと当たっているオブジェクト

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private CookingMixer cookingMixer_cs;
    private PlayerMove playerMove_cs;
    private PlayerInput playerInput_cs;
    private PlayerHaveInEatoy haveInEatoy_cs;
    private PlayerAccessController playerAccessController_cs;
    private PlayerAccessPossiblAnnounce playerAccessPosssiblAnnounce_cs;
    private GameObject dastBoxGage;

    [SerializeField]
    private GameObject timeManager;

   

    // Use this for initialization
    void Awake()
    {
        SetPlayerStatus(PlayerStatus.Normal);
        SetScript();
        playerInput_cs.Init();
        playerMove_cs.Init();
        dastBoxGage = Instantiate(Resources.Load("Prefabs/DastBoxUI") as GameObject, transform.position, Quaternion.identity, transform);
        dastBoxGage.SetActive(false);
        //transform.Find("Line").gameObject.SetActive(false);
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
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            // レンジ
            case "Microwave":
                hitObj[(int)hitObjName.Microwave] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.CateringIceEatoy)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.Microwave);
                }
                break;
            // 鍋
            case "Pot":
                hitObj[(int)hitObjName.Pot] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.CateringIceEatoy)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.Pot);
                }        
                break;
            // 焼き台
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.CateringIceEatoy)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.GrilledTable);
                }           
                break;
            // ミキサー
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.Catering)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.Mixer);
                }
                break;

            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.Normal)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.IceBox);
                }
                break;

            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = collision.gameObject;
                if (GetComponent<Player>().GetPlayerStatus() == PlayerStatus.Catering ||
                    GetComponent<Player>().GetPlayerStatus() == PlayerStatus.CateringIceEatoy)
                {
                    playerAccessPosssiblAnnounce_cs.SetSprite((int)hitObjName.DastBox);
                }
                break;
            case "Alien":
                hitObj[(int)hitObjName.Alien] = collision.gameObject;
                break;
        }
    }

    /// <summary>
    /// 当たり判定がなくなるとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Microwave":
                hitObj[(int)hitObjName.Microwave] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "Pot":
                hitObj[(int)hitObjName.Pot] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = null;
                playerAccessPosssiblAnnounce_cs.HiddenSprite();
                break;
            case "Alien":
                hitObj[(int)hitObjName.Alien] = null;
                break;
        }
    }

    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
		if (!AlienStatus.GetCounterStatusChangeFlag(GetHitObj((int)hitObjName.Alien).GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT))
		{
			// エイリアンのスクリプトを取得して料理を渡す
			haveInEatoy_cs.SetHaveInEatoyPosition(GetHitObj((int)hitObjName.Alien).transform.position);
			GetHitObj((int)hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(haveInEatoy_cs.GetHaveInEatoy());
			GetComponent<PlayerAnimCtrl>().SetServing(false);

			//haveInEatoy_cs.RevocationHaveInEatoy();
			SetPlayerStatus(PlayerStatus.Normal);
		}
    }

    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    }

	public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

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

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    private void AccessMicrowave() => cookingMicrowave_cs.CookingStart();
    

    /// <summary>
    /// 鍋へのアクション
    /// </summary>
    private void AccessPot()
    {
        GetHitObj((int)hitObjName.Pot).GetComponent<Pot>().JoystickInit(playerID);
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

    private void AccessIceBox()
    {
        if (GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().Access(playerID))
        {
            SetPlayerStatus(PlayerStatus.IceBox);
        }
    }

    private void AccessDastBox()
    {
        dastBoxGage.SetActive(true);
        dastBoxGage.GetComponent<DastBox>().Access((int)playerStatus, transform.position);
        SetPlayerStatus(PlayerStatus.DastBox);
    }

    public PlayerStatus GetPlayerStatus() => playerStatus;


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
    }


    private void StopMove()
    {
        playerMove_cs.VelocityReset();
        GetComponent<PlayerAnimCtrl>().SetWalking(false);
    }

    public GameObject GetDastBoxUI() => dastBoxGage;

    private void UpdateBranch()
    {
        switch (playerStatus)
        {
            case PlayerStatus.Microwave:
                playerInput_cs.InputMicrowave();
                UpdateCookingMicrowave();
                break;

            //茹で料理中更新処理
            case PlayerStatus.Pot:
                UpdateCookingPot();
                break;

            // 焼き料理中更新処理
            case PlayerStatus.GrilledTable:
                playerInput_cs.InputGrilled();
                UpdateCookingGrilled();
                break;

            case PlayerStatus.MixerAccess:
                playerInput_cs.InputMixerAccess();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.Play)
                {
                    SetPlayerStatus(PlayerStatus.Mixer);
                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                }
                break;

            case PlayerStatus.MixerWait:
                playerInput_cs.InputMixerWait();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.Play)
                {
                    SetPlayerStatus(PlayerStatus.Mixer);
                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                    transform.Find("Line").gameObject.SetActive(false);
                }
                break;

            case PlayerStatus.Mixer:
                playerInput_cs.InputMixer();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                {
                    playerAccessPosssiblAnnounce_cs.HiddenSprite();
                    SetPlayerStatus(PlayerStatus.Normal);
                }
                break;

            case PlayerStatus.IceBox:
                // 冷蔵庫状態更新処理
                UpdateIceBox();

                break;

            case PlayerStatus.DastBox:
                // ゴミ箱状態更新処理
                UpdateDastBox();
                break;

            case PlayerStatus.Catering:
                // 持っているイートイの座標を更新
                haveInEatoy_cs.UpdateHaveInEatoyPosition();
                break;

            case PlayerStatus.CateringIceEatoy:
                // 持っているイートイの座標を更新
                haveInEatoy_cs.UpdateHaveInEatoyPosition();
                break;

            default:
                break;
        }
    }


    private void UpdateCookingMicrowave()
    {
        GameObject eatoy = cookingMicrowave_cs.UpdateMicrowave();
        if (eatoy == null) return;

        // 料理を持つ
        playerAccessPosssiblAnnounce_cs.HiddenSprite();
        haveInEatoy_cs.SetEatoy(eatoy);
        GetHitObj((int)hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(true);
        // レンジOpenSE
        //Sound.PlaySe(GameSceneManager.seKey[16], 4);
    }

    /// <summary>
    /// 茹で料理の更新処理
    /// </summary>
    private void UpdateCookingPot()
    {
        // スティック一周ができればcuisineはnullでない
        GameObject eatoy = cookingPot_cs.UpdatePot();
        if (eatoy == null) return;

        // 料理を持つ
        playerAccessPosssiblAnnounce_cs.HiddenSprite();
        haveInEatoy_cs.SetEatoy(eatoy);
        GetHitObj((int)hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }

    /// <summary>
    /// 焼き料理の更新処理
    /// </summary>
    private void UpdateCookingGrilled()
    {
        GameObject eatoy = cookingGrilled_cs.UpdateGrilled();
        if (eatoy == null) return;

        // 焼く調理終了の処理 
        playerAccessPosssiblAnnounce_cs.HiddenSprite();
        haveInEatoy_cs.SetEatoy(eatoy);
        GetHitObj((int)hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }


    private void UpdateIceBox()
    {
        playerInput_cs.InputIceBox();
        if (GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().IsPutEatoy() && GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().IsAccessOnePlayer(playerID))
        {
            // イートイを持つ
            playerAccessPosssiblAnnounce_cs.HiddenSprite();
            haveInEatoy_cs.SetEatoy(GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().PassEatoy());

            // 冷蔵庫の後処理
            GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().ResetEatoy();
            if (GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().GetIceBoxID() == 0)
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.RefrigeratorSuccess), 5);
            }
            else
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.RefrigeratorSuccess), 6);
            }
        }
    }

    private void UpdateDastBox()
    {
        // ゴミ箱状態の時の入力
        playerInput_cs.InputDastBox();

        //　ゴミ箱ゲージがMaxの時
        if (dastBoxGage.GetComponent<DastBox>().GetGageAmount() >= 1.0f)
        {
            playerAccessPosssiblAnnounce_cs.HiddenSprite();
            GetHitObj((int)hitObjName.DastBox).transform.Find("box").GetComponent<mwAnimCtrl>().SetIsOpen(true);
            SetPlayerStatus(PlayerStatus.Normal);
            haveInEatoy_cs.RevocationHaveInEatoy(true);
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            GetDastBoxUI().SetActive(false);
            Sound.SetVolumeSe(GameSceneManager.seKey[7], 0.3f, 8);
            Sound.PlaySe(GameSceneManager.seKey[7],8);
        }
    }
}
