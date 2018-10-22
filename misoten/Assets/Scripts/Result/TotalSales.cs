using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalSales : MonoBehaviour {

    private float countChip;
    private float timeCount;
    [SerializeField] private int aftertime =5;
    private int MaxScore =200; //total仮置き
    private GameObject totalChip;

    // Use this for initialization
    void Start () {
		
	}
	
    void Awake()
    {
        timeCount = 0;
        countChip = 0;
        totalChip = GameObject.Find("totalchip");
    }

	// Update is called once per frame
	void Update () {
        AfterTime();
        ChipExpress();
    }

    private void ChipExpress()
    {
        if (timeCount >= aftertime && countChip<MaxScore)
        {
            countChip++;
            totalChip.GetComponent<Text>().text = "Total :" + countChip.ToString();

        }
    }
    
    private void AfterTime()
    {
        timeCount += Time.deltaTime;
    }

}
