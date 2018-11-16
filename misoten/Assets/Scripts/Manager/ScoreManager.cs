using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    readonly static int playerNum = 1;                     // プレイヤーの数

    [SerializeField]
    private GameObject playerScore;       // 各プレイヤースコアの参照

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
            scoreText[pID] = playerScore.GetComponent<Text>();
            playerRank[pID] = 1;
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
        playerScore.GetComponent<ScoreCount>().AddScore(score);
    }

    /// <summary>
    /// スコア減算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="score">減算ポイント</param>
    public void SubScore(int pID, int score)
    {
        playerScore.GetComponent<ScoreCount>().SubScore(score);

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
    /// スコア取得
    /// </summary>
    /// <param name="pID"></param>
    /// <returns>スコア</returns>
    public int GetPlayerScore()
    {
        return ScoreCount.GetScore();
    }
}
