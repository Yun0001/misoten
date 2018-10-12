using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour {

    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
   public  enum MWState { switchOff,switchOn }

    private GameObject microwaveFood;

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
    /// 状態
    /// </summary>
    [SerializeField]
    MWState status;


	// Use this for initialization
	void Start () {
        timer = 0;
        status = MWState.switchOff;
	}
	
	//更新処理
	void Update ()
    {
        switch (status)
        {
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

        microwaveFood = FoodManager.GetInstance().GetMicrowaveController().OutputCuisine();

      
        return true;
    }

    /// <summary>
    ///  電子レンジストップ
    /// </summary>
    public GameObject EndCooking()
    {
        // 設定した時間との誤差を計算
        float timeDifference = Mathf.Abs(timer);
        timeDifference /= 10;


        // ここでtimeDifferenceを料理の変数にセットする
        microwaveFood.GetComponent<Food>().CalcTasteCoefficient(timeDifference);

        // スイッチオフ
        status = MWState.switchOff;

        // 電子レンジからプレイヤーに料理を渡す
        return microwaveFood;
    }

    /// <summary>
    /// タイマーカウントダウン
    /// </summary>
    private void CountDownTimer() => timer -= Time.deltaTime;

    /// <summary>
    /// 状態取得
    /// </summary>
    /// <returns>状態</returns>
    public MWState GetStatus() => status;
}
