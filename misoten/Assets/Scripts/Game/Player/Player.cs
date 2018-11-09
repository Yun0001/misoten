﻿using System.Collections;
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
        MixerAccess,
        MixerWait,
        Mixer,
        Catering,           //配膳
        Hindrance,          //邪魔
        Replenishment, // 補充
        TasteCharge,//Burstチャージ中
    }

    public enum hitObjName
    {
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
        Mixer,
        TasteMachine,//旨味成分補充マシーン
        Alien,//宇宙人
        Taste,//旨味成分
        HitObjMax
    }

    private string layerName;// レイヤーの名前

    [SerializeField]
    private PlayerStatus playerStatus;

    // 左スティックの入力を取る用
    private string inputXAxisName;
    private string inputYAxisName;

    private GamePad.Index playerControllerNumber;// コントローラーナンバー
    private int playerID;//プレイヤーID

    [SerializeField]
    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 7).ToArray();// 現在プレイヤーと当たっているオブジェクト

    [SerializeField]
    private GameObject haveInHandCusine;  // 持っている食材

    private readonly static float HINDRANCE_TIME = 3;
    private float hindranceTime = HINDRANCE_TIME; // 邪魔動作の時間

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private CookingMixer cookingMixer_cs;
    private PlayerMove playerMove_cs;
    private HindranceItem hindrance_cs;
    private PlayerAnimation playerAnimation_cs;
    private PlayerInput playerInput_cs;


    // Use this for initialization
    void Awake()
    {
        layerName = LayerMask.LayerToName(gameObject.layer);
        switch (layerName)
        {
            case "Player1":
                SetPlayerID(0);
                SetPlayerControllerNumber(GamePad.Index.One);
                SetInputAxisName("L_XAxis_1", "L_YAxis_1");
                break;
            case "Player2":
                SetPlayerID(1);
                SetPlayerControllerNumber(GamePad.Index.Two);
                SetInputAxisName("L_XAxis_2", "L_YAxis_2");
                break;
            case "Player3":
                SetPlayerID(2);
                SetPlayerControllerNumber(GamePad.Index.Three);
                SetInputAxisName("L_XAxis_3", "L_YAxis_3");
                break;
            case "Player4":
                SetPlayerID(3);
                SetPlayerControllerNumber(GamePad.Index.Four);
                SetInputAxisName("L_XAxis_4", "L_YAxis_4");
                break;
        }

        SetPlayerStatus(PlayerStatus.Normal);
        SetScript();
        playerInput_cs.Init(inputXAxisName, inputYAxisName);
        playerMove_cs.Init();
    }

    private void FixedUpdate() => playerMove_cs.Move();

    // Update is called once per frame
    void Update()
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
                    GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().PutCuisine(haveInHandCusine);
                    SetHaveInHandCuisine();

                    playerAnimation_cs.SetIsCatering(false);
                }
                break;

            case PlayerStatus.Mixer:
                playerInput_cs.InputMixer();
                if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                {
                    SetPlayerStatus(PlayerStatus.Normal);
                }
                break;

            case PlayerStatus.Hindrance:
                UpdateHindrance();
                break;

            default:
                break;
        }
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
            // 補充マシーン
            case "TasteMachine":
                // 味の素補充
                hindrance_cs.ReplenishmentTaste();
                hitObj[(int)hitObjName.TasteMachine] = collision.gameObject;
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
            case "TasteMachine":
                hitObj[(int)hitObjName.TasteMachine] = null;
                break;

            case "Alien":
                hitObj[(int)hitObjName.Alien] = null;
                break;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        //ここに旨味成分に当たっているときの処理
        if (playerStatus != PlayerStatus.Catering) return;
        if (collision.tag != "Taste") return;
        if (collision.gameObject.GetComponent<Taste>().playerID == playerID) return;

        //配膳中かつ当たっているものが旨味成分かつ旨味成分が自分のものではないとき
        // 配膳中の料理の旨味を向上させる
        haveInHandCusine.GetComponent<Food>().SubQualityTaste();
    }


    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
        if (haveInHandCusine == null) return;                             // 料理を持っていないならreturn
        if (playerStatus != PlayerStatus.Catering) return;          // 配膳状態でないならreturn
        if (GetHitObj((int)hitObjName.Alien) == null) return;    // 宇宙人との当たり判定がなければreturn

        // エイリアンのスクリプトを取得して料理を渡す
        GetHitObj((int)hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(haveInHandCusine);
        // 料理コントローラーが新たに料理を出せるようにする
        CuisineControllerOfferCuisine();
        playerAnimation_cs.SetIsCatering(false);

        SetHaveInHandCuisine();
        SetPlayerStatus(PlayerStatus.Normal);
    }


    /// <summary>
    /// 料理を持つ
    /// </summary>
    public void WithaCuisine(GameObject cuisine)
    {
        SetHaveInHandCuisine(cuisine);
        SetPlayerStatus(PlayerStatus.Catering);
    }

    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    }

    private Microwave GetHitObjComponentMicroWave() => GetHitObj((int)hitObjName.Microwave).GetComponent<Microwave>();

    public int GetPlayerID() => playerID;

    public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public GamePad.Index GetPlayerControllerNumber() => playerControllerNumber;

    /// <summary>
    /// Bボタン入力
    /// </summary>
    public void ActionBranch()
    {
        AccessMicrowave();
        AccessPot();
        AccessFlyingpan();
        AccessMixer();
        OfferCuisine();
    }

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    public void AccessMicrowave()
    {
        if (GetHitObj((int)hitObjName.Microwave) == null) return;   // 電子レンジに当たっていなければreturn
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Microwave) return;// 通常状態かレンチン操作状態でなければreturn
        if (GetHitObjComponentMicroWave().IsCooking()) return;
        StopMove(); // 移動値をリセット
        cookingMicrowave_cs.CookingStart();
    }

    /// <summary>
    /// 鍋へのアクション
    /// </summary>
    public void AccessPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjName.Pot) == null) return;
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Pot) return;
        StopMove();

        cookingPot_cs.CookingStart();
    }

    /// <summary>
    /// フライパンへのアクション
    /// </summary>
    public void AccessFlyingpan()
    {
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.GrilledTable) return;
        if (GetHitObj((int)hitObjName.GrilledTable) == null) return;
        if (GetHitObj((int)hitObjName.GrilledTable).GetComponent<Flyingpan>().IsCooking()) return;
        StopMove();

        cookingGrilled_cs.CookingStart();
    }

    /// <summary>
    /// ミキサーへのアクション
    /// </summary>
    public void AccessMixer()
    {
        if (GetPlayerStatus() != PlayerStatus.Catering) return;
        if (GetHitObj((int)hitObjName.Mixer) == null) return;
        if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().GetStatus()>= Mixer.Status.AccessThree) return;
        if (GetHitObj((int)hitObjName.Mixer).GetComponent<Mixer>().IsCooking()) return;
        StopMove();

        cookingMixer_cs.Preparation();
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
        SetPlayerStatus(PlayerStatus.Normal);
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


    private void SetInputAxisName(string XAxisName, string YAxisName)
    {
        inputXAxisName = XAxisName;
        inputYAxisName = YAxisName;
    }

    private void SetPlayerControllerNumber(GamePad.Index index)
    {
        if (index < GamePad.Index.One || index > GamePad.Index.Four)
        {
            Debug.LogError("不正なコントローラindex");
            return;
        }
        playerControllerNumber = index;
    }


    private void SetScript()
    {
        cookingMicrowave_cs = GetComponent<CookingMicrowave>();
        cookingPot_cs = GetComponent<CookingPot>();
        cookingGrilled_cs = GetComponent<CookingGrilled>();
        cookingMixer_cs = GetComponent<CookingMixer>();
        playerMove_cs = GetComponent<PlayerMove>();
        hindrance_cs = GetComponent<HindranceItem>();
        playerAnimation_cs = GetComponent<PlayerAnimation>();
        playerInput_cs = GetComponent<PlayerInput>();
    }

    public string GetInputXAxisName() => inputXAxisName;

    public string GetInputYAxisName() => inputYAxisName;

    private void UpdateCookingMicrowave()
    {
        GameObject cuisine = cookingMicrowave_cs.UpdateMicrowave();
        if (cuisine == null) return;

        // 料理を持つ
        WithaCuisine(cuisine);
        playerAnimation_cs.SetIsCatering(true);
        GetHitObj((int)hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetBool(false);

    }

    /// <summary>
    /// 茹で料理の更新処理
    /// </summary>
    private void UpdateCookingPot()
    {
        // スティック一周ができればcuisineはnullでない
        GameObject cuisine = cookingPot_cs.UpdatePot();
        if (cuisine == null) return;

        // 料理を持つ
        WithaCuisine(cuisine);
        playerAnimation_cs.SetIsCatering(true);
        GetHitObj((int)hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }

    /// <summary>
    /// 焼き料理の更新処理
    /// </summary>
    private void UpdateCookingGrilled()
    {
        GameObject cuisine = cookingGrilled_cs.UpdateGrilled();
        if (cuisine == null) return;

        // 焼く調理終了の処理
        WithaCuisine(cuisine);
        playerAnimation_cs.SetIsCatering(true);
        GetHitObj((int)hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }


    /// <summary>
    /// 邪魔状態の更新処理
    /// </summary>
    private void UpdateHindrance()
    {
        // 邪魔状態の時の処理
        hindranceTime -= Time.deltaTime;
        if (hindranceTime < 0)
        {
            hindranceTime = HINDRANCE_TIME;
            SetPlayerStatus(PlayerStatus.Normal);
        }
    }

    private void CuisineControllerOfferCuisine()
    {
        switch (haveInHandCusine.GetComponent<Food>().GetCategory())
        {
            case Food.Category.Grilled:
                CuisineManager.GetInstance().GetGrilledController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;

            case Food.Category.Pot:
                CuisineManager.GetInstance().GetPotController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;

            case Food.Category.Microwave:
                CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;
        }
    }

    private void SetHaveInHandCuisine(GameObject Cuisine = null)
    {
        haveInHandCusine = Cuisine;
    }

    private void StopMove()
    {
        playerMove_cs.VelocityReset();
        playerAnimation_cs.SetPlayerStatus(0);
    }
}
