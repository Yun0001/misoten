using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour {

    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
   public  enum MWState { switchOff,switchOn }

    /// <summary>
    /// レンジの中にあるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject microwaveFoodPrefab;

    /// <summary>
    /// セットタイマ-
    /// </summary>
    [SerializeField]
    private   float setTimer;

    /// <summary>
    /// タイマー
    /// </summary>
    [SerializeField]
    private float timer;

    /// <summary>
    /// スコアマネージャー
    /// </summary>
    [SerializeField]
    private ScoreManager scoreManager;

    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    MWState status;


	// Use this for initialization
	void Start () {
        microwaveFoodPrefab = (GameObject)Resources.Load("Prefabs/MicrowaveFood");
        timer = 0;
        status = MWState.switchOff;
	}
	
	//更新処理
	void Update () {
       // Debug.Log(status);
        switch (status)
        {
            /*
            // 食材なし
            case MWState.objectNone:
                if (Input.GetKeyDown(KeyCode.R)) status = MWState.inObject;
                break;

            // 食材あり
            case MWState.inObject:
                if (Input.GetKeyDown(KeyCode.T))
                {
                    cookingStart();
                    status = MWState.cooking;
                } 
                break;

            // 調理中
            case MWState.cooking:
                CountDownTimer();
                if (0 >= timer)
                {
                    // スコア加算
                    scoreManager.AddScore(food.gameObject.GetComponent<Food>().GetOwnershipPlayerID(), (int)setTimer);

                    // 電子レンジ初期化
                    InitMicrowave();
                }
                break;
                */

            //スイッチがオフ
            case MWState.switchOff:
                //現状とくになし
                break;

                //スイッチがオン
            case MWState.switchOn:
                //タイマーカウントダウン
                CountDownTimer();
                break;
        }
	}

    /*
    /// <summary>
    /// レンジ開始
    /// </summary>
    public void cookingStart()
    {
        timer = setTimer;
        status = MWState.cooking;
    }
    */

        /*
    /// <summary>
    /// 電子レンジに食材をセット
    /// </summary>
    /// <param name="obj">セットする食材</param>
    /// <returns></returns>
    public bool SetFood(GameObject obj)
    {
        if (food != null) return false;

        food = obj;
        status = MWState.inObject;
        return true;
    }
    */

    /// <summary>
    /// 中にある食材を無くす
    /// </summary>
    /// <returns></returns>
    public bool PutOutInFood(int playerID)
    {
        if (GetInFoodID() == playerID) return false;

        InitMicrowave();
        return true;
    }

    /// <summary>
    /// 状態取得
    /// </summary>
    /// <returns>状態</returns>
    public MWState GetStatus()=> status;

    /// <summary>
    /// 中の食材のプレイヤーIDを取得
    /// </summary>
    /// <returns>食材のプレイヤーID</returns>
    public int GetInFoodID() => microwaveFoodPrefab.gameObject.GetComponent<Food>().GetOwnershipPlayerID();

    /// <summary>
    /// 電子レンジ初期化
    /// </summary>
    private void InitMicrowave()
    {
        microwaveFoodPrefab = null;
        //status = MWState.objectNone;
        status = MWState.switchOn;
        timer = 0f;
    }

    /// <summary>
    /// タイマーカウントダウン
    /// </summary>
    private void CountDownTimer()
    {
        timer -= Time.deltaTime;
    }

    //電子レンジストップ
    public GameObject EndCooking()
    {
        // 設定した時間との誤差を計算
        float timeDifference = Mathf.Abs(timer);

        // ここでtimeDifferenceを料理の変数にセットする
        microwaveFoodPrefab.GetComponent<Food>().CalcTasteCoefficient(timeDifference);

        // スイッチオフ
        status = MWState.switchOff;

        // 電子レンジからプレイヤーに料理を渡す
        return microwaveFoodPrefab;
    }
    /// <summary>
    ///  電子レンジスタート
    /// </summary>
    public bool StartCooking()
    {
        // 既にスイッチが入っていればリターン
        if (status == MWState.switchOn) return false;

        // スイッチオン
        status = MWState.switchOn;
        //タイマーセット
        timer = setTimer;
        return true;
    }

}
