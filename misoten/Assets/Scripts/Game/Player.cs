using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour
{

    enum PlayerStatus
    {
        Normal,             //通常
        Microwave,        //電子レンジ
        Pot,                   //鍋
        GrilledTable,       //焼き台
        Catering,           //配膳
        Hindrance,          //邪魔
        Replenishment // 補充
    }

    enum hitObjnName
    {
        player1, //プレイヤー
        player2,
        player3,
        player4,
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
        TasteMachine//旨味成分補充マシーン
    }

	[SerializeField] [Range(0.0f, 30.0f)]
    private float speed;  // 移動スピード
    private float moveX = 0f;              //移動量
    private float moveY = 0f;
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
    private float hindrancePoint; // 邪魔point

    [SerializeField]
    private GameObject haveInHandFood;  // 持っている食材

    private int playerID;

    //あとで消す
    private int adjustmentSpeed = 10;

    private readonly static float HINDRANCE_TIME = 5;
    private float hindranceTime = 5; // 邪魔動作の時間
    private GameObject tastePrefab;//旨味成分


    // Use this for initialization
    void Awake()
    {
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
    }

    private void FixedUpdate()
    {
        if (playerStatus == PlayerStatus.Catering)
        {
            moveX /= 2;
            moveY /= 2;
        }
        rb.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.Log(playerStatus);
        // 今だけ
        moveX = 0;
        moveY = 0;
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
                playerStatus = PlayerStatus.Normal;
            }
        }
        else
        {
            //　邪魔状態以外の時の処理
            moveX = Input.GetAxis(InputXAxisName) * 5;
            moveY = -(Input.GetAxis(InputYAxisName) * 5);


            // switch文は今だけ
            switch (layerName)
            {
                case "Player1":

                    break;
                case "Player2":
                    if (Input.GetKey(KeyCode.A)) moveX = -speed;
                    if (Input.GetKey(KeyCode.D)) moveX = speed ;
                    if (Input.GetKey(KeyCode.W)) moveY = speed;
                    if (Input.GetKey(KeyCode.S)) moveY = -speed;
                    if (Input.GetKeyDown(KeyCode.Z)) FrontoftheMicrowave();
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        playerStatus = PlayerStatus.Hindrance;
                        Instantiate(tastePrefab, transform.position, Quaternion.identity);
                    }
                    break;
                case "Player3":
                    if (Input.GetKey(KeyCode.LeftArrow)) moveX = -speed;
                    if (Input.GetKey(KeyCode.RightArrow)) moveX = speed;
                    if (Input.GetKey(KeyCode.UpArrow)) moveY = speed;
                    if (Input.GetKey(KeyCode.DownArrow)) moveY = -speed;
                    if (Input.GetKeyDown(KeyCode.K)) FrontoftheMicrowave();
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        playerStatus = PlayerStatus.Hindrance;
                        Instantiate(tastePrefab, transform.position, Quaternion.identity);
                    }
                    break;
                case "Player4":

                    break;
            }
            //

            InputButton();
        }
      
        Clamp();//移動範囲制限

        //Debug.Log(hitObj);
        //Debug.Log(haveInHandFood);
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
                hitObj[(int)hitObjnName.player1] = collision.gameObject;
                break;

            case "Player2":
                hitObj[(int)hitObjnName.player2] = collision.gameObject;
                break;

            case "Player3":
                hitObj[(int)hitObjnName.player3] = collision.gameObject;
                break;

            case "Player4":
                hitObj[(int)hitObjnName.player4] = collision.gameObject;
                break;
            // レンジ
            case "Microwave":
                hitObj[(int)hitObjnName.Microwave] = collision.gameObject;
                break;
            // 鍋
            case "Pot":
                hitObj[(int)hitObjnName.Pot] = collision.gameObject;
                break;
            // 焼き台
            case "GrilledTable":
                hitObj[(int)hitObjnName.GrilledTable] = collision.gameObject;
                break;
            // 補充マシーン
            case "TasteMachine":
                hitObj[(int)hitObjnName.TasteMachine] = collision.gameObject;
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
                hitObj[(int)hitObjnName.player1] = null;
                break;

            case "Player2":
                hitObj[(int)hitObjnName.player2] = null;
                break;

            case "Player3":
                hitObj[(int)hitObjnName.player3] = null;
                break;

            case "Player4":
                hitObj[(int)hitObjnName.player4] = null;
                break;
            case "Microwave":
                hitObj[(int)hitObjnName.Microwave] = null;
                break;
            case "Pot":
                hitObj[(int)hitObjnName.Pot] = null;
                break;
            case "GrilledTable":
                hitObj[(int)hitObjnName.GrilledTable] = null;
                break;

            case "TasteMachine":
                hitObj[(int)hitObjnName.TasteMachine] = null;
                break;
        }
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void InputButton()
    {
        //　Aボタン入力
        // 調理器具があれば調理モード
        // 補充マシーンがあれば補充

        // 補充マシーンとの当たり判定を取得
        if (GetHitObj((int)hitObjnName.TasteMachine) != null)
        {
            // 当たっていればホールドで取得
            if (GamePad.GetButton(GamePad.Button.A, PlayerNumber))
            {
                //　旨味成分補充処理（担当　贄田）
            }
        }
        else
        {
            // あたっていなかったらトリガーで取得
            if (GamePad.GetButtonDown(GamePad.Button.A, PlayerNumber))
            {
                //　レンジの前にいるとき
                FrontoftheMicrowave();

                //  鍋の前にいるとき

                // 焼き台の前にいるとき
            }
        }


        // Bボタン入力
        // 他プレイヤーの邪魔（担当　贄田）
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerNumber))
        {
            //状態を邪魔に変更
            playerStatus = PlayerStatus.Hindrance;
            // 旨味成分を実体化
            Instantiate(tastePrefab, transform.position, Quaternion.identity);

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
    /// レンジの前にいるとき
    /// </summary>
    private void FrontoftheMicrowave()
    {
        // ヒットオブジェクトがなければ関数を抜ける
        if (GetHitObj((int)hitObjnName.Microwave) == null) return;

        //スイッチがOFF
        if (GetHitObjConmponetMicroWave().GetStatus() == MicroWave.MWState.switchOff)
        {
            // スイッチON
            PresstheMicrowaveStartButton();
        }
        else
        {
            //スイッチをOFFにして料理をもつ
            WithaCuisine(PresstheMicrowaveStopButton());
        }

        /*
        if (GetHitObj((int)hitObjnName.Microwave).tag == "Microwave")
        {
   
            
            //　レンジの状態判定
            switch (GetHitObjConmponetMicroWave().GetStatus())
            {
                case MicroWave.MWState.objectNone:
                    // 食材を入れる
                    FoodInMicrowave();
                    break;

                case MicroWave.MWState.inObject:
                    // レンジの食材が自分の食材か判定処理
                    // 自分以外の食材なら中の食材を出す
                    if (GetHitObjConmponetMicroWave().PutOutInFood(playerID))
                    {
                        // 邪魔ポイント加算
                        AddHindrancePoint();
                    }
                    else
                    {
                        // 自分の食材ならレンチンスタート
                        MicrowaveSwitchOn();
                    }
                    break;

                case MicroWave.MWState.cooking:
                        GetHitObjConmponetMicroWave().PutOutInFood(playerID);
                    break;
            }      
        }
        */
    }

    /// <summary>
    /// 食材を入れる
    /// </summary>
    private void FoodInMicrowave()
    {
        // 何も持っていなかったらリターン
        //if (GetHitObj() == null) return;

        //　食材が入っていなかったら自分が持っている食材をレンジに入れる
        //GetHitObjConmponetMicroWave().SetFood(haveInHandFood);
        // 手に持っているオブジェクトをなくす
        haveInHandFood = null;
    }

    /// <summary>
    /// レンチンスタート
    /// </summary>
    //private void MicrowaveSwitchOn() => GetHitObjConmponetMicroWave().cookingStart();


    private void PresstheMicrowaveStartButton()
    {
        GetHitObjConmponetMicroWave().StartCooking();
    }

    private GameObject PresstheMicrowaveStopButton()
    {
        return GetHitObjConmponetMicroWave().EndCooking();
    }


    /// <summary>
    /// 料理を持つ
    /// </summary>
    private void WithaCuisine(GameObject Food)
    {
        haveInHandFood = Food;
        playerStatus = PlayerStatus.Catering;
    }

    private GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

            return hitObj[HitObjID].gameObject;
    } 

    private MicroWave GetHitObjConmponetMicroWave() => GetHitObj((int)hitObjnName.Microwave).GetComponent<MicroWave>();

    public void ResetHindrancePoint() => hindrancePoint = 1f;

    public float GetHindrancePoint() => hindrancePoint;

    private void AddHindrancePoint() => hindrancePoint += 0.25f;

    public int GetPlayerID() => playerID;
}
