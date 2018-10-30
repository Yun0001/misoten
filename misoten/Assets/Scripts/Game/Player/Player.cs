using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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

    private GamePad.Index PlayerControllerNumber;// コントローラーナンバー
    private int playerID;//プレイヤーID
    [SerializeField]
    private GameObject[] hitObj = new GameObject[(int)hitObjName.HitObjMax];// 現在プレイヤーと当たっているオブジェクト

    [SerializeField]
    private GameObject haveInHandFood;  // 持っている食材

    private readonly static float HINDRANCE_TIME = 3;
    private float hindranceTime = HINDRANCE_TIME; // 邪魔動作の時間

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    private PlayerMove playerMove_cs;
    private HindranceItem hindrance_cs;
    private PlayerAnimation playerAnimation_cs;
    

    // Use this for initialization
    void Awake()
    {
        layerName = LayerMask.LayerToName(gameObject.layer);
        switch (layerName)
        {
            case "Player1":
                playerID = 0;
                PlayerControllerNumber = GamePad.Index.One;
                inputXAxisName = "L_XAxis_1";
                inputYAxisName = "L_YAxis_1";
                break;
            case "Player2":
                playerID = 1;
                PlayerControllerNumber = GamePad.Index.Two;
                inputXAxisName = "L_XAxis_2";
                inputYAxisName = "L_YAxis_2";
                break;
            case "Player3":
                playerID = 2;
                PlayerControllerNumber = GamePad.Index.Three;
                inputXAxisName = "L_XAxis_3";
                inputYAxisName = "L_YAxis_3";
                break;
            case "Player4":
                playerID = 3;
                PlayerControllerNumber = GamePad.Index.Four;
                inputXAxisName = "L_XAxis_4";
                inputYAxisName = "L_YAxis_4";
                break;
        }

        playerStatus = PlayerStatus.Normal;
        for (int i = 0; i < hitObj.Length; i++)
        {
            hitObj[i] = null;
        }
        cookingMicrowave_cs = GetComponent<CookingMicrowave>();
        cookingPot_cs=GetComponent<CookingPot>();
        cookingGrilled_cs = GetComponent<CookingGrilled>();
        GetComponent<PlayerInput>().Init(inputXAxisName, inputYAxisName);
        playerMove_cs = GetComponent<PlayerMove>();
        playerMove_cs.Init();
        hindrance_cs=GetComponent<HindranceItem>();
        playerAnimation_cs = GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        if (playerStatus == PlayerStatus.Pot) return; // 茹で料理を調理中ならばリターン

        playerMove_cs.Move();
    }

    // Update is called once per frame
    void Update ()
    {
        if (playerStatus == PlayerStatus.Hindrance)
        {
            // 邪魔状態の時の処理
            hindranceTime -= Time.deltaTime;
            if (hindranceTime < 0)
            {
                hindranceTime = HINDRANCE_TIME;
                SetPlayerStatus(PlayerStatus.Normal);
            }
        }
        else
        {
            //　邪魔状態以外の時の処理
            switch (playerStatus)
            {
                case PlayerStatus.Pot:
                    //茹で料理を調理中の処理
                    GameObject cuisine = null;
                    cuisine = cookingPot_cs.Mix();
                    if (cuisine != null) WithaCuisine(cuisine);
                    break;

                case PlayerStatus.GrilledTable:
                    if (GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>().IsEndCooking())
                    {
                        // 焼く調理終了の処理
                        WithaCuisine(GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>().GrilledCookingEnd());
                     GetHitObj((int)hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);

                    }
                    break;

                    //電子レンジの爆発状態
                case PlayerStatus.Microwave:
                    if (GetHitObjComponentMicroWave().GetStatus() == MicroWave.EMWState.Explostion)
                    {
                        SetPlayerStatus(PlayerStatus.Explosion);
                    }
                    break;
                case PlayerStatus.Explosion:
                    if (GetHitObjComponentMicroWave().GetStatus() == MicroWave.EMWState.SwitchOff)
                    {
                        SetPlayerStatus(PlayerStatus.Normal);
                    }
                    break;
                default:
                    
                    break;
            }
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
        haveInHandFood.GetComponent<Food>().SubQualityTaste();        


        
        // これで、カウンター側の客（０番目）の待機状態が取れる
        AlienStatus.GetCounterStatusChangeFlag(0, (int)AlienStatus.EStatus.STAND);

        // これで、持ち帰り側の客（０番目）の待機状態が取れる
        AlienStatus.GetTakeOutStatusChangeFlag(0, (int)AlienStatus.EStatus.STAND);
        

        // GetHitObj((int)hitObjName.Alien).GetComponent<Alien>
    }


    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
        if (haveInHandFood == null) return;
        if (playerStatus != PlayerStatus.Catering) return;
        if (GetHitObj((int)hitObjName.Alien) == null) return;


        // エイリアンのスクリプトを取得して料理を渡す
        GetHitObj((int)hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(haveInHandFood);
        switch (haveInHandFood.GetComponent<Food>().GetCategory())
        {
            case 0:
                CuisineManager.GetInstance().GetGrilledController().OfferCuisine(haveInHandFood.GetComponent<Food>().GetFoodID());
                break;
            case 1:
                CuisineManager.GetInstance().GetPotController().OfferCuisine(haveInHandFood.GetComponent<Food>().GetFoodID());
                break;

            case 2:
                CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(haveInHandFood.GetComponent<Food>().GetFoodID());
                break;
        }
       
        haveInHandFood = null;
        playerStatus = PlayerStatus.Normal;
    }

    
    /// <summary>
    /// 料理を持つ
    /// </summary>
    public void WithaCuisine(GameObject Food)
    {
        haveInHandFood = Food;
        SetPlayerStatus(PlayerStatus.Catering);
    }
    
    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    } 

    private MicroWave GetHitObjComponentMicroWave() => GetHitObj((int)hitObjName.Microwave).GetComponent<MicroWave>();
    
    private Pot GetHitObjComponetPot() => GetHitObj((int)hitObjName.Pot).GetComponent<Pot>();

    private Grilled GetHitObjComponentGrilled() => GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>();

    public int GetPlayerID() => playerID;

    public  void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public GamePad.Index GetPlayerControllerNumber() => PlayerControllerNumber;


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
        // 電子レンジに当たっていなければ関数を抜ける
        if (GetHitObj((int)hitObjName.Microwave) == null) return;
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Microwave) return;
        if (GetHitObjComponentMicroWave().GetPlayerID() != playerID) return;
        playerMove_cs.MoveReset();
        playerMove_cs.VelocityReset();
        playerAnimation_cs.SetPlayerStatus(0);

        switch (GetHitObjComponentMicroWave().GetStatus())
        {
            case MicroWave.EMWState.SwitchOff:
                // 電子レンジを動かす
                cookingMicrowave_cs.PresstheMicrowaveStartButton();
                break;

            case MicroWave.EMWState.SwitchOn:
                // 電子レンジを停止させる
                GameObject cuisine = cookingMicrowave_cs.PresstheMicrowaveStopButton();
                if (cuisine == null)
                {
                    SetPlayerStatus(PlayerStatus.Microwave);
                }
                else
                {
                    WithaCuisine(cuisine);
                }

                break;

            default:
                break;
        }
    }

    public void ActionPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjName.Pot) == null) return;
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.Pot) return;
        playerMove_cs.MoveReset();
        playerMove_cs.VelocityReset();
        playerAnimation_cs.SetPlayerStatus(0);

        cookingPot_cs.CookingStart();
    }

    public void ActionGrilled()
    {
        if (GetPlayerStatus() != PlayerStatus.Normal && GetPlayerStatus() != PlayerStatus.GrilledTable) return;
        if (GetHitObj((int)hitObjName.GrilledTable) == null) return;
        if (GetHitObjComponentGrilled().GetStatus() == Grilled.GrilledState.inCcoking) return;
        playerMove_cs.MoveReset();
        playerMove_cs.VelocityReset();
        playerAnimation_cs.SetPlayerStatus(0);


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

    public string GetInputXAxisName() => inputXAxisName;

    public string GetInputYAxisName() => inputYAxisName;
}
