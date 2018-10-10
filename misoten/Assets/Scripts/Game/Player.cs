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
                    if (GetHitObjCommponetPot().UpdateCooking(stickVec))
                    {
                        //調理終了の処理
                        WithaCuisine(GetHitObjCommponetPot().GetPotFood());
                    }
                    break;

                case PlayerStatus.GrilledTable:
                    switch (playerID)
                    {
                        case 0:
                            if (GamePad.GetButtonDown(GamePad.Button.A, PlayerNumber))
                            {
                                GetHitObjCommponentGrilled().CalcGrilledFoodTasteCoefficient();
                            }
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            if (GamePad.GetButtonDown(GamePad.Button.A, PlayerNumber))
                            {
                                GetHitObjCommponentGrilled().CalcGrilledFoodTasteCoefficient();
                            }
                            break;
                    }
               
                    if (GetHitObjCommponentGrilled().UpdateGrilled())
                    {
                        // 調理終了の処理
                        WithaCuisine(GetHitObjCommponentGrilled().GetGrilledFood());
                    }
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
            case "Alien":
                hitObj[(int)hitObjnName.Alien] = collision.gameObject;
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

            case "Alien":
                hitObj[(int)hitObjnName.Alien] = null;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //ここに旨味成分に当たっているときの処理
        if (collision.tag == "Taste")
        {
            if (collision.gameObject.GetComponent<Taste>().playerID != playerID)
            {
                // 配膳中の料理の旨味を向上させる
                haveInHandFood.GetComponent<Food>().SubQualityTaste();
            }
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
                CookingMicrowave();

                //  鍋の前にいるとき
                CookingPot();

                // 焼き台の前にいるとき
                CookingGrilled();
            }
        }


        // Bボタン入力
        // 他プレイヤーの邪魔（担当　贄田）
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerNumber))
        {
            //状態を邪魔に変更
            SetPlayerStatus(PlayerStatus.Hindrance);
            // 旨味成分を実体化
            GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
            taste.GetComponent<Taste>().playerID = playerID;
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
    private void CookingMicrowave()
    {
        // ヒットオブジェクトがなければ関数を抜ける

       if (GetHitObj((int)hitObjnName.Microwave) == null) return;

        //スイッチがOFF
        if (GetHitObjConmponentMicroWave().GetStatus() == MicroWave.MWState.switchOff)
        {
            // スイッチON
            PresstheMicrowaveStartButton();
            SetPlayerStatus(PlayerStatus.Microwave);
        }
        else
        {
            //スイッチをOFFにして料理をもつ
            WithaCuisine(PresstheMicrowaveStopButton());
        }
    }


    private void CookingPot()
    {
        // 鍋に当たっていなければ抜ける
        if (GetHitObj((int)hitObjnName.Pot) == null) return;

        // 鍋が未使用な自分が使用する
        if (GetHitObjCommponetPot().GetStatus() == Pot.PotState.unused)
        {
            GetHitObjCommponetPot().StartCookingPot();  //調理開始
            SetPlayerStatus(PlayerStatus.Pot);
        }
    }

    private void CookingGrilled()
    {
        if (GetHitObj((int)hitObjnName.GrilledTable) == null) return;

        //未使用ならば調理する
        if (GetHitObjCommponentGrilled().GetStatus() == Grilled.GrilledState.unused)
        {
            GetHitObjCommponentGrilled().StartCooking(); // 調理開始
            SetPlayerStatus(PlayerStatus.GrilledTable);
        }
    }

    /// <summary>
    /// レンジのスタート
    /// </summary>
    /// <returns></returns>
    private void PresstheMicrowaveStartButton()
    {
        GetHitObjConmponentMicroWave().StartCooking();
    }
    
    /// <summary>
    /// レンジのストップ
    /// </summary>
    /// <returns></returns>
    private GameObject PresstheMicrowaveStopButton()
    {
        return GetHitObjConmponentMicroWave().EndCooking();
    }
    
    /// <summary>
    /// 料理を持つ
    /// </summary>
    private void WithaCuisine(GameObject Food)
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
                    CookingMicrowave();
                    CookingPot();
                    CookingGrilled();
                    //GetHitObj((int)hitObjnName.Microwave)?.gameObject.GetComponent<Player>().CookingMicrowave();
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    SetPlayerStatus(PlayerStatus.Hindrance);
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
                    CookingMicrowave();
                    CookingPot();
                    CookingGrilled();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    SetPlayerStatus(PlayerStatus.Hindrance);
                    GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
                    taste.GetComponent<Taste>().playerID = playerID;
                }
                break;
            case "Player4":
                break;
        }
        //
    }
    
    private GameObject GetHitObj(int HitObjID)
    {
        if (hitObj[HitObjID] == null) return null;

        return hitObj[HitObjID].gameObject;
    } 

    private MicroWave GetHitObjConmponentMicroWave() => GetHitObj((int)hitObjnName.Microwave).GetComponent<MicroWave>();
    
    private Pot GetHitObjCommponetPot() => GetHitObj((int)hitObjnName.Pot).GetComponent<Pot>();

    private Grilled GetHitObjCommponentGrilled() => GetHitObj((int)hitObjnName.GrilledTable).GetComponent<Grilled>();

    public void ResetHindrancePoint() => hindrancePoint = 1f;

    public float GetHindrancePoint() => hindrancePoint;

    private void AddHindrancePoint() => hindrancePoint += 0.25f;

    public int GetPlayerID() => playerID;

    private void SetPlayerStatus(PlayerStatus state) => playerStatus = state;
}
