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

    private float timer;


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
	
	// Update is called once per frame
	void Update () {
        Debug.Log(status);
        switch (status)
        {
            case MWState.objectNone:
                if (Input.GetKeyDown(KeyCode.R)) status = MWState.inObject;
                break;

            case MWState.inObject:
                if (Input.GetKeyDown(KeyCode.T))
                {
                    cookingStart();
                    status = MWState.cooking;
                } 
                break;

            case MWState.cooking:
                timer -= Time.deltaTime;
                Debug.Log(timer);
                if (0 >= timer)
                {
                    status = MWState.objectNone;
                    food = null;
                    timer = 0f;
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

    public MWState GetStatus()
    {
        return status;
    }
}
