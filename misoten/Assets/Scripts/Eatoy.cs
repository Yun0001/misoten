using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Eatoy : MonoBehaviour {

    public enum EEatoyColor { None, Yellow, Orange, Red, Purple, Bule, Green, ColorMax }
    [SerializeField]
    private EEatoyColor eatoyColor;

    [SerializeField]
    private int eatoyPoint;

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
    }


    public void Thawing() => isIcing = false;

    public void AddPoint(int p) => eatoyPoint += p;

    public void SubPoint(int p) => eatoyPoint = Mathf.Max(0, eatoyPoint - p);

    public int GetEatoyPoint() => eatoyPoint;

    public EEatoyColor GetEatoyColor() => eatoyColor;

    public bool IsIcing() => isIcing;
}
