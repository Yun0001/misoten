using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eatoy : MonoBehaviour {

    public enum EEatoyColor { None, Red, Bule, Yellow, Purple, Green, Orange, ColorMax }

    [SerializeField]
    private EEatoyColor eatoyColor;

    [SerializeField]
    private float eatoyPoint;

    public void Init(int colorID, Sprite eatoySprite)
    {
        eatoyPoint = 0;

        // enum設定
        // 何かスマートな方法ないかなー
        // .NET4.7.1ならなー
        eatoyColor = ConversionID(colorID);

        // スプライト設定
        GetComponent<SpriteRenderer>().sprite = eatoySprite;
    }

    private EEatoyColor ConversionID(int colorID)
    {
        switch (colorID)
        {
            case 1:     return EEatoyColor.Red;
            case 2:     return EEatoyColor.Bule;
            case 3:     return EEatoyColor.Yellow;
            case 4:     return EEatoyColor.Purple;
            case 5:     return EEatoyColor.Green;
            case 6:     return EEatoyColor.Orange;
            default:
                Debug.LogError("不正なColorID");
                return EEatoyColor.None;
        }
    }

    public void AddPoint(int p) => eatoyPoint += p;

    public void SubPoint(int p) => eatoyPoint = Mathf.Max(0, eatoyPoint - p);

    public EEatoyColor GetEatoyColor() => eatoyColor;
}
