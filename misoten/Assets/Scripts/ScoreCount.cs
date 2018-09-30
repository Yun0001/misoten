using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{


    /// <summary>
    /// 各プレイヤーのスコア
    /// </summary>
    private int score;

    void Start()
    {
        score = 0;
    }


    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="addscore">加算分スコア</param>
    public void AddScore(int addscore)
    {
        score += addscore;
    }

    /// <summary>
    /// スコア減算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="subscore">減算分スコア</param>
    public void SubScore(int subscore)
    {
        score -= subscore;

        if (subscore < 0)   subscore = 0;
    }

    public int GetScore()
    {
        return score;
    }
}

