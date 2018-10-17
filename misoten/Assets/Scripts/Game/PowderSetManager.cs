using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderSetManager : MonoBehaviour {

    //private bool Is;
    [SerializeField]
    private float SetPressure;
    private int minSet;
    [SerializeField]
    private int SetCount;
    private int maxSetCount;
    [SerializeField] private float maxSetPressureTime;
    [SerializeField] private Player playerScript;

    // Use this for initialization
    private void Start () {
        SetPressure = 0f;
        minSet = 2;
        maxSetPressureTime = 10f;
        maxSetCount =3;
        playerScript = GetComponent<Player>();
        //SetCount = playerScript.GetSetCountPlayer();
    }


    // Update is called once per frame
    private void Update () {
        //PowderSet();       
    }

    public void PowderSet()
    {
        
        //if (Input.GetKey(KeyCode.Y))
        if (SetCount < maxSetCount)
        {
            if (SetPressure < maxSetPressureTime)
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
            // Debug.Log(SetCount);
        }
 
    }

    //時間初期化
    public void InitSet()
    {
        SetPressure = 0f;
    }

    public int GetSetCount()
    {
        return SetCount;
    }
    //Powder1回分使う
    public void SubSetCount()
    {
        if (SetCount>0)
        {
            SetCount -= 1;
        }
    }
}
