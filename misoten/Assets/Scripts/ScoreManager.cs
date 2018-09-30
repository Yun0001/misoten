using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    const int playerNum = 4;

    [SerializeField]
    private GameObject[] playerScore;

    private Text[] scoreText=new Text[playerNum];

    /// <summary>
    /// 邪魔係数
    /// </summary>
    [SerializeField]
    private float hindranceCoefficient ;

    /// <summary>
    /// 時間係数
    /// </summary>
    [SerializeField]
    private float timeCoefficient;

    /// <summary>
    /// 順位
    /// </summary>
    private int[] playerRank = new int[playerNum];

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < playerNum; i++)
        {
            scoreText[i] = playerScore[i].GetComponent<Text>();
            playerRank[i] = 1;
            scoreText[i].text = "Score:"+ playerScore[i].gameObject.GetComponent<ScoreCount>().GetScore().ToString()+"/"+playerRank[i].ToString();

        }
    }
    
    public void AddScore(int pID, int score)
    {
        playerScore[pID].gameObject.GetComponent<ScoreCount>().AddScore(CalcAddScore(pID, score));
        int oldRank = playerRank[pID];
        DecisionPlayerRank();
        if (oldRank != playerRank[pID])
        {
           
        }
        else
        {
            scoreText[pID].text = "Score:" + playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore().ToString() + "/" + playerRank[pID].ToString();
        }
        for (int i = 0; i < playerNum; i++)
        {
            scoreText[i].text = "Score:" + playerScore[i].gameObject.GetComponent<ScoreCount>().GetScore().ToString() + "/" + playerRank[i].ToString();
        }

    }

    public void SubScore(int pID, int score)
    {
        playerScore[pID].gameObject.GetComponent<ScoreCount>().SubScore(score);
        DecisionPlayerRank();
        scoreText[pID].text = "Score:" + playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore().ToString() + "/" + playerRank[pID].ToString();
    }

    /// <summary>
    /// 順位決定
    /// </summary>
    private void DecisionPlayerRank()
    {
        int[] workArray = new int[playerNum];
        InitArray(workArray);       // 作業用配列初期化
        BubbleSort(workArray);   // バブルソート
        SetRank(workArray);      // 順位セット
    }

    /// <summary>
    ///  作業用配列初期化
    /// </summary>
    /// <param name="workarray"></param>
    private void InitArray(int[] workarray)
    {
        for (int i = 0; i < playerNum; i++)
        {
            workarray[i] = playerScore[i].gameObject.GetComponent<ScoreCount>().GetScore();
        }
    }

    /// <summary>
    /// バブルソート
    /// </summary>
    /// <param name="workarray"></param>
    private void BubbleSort(int[] workarray)
    {
        for (int i = 0; i < playerNum - 1; i++)
        {
            for (int j = playerNum - 1; j > i; j--)
            {
                if (workarray[j - 1] < workarray[j])
                {
                    int work = workarray[j - 1];
                    workarray[j - 1] = workarray[j];
                    workarray[j] = work;
                }
            }
        }
    }

    /// <summary>
    ///  順位セット
    /// </summary>
    /// <param name="workarray"></param>
    private void SetRank(int[] workarray)
    {
        for (int i = 0; i < playerNum; i++)
        {
            for (int j = 0; j < playerNum; j++)
            {
                if (playerScore[i].gameObject.GetComponent<ScoreCount>().GetScore() == workarray[j])
                {
                    playerRank[i] = j + 1;
                    break;
                }
            }
        }
    }

    private int CalcAddScore(int pID, int score)
    {
        //　後で時間係数も計算に加える
        return score * playerRank[pID];
    }
}
