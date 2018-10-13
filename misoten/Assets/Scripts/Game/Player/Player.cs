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
        Replenishment // 補充
    }

    public enum hitObjName
    {
        player1, //プレイヤー
        player2,
        player3,
        player4,
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
        TasteMachine,//旨味成分補充マシーン
        Alien,//宇宙人
        Taste//旨味成分
    }

	[SerializeField] [Range(0.0f, 30.0f)]
    private float speed;  // 移動スピード
    [SerializeField]
    private Vector2 move;  //移動量
    private string layerName;// レイヤーの名前
    [SerializeField]
    private PlayerStatus playerStatus;
 
    // 左スティックの入力を取る用
    private string InputXAxisName;
    private string InputYAxisName;
    private GamepadState padState;
    private GamePad.Index PlayerNumber;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject[] hitObj = new GameObject[9];
    private float hindrancePoint; // 邪魔point（現在未使用）

    [SerializeField]
    private GameObject haveInHandFood;  // 持っている食材

    private int playerID;

    private readonly static float HINDRANCE_TIME = 5;
    private float hindranceTime = 5; // 邪魔動作の時間
    private GameObject tastePrefab;//旨味成分

    [SerializeField]
    private int SetCountPlayer;
    [SerializeField]
    private PowderSetManager PowderSetScript;

    private CookingMicrowave cookingMicrowave_cs;
    private CookingPot cookingPot_cs;
    private CookingGrilled cookingGrilled_cs;
    

    // Use this for initialization
    void Awake()
    {
        PowderSetScript = GetComponent<PowderSetManager>();
        SetCount();
        rb = GetComponent<Rigidbody2D>();
        layerName = LayerMask.LayerToName(gameObject.layer);
        switch (layerName)
        {
            case "Player1":
                playerID = 0;
                break;
            case "Player2":
                playerID = 1;
                break;
            case "Player3":
                playerID = 2;
                break;
            case "Player4":
                playerID = 3;
                break;
        }
        playerStatus = PlayerStatus.Normal;
        hindrancePoint = 1;
        for (int i = 0; i < 9; i++)
        {
            hitObj[i] = null;
        }
        tastePrefab = (GameObject)Resources.Load("Prefabs/Taste");
        cookingMicrowave_cs = GetComponent<CookingMicrowave>();
        cookingPot_cs=GetComponent<CookingPot>();
        cookingGrilled_cs = GetComponent<CookingGrilled>();
    }

    private void FixedUpdate()
    {
        if (playerStatus == PlayerStatus.Pot) return; // 茹で料理を調理中ならばリターン

        if (playerStatus == PlayerStatus.Catering) move /= 2;// 配膳中ならば移動量半減

        rb.velocity = move * speed;
    }

    // Update is called once per frame
    void Update ()
    {
        // 今だけ
        move = new Vector2(0f, 0f);
        //

        if (!PlayerNumberDecision())    return;

        padState = GamepadInput.GamePad.GetState(PlayerNumber);

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
            move = new Vector2(Input.GetAxis(InputXAxisName) * 5, -(Input.GetAxis(InputYAxisName) * 5));


            InputKeyboard();  //　コントローラー４つでプレイするなら不要

            switch (playerStatus)
            {
                case PlayerStatus.Pot:
                    //茹で料理を調理中の処理
                    Vector2 stickVec = new Vector2(Input.GetAxis(InputXAxisName) * 5, -(Input.GetAxis(InputYAxisName) * 5));
                    GameObject cuisine = null;
                    cuisine = cookingPot_cs.Mix(stickVec);
                    if (cuisine != null) WithaCuisine(cuisine);
                    break;
                default:
                    InputButton();
                    break;
            }
        }
      
        Clamp();//移動範囲制限
    }

    /// <summary>
    /// プレイヤー番号を判定
    /// </summary>
    /// <returns>取得の成否</returns>
    private bool PlayerNumberDecision()
    {
        switch (layerName)
        {
            case "Player1":
                PlayerNumber = GamePad.Index.One;
                InputXAxisName = "L_XAxis_1";
                InputYAxisName = "L_YAxis_1";
                return true;
            case "Player2":
                PlayerNumber = GamePad.Index.Two;
                InputXAxisName = "L_XAxis_2";
                InputYAxisName = "L_YAxis_2";
                return true;
            case "Player3":
                PlayerNumber = GamePad.Index.Three;
                InputXAxisName = "L_XAxis_3";
                InputYAxisName = "L_YAxis_3";
                return true;
            case "Player4":
                PlayerNumber = GamePad.Index.Four;
                InputXAxisName = "L_XAxis_4";
                InputYAxisName = "L_YAxis_4";
                return true;
        }
        return false;
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            //プレイヤー
            case "Player1":
                hitObj[(int)hitObjName.player1] = collision.gameObject;
                break;

            case "Player2":
                hitObj[(int)hitObjName.player2] = collision.gameObject;
                break;

            case "Player3":
                hitObj[(int)hitObjName.player3] = collision.gameObject;
                break;

            case "Player4":
                hitObj[(int)hitObjName.player4] = collision.gameObject;
                break;
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
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player1":
                hitObj[(int)hitObjName.player1] = null;
                break;

            case "Player2":
                hitObj[(int)hitObjName.player2] = null;
                break;

            case "Player3":
                hitObj[(int)hitObjName.player3] = null;
                break;

            case "Player4":
                hitObj[(int)hitObjName.player4] = null;
                break;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        //ここに旨味成分に当たっているときの処理
        if (playerStatus != PlayerStatus.Catering) return;
        if (collision.tag != "Taste") return;
        if (collision.gameObject.GetComponent<Taste>().playerID == playerID) return;

        //配膳中かつ当たっているものが旨味成分かつ旨味成分が自分のものではないとき
        // 配膳中の料理の旨味を向上させる
        haveInHandFood.GetComponent<Food>().SubQualityTaste();        
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void InputButton()
    {
        // 補充マシーンとの当たり判定を取得
        if (GetHitObj((int)hitObjName.TasteMachine) != null)
        {
            // 当たっていればホールドで取得
            if (GamePad.GetButton(GamePad.Button.LeftShoulder, PlayerNumber))
            {
                //　旨味成分補充処理
                PowderSetScript.PowderSet();
            }
            else if (GamePad.GetButtonUp(GamePad.Button.LeftShoulder, PlayerNumber))
            {
                // Aボタンを離せば初期化
                PowderSetScript.InitSet();
            }
        }


        // Aボタン入力（電子レンジ）
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerNumber)) ActionMicrowave();
        // Xボタン入力（鍋）
        if (GamePad.GetButtonDown(GamePad.Button.X, PlayerNumber)) ActionPot();
        // Bボタン入力（フライパン）
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerNumber)) ActionGrilled();
        // Yボタン入力（邪魔）
        if (GamePad.GetButtonDown(GamePad.Button.Y, PlayerNumber))
        {
            //状態を邪魔に変更
            SetPlayerStatus(PlayerStatus.Hindrance);
            // 旨味成分を実体化
            SubSetCount();
            GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
            taste.GetComponent<Taste>().playerID = playerID;
        }

        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, PlayerNumber))
        {
            //　宇宙人の前にいるとき
            OfferCuisine();
        }
    }


    /// <summary>
    /// 移動範囲の制限
    /// </summary>
    private void Clamp()
    {
        float width = 7f;
        Vector2 min = new Vector2(-width, -4.5f);
        Vector2 max = new Vector2(width, 0.9f);

        Vector2 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    /// <summary>
    /// 料理を渡す
    /// </summary>
    private void OfferCuisine()
    {
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
                CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(haveInHandFood.GetComponent<Food>().GetFoodID());
                break;

            case 2:
                CuisineManager.GetInstance().GetPotController().OfferCuisine(haveInHandFood.GetComponent<Food>().GetFoodID());
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

    private void InputKeyboard()
    {
        // switch文は今だけ
        switch (layerName)
        {
            case "Player1":
                Debug.Log(Input.GetAxis(InputXAxisName));
                Debug.Log(Input.GetAxis(InputYAxisName));
                break;
            case "Player2":
                if (Input.GetKey(KeyCode.A)) move.x = -speed;
                if (Input.GetKey(KeyCode.D)) move.x = speed;
                if (Input.GetKey(KeyCode.W)) move.y = speed;
                if (Input.GetKey(KeyCode.S)) move.y = -speed;
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ActionMicrowave();
                    ActionPot();
                    ActionGrilled();
                    OfferCuisine();
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    SetPlayerStatus(PlayerStatus.Hindrance);
                    SubSetCount();
                    GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
                    taste.GetComponent<Taste>().playerID = playerID;
                }
                break;
            case "Player3":
                if (Input.GetKey(KeyCode.LeftArrow)) move.x = -speed;
                if (Input.GetKey(KeyCode.RightArrow)) move.x = speed;
                if (Input.GetKey(KeyCode.UpArrow)) move.y = speed;
                if (Input.GetKey(KeyCode.DownArrow)) move.y = -speed;
                if (Input.GetKeyDown(KeyCode.K))
                {
                    ActionMicrowave();
                    ActionPot();
                    ActionGrilled();
                    OfferCuisine();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    SetPlayerStatus(PlayerStatus.Hindrance);
                    SubSetCount();
                    GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
                    taste.GetComponent<Taste>().playerID = playerID;
                }
                break;
            case "Player4":
                break;
        }
        //
    }
    
    public GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    } 

    private MicroWave GetHitObjConmponentMicroWave() => GetHitObj((int)hitObjName.Microwave).GetComponent<MicroWave>();
    
    private Pot GetHitObjCommponetPot() => GetHitObj((int)hitObjName.Pot).GetComponent<Pot>();

    private Grilled GetHitObjCommponentGrilled() => GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>();

    public int GetPlayerID() => playerID;

    public  void SetPlayerStatus(PlayerStatus state) => playerStatus = state;
    private void SetCount() => SetCountPlayer = PowderSetScript.GetSetCount();
    //Powder1回分使う
    private void SubSetCount() => PowderSetScript.SubSetCount();

    public int GetSetCountPlayer() => SetCountPlayer;

    /// <summary>
    /// 電子レンジへのアクション
    /// </summary>
    private void ActionMicrowave()
    {
        // 電子レンジに当たっていなければ関数を抜ける
        if (GetHitObj((int)hitObjName.Microwave) == null) return;

        switch (GetHitObjConmponentMicroWave().GetStatus())
        {
            case MicroWave.MWState.switchOff:
                // 電子レンジを動かす
                cookingMicrowave_cs.PresstheMicrowaveStartButton();
                break;

            case MicroWave.MWState.switchOn:
                // 電子レンジを停止させる
                WithaCuisine(cookingMicrowave_cs.PresstheMicrowaveStopButton());
                break;

            default:
                break;
        }
    }

    private void ActionPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjName.Pot) == null) return;

        cookingPot_cs.CookingStart();
    }

    private void ActionGrilled()
    {
        if (GetHitObj((int)hitObjName.GrilledTable) == null) return;

        switch (GetHitObj((int)hitObjName.GrilledTable).GetComponent<Grilled>().GetStatus())
        {
            case Grilled.GrilledState.unused:
                cookingGrilled_cs.OnFire();
                break;

            case Grilled.GrilledState.inCcoking:
                cookingGrilled_cs.ShaketheFryingpan();
                break;
            default:
                break;
        }

    }
}
