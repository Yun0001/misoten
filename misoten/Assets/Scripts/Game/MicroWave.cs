using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour {

    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
    enum MWState { objectNone,inObject, cooking }

    /// <summary>
    /// レンジの中にあるオブジェクト
    /// </summary>
    private GameObject food;

    /// <summary>
    /// セットタイマー
    /// </summary>
    [SerializeField]
    private float timer;

    /// <summary>
    /// 状態
    /// </summary>
    MWState status;


	// Use this for initialization
	void Start () {
        status = MWState.objectNone;
	}
	
	// Update is called once per frame
	void Update () {
        switch (status)
        {
            case MWState.objectNone:
                break;

            case MWState.inObject:
                break;

            case MWState.cooking:
                break;
        }
	}
}
