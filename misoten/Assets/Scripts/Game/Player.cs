using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour {

    enum hitObjnName
    {
        MicroWave
    }

	[Range(0.0f, 30.0f)]
	[SerializeField]
	private float speed;    // 移動スピード

	private float moveX = 0f;
    private float moveZ = 0f;
    private string layerName;
    private string InputXAxisName;
    private string InputYAxisName;
    private GamepadState padState;
    private GamePad.Index PlayerNumber;
    private Rigidbody rb;
    private GameObject hitObj;

    [SerializeField]
    private GameObject haveInHandFood;  // 持っている食材

    //あとで消す
    private int adjustmentSpeed = 10;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        layerName = LayerMask.LayerToName(gameObject.layer);
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
                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -speed/ adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveX = speed / adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    moveZ = speed / adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveZ = -speed / adjustmentSpeed;
                }
                break;
            case "Player3":
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    moveX = -speed / adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveX = speed / adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    moveZ = speed / adjustmentSpeed;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    moveZ = -speed / adjustmentSpeed;
                }
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
        if (GetHitObj().tag == "Microwave")
        {
            //　レンジの状態判定
            switch (GetHitObj().GetComponent<MicroWave>().GetStatus())
            {
                case MicroWave.MWState.objectNone:
                    // 食材を入れる
                    FoodInMicrowave();
                    break;

                case MicroWave.MWState.inObject:
                    // 必要ならここに
                    // レンジの食材が自分の食材か判定処理

                    // レンチンスタート
                    MicrowaveSwitchOn();
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
        GetHitObj().GetComponent<MicroWave>().SetFood(haveInHandFood);
        // 手に持っているオブジェクトをなくす
        haveInHandFood = null;
    }

    /// <summary>
    /// レンチンスタート
    /// </summary>
    private void MicrowaveSwitchOn()
    {
        GetHitObj().GetComponent<MicroWave>().cookingStart();
    }

    private GameObject GetHitObj()
    {
        return hitObj.gameObject;
    }

}
