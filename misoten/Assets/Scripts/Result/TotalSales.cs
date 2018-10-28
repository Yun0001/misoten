using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalSales : MonoBehaviour {

    private float countChip;
    private float timeCount;
    [SerializeField] private int aftertime =5;
    public int MaxScore =0; //total仮置き
    private GameObject totalChip;
    private GameObject hero;
    private ScoreManager scoreManager;
    private int[] ResultScore =new int[4];

    // Use this for initialization
    void Start () {
		
	}
	
    void Awake()
    {
        MaxScore = 0;
        timeCount = 0;
        countChip = 0;
        totalChip = GameObject.Find("totalchip");
        hero = GameObject.Find("ScoreManager");
        scoreManager = hero.GetComponent<ScoreManager>();
        TotalGetScore();
    }

	// Update is called once per frame
	void Update () {
        
        AfterTime();
        ChipExpress();
    }

    private void ChipExpress()
    {
        if (timeCount >= aftertime && countChip<=MaxScore)
        {
            totalChip.GetComponent<Text>().text = "Total :" + countChip.ToString();
            countChip++;
        }
    }
    
    private void AfterTime()
    {
        timeCount += Time.deltaTime;
    }

    private void TotalGetScore()
    {
        for (int pID = 0; pID < 4; pID++)
        {
            ResultScore[pID] = scoreManager.GetPlayerScore(pID);
            //Debug.Log(pID + " : " + ResultScore[pID]);
            //Debug.Log(pID + " : " + scoreManager.GetPlayerScore(pID));
            MaxScore += ResultScore[pID];
        }

    }

    public int GetMaxScore()
    {
        return MaxScore;
    }
}
