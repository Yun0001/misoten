using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour {

    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
   public  enum MWState { objectNone,inObject, cooking }

    /// <summary>
    /// レンジの中にあるオブジェクト
    /// </summary>
    private GameObject food;

    /// <summary>
    /// セットタイマー
    /// </summary>
    [SerializeField]
    private float setTimer;

    /// <summary>
    /// タイマー
    /// </summary>
    private float timer;

    /// <summary>
    /// スコア
    /// </summary>
    [SerializeField]
    private ScoreCount scoreCount;

    /// <summary>
    /// 状態
    /// </summary>
    MWState status;


	// Use this for initialization
	void Start () {
        food = null;
        timer = 0;
        status = MWState.objectNone;
	}
	
	//更新処理
	void Update () {
        Debug.Log(status);
        switch (status)
        {
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
                    scoreCount.AddScore(food.gameObject.GetComponent<Food>().GetOwnershipPlayerID(), (int)setTimer);

                    // 電子レンジ初期化
                    InitMicrowave();
                }
                break;
        }
	}

    /// <summary>
    /// レンジ開始
    /// </summary>
    public void cookingStart()
    {
        timer = setTimer;
        status = MWState.cooking;
    }

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

    /// <summary>
    /// 中ｍにある食材を無くす
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
    public MWState GetStatus()
    {
        return status;
    }

    /// <summary>
    /// 中の食材のプレイヤーIDを取得
    /// </summary>
    /// <returns>食材のプレイヤーID</returns>
    public int GetInFoodID()
    {
        return food.gameObject.GetComponent<Food>().GetOwnershipPlayerID();
    }

    /// <summary>
    /// 電子レンジ初期化
    /// </summary>
    private void InitMicrowave()
    {
        food = null;
        status = MWState.objectNone;
        timer = 0f;
    }

    /// <summary>
    /// タイマーカウントダウン
    /// </summary>
    private void CountDownTimer()
    {
        timer -= Time.deltaTime;
        Debug.Log(timer);
    }
}
