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
    private int playerID;//プレイヤーID

    [SerializeField]
    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 7).ToArray();// 現在プレイヤーと当たっているオブジェクト

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private CookingMixer cookingMixer_cs;
    private PlayerMove playerMove_cs;
    private PlayerInput playerInput_cs;
    private PlayerHaveInEatoy haveInEatoy_cs;
    private GameObject dastBoxGage;

    // Use this for initialization
    void Awake()
    {
        SetPlayerStatus(PlayerStatus.Normal);
        SetScript();
        playerInput_cs.Init();
        playerMove_cs.Init();
        dastBoxGage = Instantiate(Resources.Load("Prefabs/DastBoxUI") as GameObject, transform.position, Quaternion.identity, transform);
        dastBoxGage.SetActive(false);
    }

    /// <summary>
    /// プレイヤー移動
    /// </summary>
    private void FixedUpdate() => playerMove_cs.Move();

    // Update is called once per frame
    void Update()
    {
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
                break;
            // 鍋
            case "Pot":
                hitObj[(int)hitObjName.Pot] = collision.gameObject;
                break;
            // 焼き台
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = collision.gameObject;
                break;
            // ミキサー
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = collision.gameObject;
                break;

            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = collision.gameObject;
                break;

            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = collision.gameObject;
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
                break;
            case "Pot":
                hitObj[(int)hitObjName.Pot] = null;
                break;
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = null;
                break;
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = null;
                break;
            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = null;
                break;
            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = null;
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
        if (haveInEatoy_cs.GetHaveInEatoy() == null) return;                             // 料理を持っていないならreturn
        if (playerStatus != PlayerStatus.Catering) return;          // 配膳状態でないならreturn
        if (GetHitObj((int)hitObjName.Alien) == null) return;    // 宇宙人との当たり判定がなければreturn

        // エイリアンのスクリプトを取得して料理を渡す
        GetHitObj((int)hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(haveInEatoy_cs.GetHaveInEatoy());
        GetComponent<PlayerAnimCtrl>().SetServing(false);

        haveInEatoy_cs.RevocationHaveInEatoy();
        SetPlayerStatus(PlayerStatus.Normal);
    }

    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    }

    private Microwave GetHitObjComponentMicroWave() => GetHitObj((int)hitObjName.Microwave).GetComponent<Microwave>();

    public int GetPlayerID() => playerID;

    public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    /// <summary>
    /// Bボタン入力
    /// </summary>
    public void ActionBranch()
    {
        AccessMicrowave();
        AccessPot();
        AccessFlyingpan();
        AccessMixer();
        AccessIceBox();
        AccessDastBox();
        OfferCuisine();
    }

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    private void AccessMicrowave()
    {
        if (GetHitObj((int)hitObjName.Microwave) == null) return;   // 電子レンジに当たっていなければreturn
        if (GetPlayerStatus() != PlayerStatus.CateringIceEatoy && GetPlayerStatus() != PlayerStatus.Microwave) return;// 通常状態かレンチン操作状態でなければreturn
        if (GetHitObjComponentMicroWave().IsCooking()) return;
        StopMove(); // 移動値をリセット
        cookingMicrowave_cs.CookingStart();
    }

    /// <summary>
    /// 鍋へのアクション
    /// </summary>
    private void AccessPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjName.Pot) == null) return;
        if (GetPlayerStatus() != PlayerStatus.CateringIceEatoy && GetPlayerStatus() != PlayerStatus.Pot) return;
        StopMove();

        GetHitObj((int)hitObjName.Pot).GetComponent<Pot>().JoystickInit(playerID);
        cookingPot_cs.CookingStart();
    }

    /// <summary>
    /// フライパンへのアクション
    /// </summary>
    private void AccessFlyingpan()
    {
        if (GetPlayerStatus() != PlayerStatus.CateringIceEatoy && GetPlayerStatus() != PlayerStatus.GrilledTable) return;
        if (GetHitObj((int)hitObjName.GrilledTable) == null) return;
        if (GetHitObj((int)hitObjName.GrilledTable).GetComponent<Flyingpan>().IsCooking()) return;
        StopMove();

        cookingGrilled_cs.CookingStart();
    }

    /// <summary>
    /// ミキサーへのアクション
    /// </summary>
    private void AccessMixer()
    {
        if (GetPlayerStatus() != PlayerStatus.Catering) return;
        if (GetHitObj((int)hitObjName.Mixer) == null) return;
        if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus()>= Mixer.Status.AccessThree) return;
        if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().IsCooking()) return;
        StopMove();

        cookingMixer_cs.Preparation();
    }

    private void AccessIceBox()
    {
        if (GetPlayerStatus() != PlayerStatus.Normal) return;
        if (GetHitObj((int)hitObjName.IceBox) == null) return;
        StopMove();

        if (GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().Access(playerID))
        {
            SetPlayerStatus(PlayerStatus.IceBox);
        }
    }

    private void AccessDastBox()
    {
        if (GetPlayerStatus() != PlayerStatus.Catering && GetPlayerStatus() != PlayerStatus.CateringIceEatoy) return;
        if (GetHitObj((int)hitObjName.DastBox) == null) return;
        StopMove();

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



    /// <summary>
    /// プレイヤーIDセット
    /// </summary>
    /// <param name="pID"></param>
    private void SetPlayerID(int pID)
    {
        if (pID < 0 || pID > 3)
        {
            Debug.LogError("不正なplayerID");
            return;
        }
        playerID = pID;
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
    }


    private void UpdateCookingMicrowave()
    {
        GameObject eatoy = cookingMicrowave_cs.UpdateMicrowave();
        if (eatoy == null) return;

        // 料理を持つ
        haveInEatoy_cs.SetEatoy(eatoy);
        SetPlayerStatus(PlayerStatus.Catering);
        GetComponent<PlayerAnimCtrl>().SetServing(true);
        GetHitObj((int)hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetBool(false);

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
        haveInEatoy_cs.SetEatoy(eatoy);
        SetPlayerStatus(PlayerStatus.Catering);
        GetComponent<PlayerAnimCtrl>().SetServing(true);
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
        haveInEatoy_cs.SetEatoy(eatoy);
        SetPlayerStatus(PlayerStatus.Catering);
        GetComponent<PlayerAnimCtrl>().SetServing(true);
        GetHitObj((int)hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
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
                break;

            case PlayerStatus.MixerWait:
                playerInput_cs.InputMixerWait();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.Play)
                {
                    SetPlayerStatus(PlayerStatus.Mixer);
                    // ミキサーに持っている料理を入れる
                    // GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().PutCuisine(haveInHandCusine);
                    //Destroy(haveInHandCusine);
                    //SetHaveInHandCuisine();

                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                }
                break;

            case PlayerStatus.Mixer:
                playerInput_cs.InputMixer();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                {
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
                haveInEatoy_cs.SetHaveInEatoyPosition();
                break;

            case PlayerStatus.CateringIceEatoy:
                // 持っているイートイの座標を更新
                haveInEatoy_cs.SetHaveInEatoyPosition();
                break;

            default:
                break;
        }
    }

    private void UpdateGrilled()
    {

    }


    private void UpdateIceBox()
    {
        playerInput_cs.InputIceBox();
        if (GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().IsPutEatoy() && GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().IsAccessOnePlayer(playerID))
        {
            // イートイを持つ
            haveInEatoy_cs.SetEatoy(GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().PassEatoy());
            SetPlayerStatus(PlayerStatus.CateringIceEatoy);
            GetComponent<PlayerAnimCtrl>().SetServing(true);

            // 冷蔵庫の後処理
            GetHitObj((int)hitObjName.IceBox).GetComponent<IceBox>().ResetEatoy();
            GetHitObj((int)hitObjName.IceBox).transform.Find("icebox").GetComponent<iceboxAnimCtrl>().SetIsOpen(false);
        }
    }

    private void UpdateDastBox()
    {
        // ゴミ箱状態の時の入力
        playerInput_cs.InputDastBox();

        //　ゴミ箱ゲージがMaxの時
        if (dastBoxGage.GetComponent<DastBox>().GetGageAmount() >= 1.0f)
        {
            SetPlayerStatus(PlayerStatus.Normal);
            haveInEatoy_cs.RevocationHaveInEatoy();
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            GetDastBoxUI().SetActive(false);
            Sound.PlaySe(GameSceneManager.seKey[7]);
        }
    }
}
