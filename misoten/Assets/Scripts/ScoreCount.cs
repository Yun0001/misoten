using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{


    /// <summary>
    /// プレイヤーのスコア
    /// </summary>
    private int score;

    void Awake() => score = 0;


    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="addscore">加算分スコア</param>
    public void AddScore(int addscore) => score += addscore;

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

    /// <summary>
    /// スコア取得’
    /// </summary>
    /// <returns>スコア</returns>
    public int GetScore() => score;
}