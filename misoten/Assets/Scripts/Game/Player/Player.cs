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
        Catering,           //配膳
        Hindrance,          //邪魔
        Replenishment, // 補充
        TasteCharge,//Burstチャージ中
        Explosion//爆発
    }

    public enum hitObjName
    {
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
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
    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 5).ToArray();// 現在プレイヤーと当たっているオブジェクト

    [SerializeField]
    private GameObject haveInHandCusine;  // 持っている食材

    private readonly static float HINDRANCE_TIME = 3;
    private float hindranceTime = HINDRANCE_TIME; // 邪魔動作の時間

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private PlayerMove playerMove_cs;
    private HindranceItem hindrance_cs;
    private PlayerAnimation playerAnimation_cs;
    private PlayerInput playerInput_cs;

    [SerializeField]
    private int hitAlienID;
    

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

    private void FixedUpdate()
    {
        if (playerStatus == PlayerStatus.Pot) return; // 茹で料理を調理中ならばリターン

        playerMove_cs.Move();
    }

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
                UpdateCookingGrilled();
                break;

            case PlayerStatus.Hindrance:
                UpdateHindrance();
                break;

            default:
                break;
        }
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
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
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

        SetHaveInHanCuisine();
        SetPlayerStatus(PlayerStatus.Normal);
    }

    
    /// <summary>
    /// 料理を持つ
    /// </summary>
    public void WithaCuisine(GameObject cuisine)
    {
        SetHaveInHanCuisine(cuisine);
        SetPlayerStatus(PlayerStatus.Catering);
    }
    
    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    } 

    private Microwave GetHitObjComponentMicroWave() => GetHitObj((int)hitObjName.Microwave).GetComponent<Microwave>();
    
    private Pot GetHitObjComponetPot() => GetHitObj((int)hitObjName.Pot).GetComponent<Pot>();

    private Grilled GetHitObjComponentGrilled() => GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>();

    public int GetPlayerID() => playerID;

    public  void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public GamePad.Index GetPlayerControllerNumber() => playerControllerNumber;

    /// <summary>
    /// Bボタン入力
    /// </summary>
    public void ActionBranch()
    {
        ActionMicrowave();
        ActionPot();
        ActionGrilled();
        OfferCuisine();
    }

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    public void ActionMicrowave()
    {
        if (GetHitObj((int)hitObjName.Microwave) == null) return;   // 電子レンジに当たっていなければreturn
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Microwave) return;// 通常状態かレンチン操作状態でなければreturn
        if (GetHitObjComponentMicroWave().IsCooking()) return;
        ResetMove(); // 移動値をリセット
        cookingMicrowave_cs.PresstheMicrowaveStartButton();

    }

    public void ActionPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjName.Pot) == null) return;
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Pot) return;
        ResetMove();

        cookingPot_cs.CookingStart();
    }

    public void ActionGrilled()
    {
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.GrilledTable) return;
        if (GetHitObj((int)hitObjName.GrilledTable) == null) return;
        if (GetHitObjComponentGrilled().IsCooking()) return;
        ResetMove();

        cookingGrilled_cs.OnFire();
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
                SetPlayerStatus(PlayerStatus.Normal);
                break;

            case PlayerStatus.GrilledTable:
                cookingGrilled_cs.CancelCooking();
                SetPlayerStatus(PlayerStatus.Normal);
                break;

            case PlayerStatus.Pot:
                cookingPot_cs.CancelCooking();
                SetPlayerStatus(PlayerStatus.Normal);
                break;

        }
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
        playerMove_cs = GetComponent<PlayerMove>();
        hindrance_cs = GetComponent<HindranceItem>();
        playerAnimation_cs = GetComponent<PlayerAnimation>();
        playerInput_cs = GetComponent<PlayerInput>();
    }

    public string GetInputXAxisName() => inputXAxisName;

    public string GetInputYAxisName() => inputYAxisName;

    private void UpdateCookingMicrowave()
    {
        GameObject cuisine = null;
        cuisine = cookingMicrowave_cs.UpdateMicrowave();
        if (cuisine != null)
        {
            // 料理を持つ
            WithaCuisine(cuisine);
            playerAnimation_cs.SetIsCatering(true);
        }
    }

    /// <summary>
    /// 茹で料理の更新処理
    /// </summary>
    private void UpdateCookingPot()
    {
        // スティック一周ができればcuisineはnullでない
        GameObject cuisine = null;
        cuisine = cookingPot_cs.Mix();
        if (cuisine != null)
        {
            // 料理を持つ
            WithaCuisine(cuisine);
            playerAnimation_cs.SetIsCatering(true);
        }
    }

    /// <summary>
    /// 焼き料理の更新処理
    /// </summary>
    private void UpdateCookingGrilled()
    {
        if (GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>().IsEndCooking())
        {
            // 焼く調理終了の処理
            WithaCuisine(GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>().GrilledCookingEnd());
            playerAnimation_cs.SetIsCatering(true);
            GetHitObj((int)hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
        }
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
            case 0:
                CuisineManager.GetInstance().GetGrilledController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;
            case 1:
                CuisineManager.GetInstance().GetPotController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;

            case 2:
                CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(haveInHandCusine.GetComponent<Food>().GetFoodID());
                break;
        }
    }

    private void SetHaveInHanCuisine(GameObject Cuisine = null)
    {
        haveInHandCusine = Cuisine;
    }

    private void ResetMove()
    {
        playerMove_cs.MoveReset();
        playerMove_cs.VelocityReset();
        playerAnimation_cs.SetPlayerStatus(0);
    }

    public void SetHitAlienID(int aID) => hitAlienID = aID;
}
