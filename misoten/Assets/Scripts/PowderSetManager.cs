using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderSetManager : MonoBehaviour {

    //private bool Is;
    private float SetPressure;
    private int minSet;
    private int SetCount;
    private int maxSetCount;
    [SerializeField]
    private float maxSetPressureTime;

    // Use this for initialization
    private void Start () {
        SetPressure = 0f;
        minSet = 2;
        maxSetPressureTime = 10f;
        maxSetCount =3;
    }

    // Update is called once per frame
    private void Update () {
        PowderSet();
       
    }

    private void PowderSet()
    {
        //if(){}ToDo:テーブル判定フラグ
        if (Input.GetKey(KeyCode.Y))
        {
            if (SetPressure < maxSetPressureTime && SetCount<maxSetCount)
            {
                SetPressure += Time.deltaTime * 10f;
            }
            else
            {
                //SetPressure = maxSetPressureTime;
                InitSet();
                SetCount += 1;
                if (SetCount >= maxSetCount) SetCount = maxSetCount;
            }
            Debug.Log(SetCount);
        }
        else
        { 
            InitSet();
        }

        //ToDo:test
        if (Input.GetKey(KeyCode.H))
        {
            SetCount = 0;
        }
    }

    //時間初期化
    private void InitSet()
    {
        SetPressure = 0f;
    }

    public int GetSetCount()
    {
        return SetCount;
    }

}
