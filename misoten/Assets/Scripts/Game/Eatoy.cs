using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eatoy : MonoBehaviour
{
    /// <summary>
    /// イートイカラー
    /// </summary>
    public enum EEatoyColor { None, Yellow, Orange, Red, Purple, Bule, Green, ColorMax }

    /// <summary>
    /// イートイカラー
    /// </summary>
    [SerializeField]
    private EEatoyColor eatoyColor;

    /// <summary>
    /// イートイポイント
    /// </summary>
    [SerializeField]
    private int eatoyPoint;

    /// <summary>
    /// 凍っている状態か
    /// </summary>
    [SerializeField]
    private bool isIcing;


    public void Init(int colorID, Sprite eatoySprite)
    {
        eatoyPoint = 0;
        isIcing = true;
        
        // enum設定
        eatoyColor = (EEatoyColor)Enum.ToObject(typeof(EEatoyColor), colorID);

        // スプライト設定
        GetComponent<SpriteRenderer>().sprite = eatoySprite;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
    }

    /// <summary>
    ///解凍
    /// </summary>
    public void Thawing()
    {
        isIcing = false;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public int AddPoint(int p) => eatoyPoint += p;

    public void SubPoint(int p) => eatoyPoint = Mathf.Max(0, eatoyPoint - p);

    public int GetEatoyPoint() => eatoyPoint;

    public EEatoyColor GetEatoyColor() => eatoyColor;

    public bool IsIcing() => isIcing;

    public bool IsChangeEatoy()
    {
        return 
            eatoyColor == EEatoyColor.Orange ||
            eatoyColor == EEatoyColor.Purple ||
            eatoyColor == EEatoyColor.Green;
    }

    public void HiddenSprite()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void DisplaySprite()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
