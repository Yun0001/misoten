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
    private float hindranceCoefficient;

    /// <summary>
    /// 時間係数
    /// </summary>
    [SerializeField]
    private float timeCoefficient;


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < playerNum; i++)
        {
            scoreText[i] = playerScore[i].GetComponent<Text>();
            scoreText[i].text = "Score:"+ playerScore[i].gameObject.GetComponent<ScoreCount>().GetScore().ToString();
        }
            
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void AddScore(int pID, int score)
    {
        playerScore[pID].gameObject.GetComponent<ScoreCount>().AddScore(score);
        scoreText[pID].text = "Score:" + playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore().ToString();
    }

    public void SubScore(int pID, int score)
    {
        playerScore[pID].gameObject.GetComponent<ScoreCount>().SubScore(score);
        scoreText[pID].text = "Score:" + playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore().ToString();
    }
}
