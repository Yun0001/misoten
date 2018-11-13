using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class ScoreCount : MonoBehaviour
{


    /// <summary>
    /// プレイヤーのスコア
    /// </summary>
    private static int score;

    private Sprite[] sprits;

    [SerializeField]
    private GameObject[] spriteObj;

    void Awake()
    {
        score = 0;
        sprits = Resources.LoadAll<Sprite>("Textures/UI_Digital2");
        for (int i = 0; i < spriteObj.Length; i++)
        {
            spriteObj[i].GetComponent<SpriteRenderer>().sprite = sprits[0];
        }
        
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <param name="addscore">加算分スコア</param>
    public void AddScore(int addscore)
    {
        score += addscore;
        UpdateSprite();
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

    /// <summary>
    /// スコア取得’
    /// </summary>
    /// <returns>スコア</returns>
    public int GetScore() => score;

    private void UpdateSprite()
    {
        int suu = score;
        int rement;
        for (int i = 0; i < 4; i++)
        {
            rement = suu % 10;
            spriteObj[i].GetComponent<SpriteRenderer>().sprite = sprits[rement];
            suu = suu / 10;
        }
    }
}