using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    const int playerNum = 4;                     // プレイヤーの数

    [SerializeField]
    private GameObject[] playerScore;       // 各プレイヤースコアの参照

    [SerializeField]
    private GameObject[] player;            　// 各プレイヤーの参照

    [SerializeField]
    private float timeCoefficient;                // 時間係数

    [SerializeField]
    private float[] rankCoefficient;             // 順位係数

    private int[] playerRank = new int[playerNum];     // プレイヤーの順位
    private Text[] scoreText = new Text[playerNum];   // スコア表示テキスト

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < playerNum; i++)
        {
            scoreText[i] = playerScore[i].GetComponent<Text>();
            playerRank[i] = 1;
            scoreText[i].text = "Score:"+ GetPlayerScore(i).ToString()+"/"+playerRank[i].ToString();
        }
    }
    
    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="score">加算ポイント</param>
    public void AddScore(int pID, int score)
    {
        // スコア加算
        int b = player[pID].gameObject.GetComponent<Player>().GetHindrancePoint();
        playerScore[pID].gameObject.GetComponent<ScoreCount>().AddScore(CalcAddScore(pID, score));
        int a = playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore();
 
        UpdatePlayerRank();     // 順位更新
        UpdateScoreText();       // 表示テキスト更新
    }

    /// <summary>
    /// スコア減算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="score">減算ポイント</param>
    public void SubScore(int pID, int score)
    {
        playerScore[pID].gameObject.GetComponent<ScoreCount>().SubScore(score);

        UpdatePlayerRank();     // 順位更新
        UpdateScoreText();      // 表示テキスト更新
    }

    /// <summary>
    /// 順位更新
    /// </summary>
    private void UpdatePlayerRank()
    {
        int[] workArray = new int[playerNum];
        InitArray(workArray);       // 作業用配列初期化
        BubbleSort(workArray);   // バブルソート
        SetRank(workArray);      // 順位セット
    }

    /// <summary>
    ///  作業用配列初期化
    /// </summary>
    /// <param name="workarray">作業用配列</param>
    private void InitArray(int[] workarray)
    {
        for (int i = 0; i < playerNum; i++)
            workarray[i] = GetPlayerScore(i);
    }

    /// <summary>
    /// バブルソート
    /// </summary>
    /// <param name="workarray">作業用配列</param>
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
    /// <param name="workarray">作業用配列</param>
    private void SetRank(int[] workarray)
    {
        for (int i = 0; i < playerNum; i++)
        {
            for (int j = 0; j < playerNum; j++)
            {
                if (GetPlayerScore(i) == workarray[j])
                {
                    playerRank[i] = j + 1;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 加算スコアの計算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="score">スコア</param>
    /// <returns>係数を掛けた計算後のスコア</returns>
    private int CalcAddScore(int pID, int score)
    {
        // 後で時間係数も計算に加える
        // なぜかPlayerから邪魔係数を取得しようとすると戻り値が０になる
        // とりあえず邪魔係数なしで計算
        //return  (int)(score * rankCoefficient[playerRank[pID] - 1] * player[pID].gameObject.GetComponent<Player>().GetHindrancePoint());
        return  (int)(score * rankCoefficient[playerRank[pID] - 1]);
    }

    /// <summary>
    ///  表示テキスト更新
    /// </summary>
    private void UpdateScoreText()
    {
        for (int i = 0; i < playerNum; i++)
            scoreText[i].text = "Score:" + GetPlayerScore(i).ToString() + "/" + playerRank[i].ToString();
    }

    /// <summary>
    /// スコア取得
    /// </summary>
    /// <param name="pID"></param>
    /// <returns>スコア</returns>
    private int GetPlayerScore(int pID)
    {
        return playerScore[pID].gameObject.GetComponent<ScoreCount>().GetScore();
    }
}
