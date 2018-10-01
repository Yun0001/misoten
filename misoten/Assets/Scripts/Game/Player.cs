using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour {

    enum hitObjnName
    {
        MicroWave
    }
    
    private float speed { get; set; }    // 移動スピード
    private float moveX = 0f;              //移動量
    private float moveZ = 0f;
    private string layerName;// レイヤーの名前
 
    // 左スティックの入力を取る用
    private string InputXAxisName;
    private string InputYAxisName;
    private GamepadState padState;
    private GamePad.Index PlayerNumber;
    private Rigidbody rb;
    private GameObject hitObj;
    private float hindrancePoint; // 邪魔point

    [SerializeField]
    private GameObject haveInHandFood;  // 持っている食材

    private int playerID;

    //あとで消す
    private int adjustmentSpeed = 10;

    // Use this for initialization
    void Start() {
        speed = 20f;
        rb = GetComponent<Rigidbody>();
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
        hindrancePoint = 1;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveX * speed, 0, moveZ * speed);
    }

    // Update is called once per frame
    void Update ()
    {
        // 今だけ
        moveX = 0;
        moveZ = 0;
        //

        if (!PlayerNumberDecision())    return;

        padState = GamepadInput.GamePad.GetState(PlayerNumber);
        moveX = Input.GetAxis(InputXAxisName);
        moveZ = -(Input.GetAxis(InputYAxisName));


        // switch文は今だけ
        switch (layerName)
        {
            case "Player1":

                break;
            case "Player2":
                if (Input.GetKey(KeyCode.A))     moveX = -speed/ adjustmentSpeed; 
                if (Input.GetKey(KeyCode.D))     moveX = speed / adjustmentSpeed;
                if (Input.GetKey(KeyCode.W))    moveZ = speed / adjustmentSpeed;
                if (Input.GetKey(KeyCode.S))     moveZ = -speed / adjustmentSpeed;
                if (Input.GetKeyDown(KeyCode.Z))     FrontoftheMicrowave();
                Debug.Log(hindrancePoint);
                break;
            case "Player3":
                if (Input.GetKey(KeyCode.LeftArrow))       moveX = -speed / adjustmentSpeed;
                if (Input.GetKey(KeyCode.RightArrow))     moveX = speed / adjustmentSpeed;
                if (Input.GetKey(KeyCode.UpArrow))         moveZ = speed / adjustmentSpeed;
                if (Input.GetKey(KeyCode.DownArrow))    moveZ = -speed / adjustmentSpeed;
                if (Input.GetKeyDown(KeyCode.L))                   FrontoftheMicrowave();
                break;
            case "Player4":

                break;
        }
        //

        InputButton();

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Microwave")
        {
            hitObj = other.gameObject;
        }
    }

    /// <summary>
    /// 当たり判定がなくなるとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Microwave")
        {
            hitObj = null;
        }
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void InputButton()
    {
        //　Aボタン入力
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerNumber))
        {
            //　レンジの前にいるとき
            FrontoftheMicrowave();
        }
    }

    /// <summary>
    /// レンジの前にいるとき
    /// </summary>
    private void FrontoftheMicrowave()
    {
        // ヒットオブジェクトがなければ関数を抜ける
        if (hitObj == null) return;

        if (GetHitObj().tag == "Microwave")
        {
            //　レンジの状態判定
            switch (GetHitObjConmponetMicroWave().GetStatus())
            {
                case MicroWave.MWState.objectNone:
                    // 食材を入れる
                    FoodInMicrowave();
                    break;

                case MicroWave.MWState.inObject:
                    // 必要ならここに
                    // レンジの食材が自分の食材か判定処理
                    if (GetHitObjConmponetMicroWave().GetInFoodID() != playerID)
                    {
                        // 自分以外の食材なら中の食材を出す
                        GetHitObjConmponetMicroWave().PutOutInFood(playerID);
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
                    if (GetHitObjConmponetMicroWave().GetInFoodID() != playerID)
                    {
                        GetHitObjConmponetMicroWave().PutOutInFood(playerID);
                    }
             
                    break;
            }
        }
    }

    /// <summary>
    /// 食材を入れる
    /// </summary>
    private void FoodInMicrowave()
    {
        // 何も持っていなかったらリターン
        if (GetHitObj() == null) return;

        //　食材が入っていなかったら自分が持っている食材をレンジに入れる
        GetHitObjConmponetMicroWave().SetFood(haveInHandFood);
        // 手に持っているオブジェクトをなくす
        haveInHandFood = null;
    }

    /// <summary>
    /// レンチンスタート
    /// </summary>
    private void MicrowaveSwitchOn()
    {
        GetHitObjConmponetMicroWave().cookingStart();
    }

    private GameObject GetHitObj()
    {
        return hitObj.gameObject;
    }

    private MicroWave GetHitObjConmponetMicroWave()
    {
        return GetHitObj().GetComponent<MicroWave>();
    }

    public void ResetHindrancePoint()
    {
        hindrancePoint = 1f;
    }

    public float GetHindrancePoint()
    {
         return hindrancePoint;
    }

    private void AddHindrancePoint()
    {
        hindrancePoint += 0.25f;
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}
