using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    readonly static int playerNum = 1;                     // プレイヤーの数

    [SerializeField]
    private GameObject[] playerScore;       // 各プレイヤースコアの参照

    [SerializeField]
    private GameObject[] player;             // 各プレイヤーの参照

    [SerializeField]
    private GameObject gameTimeManager; //タイムマネージャーの参照

    [SerializeField]
    private float timeCoefficient;                // 時間係数

    [SerializeField]
    private float[] rankCoefficient;             // 順位係数

    private int[] playerRank = new int[playerNum];     // プレイヤーの順位
    private Text[] scoreText = new Text[playerNum];   // スコア表示テキスト

    /// <summary>
    /// 初期処理
    /// </summary>
    void Start()
    {
        for (int pID = 0; pID < playerNum; pID++)
        {
            scoreText[pID] = playerScore[pID].GetComponent<Text>();
            playerRank[pID] = 1;
            scoreText[pID].text = "Chip:"+ GetPlayerScore(pID).ToString()+"/"+playerRank[pID].ToString();
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
        //playerScore[pID].GetComponent<ScoreCount>().AddScore(CalcAddScore(pID, score));
        playerScore[pID].GetComponent<ScoreCount>().AddScore(score);
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
        playerScore[pID].GetComponent<ScoreCount>().SubScore(score);

        UpdatePlayerRank();     // 順位更新
        UpdateScoreText();      // 表示テキスト更新
    }

    /// <summary>
    /// 順位更新
    /// </summary>
    private void UpdatePlayerRank()
    {
        var workArray = new int[playerNum]; //作業用配列

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
        for (int pID = 0; pID < playerNum; pID++)
            workarray[pID] = GetPlayerScore(pID);
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

    /*
    /// <summary>
    /// 加算スコアの計算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="score">スコア</param>
    /// <returns>係数を掛けた計算後のスコア</returns>
    private int CalcAddScore(int pID, int score)
    {
        
        // スコア＊順位係数＊邪魔係数
        // 残り時間が60秒未満ならさらに時間係数を掛ける
        return gameTimeManager.GetComponent<GameTimeManager>().GetCountTime() < 60 ?
                    (int)(score * rankCoefficient[playerRank[pID] - 1] * player[pID].GetComponent<Player>().GetHindrancePoint() * timeCoefficient) :
                    (int)(score * rankCoefficient[playerRank[pID] - 1] * player[pID].GetComponent<Player>().GetHindrancePoint());
    }
    */

    /// <summary>
    ///  表示テキスト更新
    /// </summary>
    private void UpdateScoreText()
    {
        for (int i = 0; i < playerNum; i++)
            scoreText[i].text = "Chip:" + GetPlayerScore(i).ToString();
       // scoreText[i].text = "Chip:" + GetPlayerScore(i).ToString() + "/" + playerRank[i].ToString();
    }

    /// <summary>
    /// スコア取得
    /// </summary>
    /// <param name="pID"></param>
    /// <returns>スコア</returns>
    public int GetPlayerScore(int pID) => playerScore[pID].GetComponent<ScoreCount>().GetScore();
    //public int GetPlayerScore(int pID) {
    //    return playerScore[pID].GetComponent<ScoreCount>().GetScore();
    //}

    public int GetPlayerRank(int ID) => playerRank[ID];
    //public int GetPlayerRank(int ID)
    //{
    //    return playerRank[ID];
    //}
}
