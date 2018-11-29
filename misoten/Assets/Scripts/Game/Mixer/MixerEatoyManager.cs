using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerEatoyManager : MonoBehaviour
{
    public enum MixMode
    {
        None,
        TwoParson,
        ThreeParson
    }

    [SerializeField]
    private GameObject eatoyPrefab;

    private Sprite[] eatoySprits;

    [SerializeField]
    private GameObject[] eatoies;

    private MixMode mixMode;

    private Mixer mixer_cs; 

    private void Awake()
    {
        mixer_cs = GetComponent<Mixer>();
        mixMode = MixMode.None;
        // イートイスプライトロード
        eatoySprits = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
    }

    public GameObject MixEatoy()
    {
        // イートイを破棄
        RevocationEatoies();

        // イートイ生成
        return InstanceEatoy();
    }

    /// <summary>
    /// イートイ生成
    /// </summary>
    /// <returns></returns>
    private GameObject InstanceEatoy()
    {
        GameObject putEatoy = Instantiate(eatoyPrefab, transform.position, Quaternion.identity);
        Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
        putEatoy.transform.localScale = scale;

        int eatoyID = 0;
        switch (mixMode)
        {
            case MixMode.TwoParson:
                eatoyID = DecisionTwoParonPutEatoyID();
                break;

            case MixMode.ThreeParson:
                eatoyID = DecisionThreeParsonPutEatoyID();
                break;

            default:
                Debug.LogError("不正な状態");
                break;
        }

        Eatoy putEatoy_cs = putEatoy.GetComponent<Eatoy>();
        putEatoy_cs.Init(eatoyID, eatoySprits[eatoyID - 1]);
        putEatoy_cs.Thawing();
        putEatoy_cs.AddPoint(CalcEatoyPoint());
        return putEatoy;
    }

    /// <summary>
    /// イートイを全て破棄
    /// </summary>
    private void RevocationEatoies()
    {
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] != null)
            {
                Destroy(eatoies[i]);
            }
        }
    }
    
    /// <summary>
    /// 二人でミキサー
    /// </summary>
    /// <returns></returns>
    private int DecisionTwoParonPutEatoyID()
    {
        // イートイの色を格納
        Eatoy.EEatoyColor[] eatoyColor = new Eatoy.EEatoyColor[2];
        for (int i = 0; i < 2; i++)
        {
            eatoyColor[i] = eatoies[i].GetComponent<Eatoy>().GetEatoyColor();
        }

        // 同じ色だった場合
        if (eatoyColor[0] == eatoyColor[1])
        {
            return (int)eatoyColor[0];
        }
        // 違う色
        else
        {
            // ２つのイートイの色によって返す色を決める
            switch ((int)eatoyColor[0] + (int)eatoyColor[1])
            {
                // オレンジ
                case 4:
                    return (int)Eatoy.EEatoyColor.Orange;
                // 緑
                case 6:
                    return (int)Eatoy.EEatoyColor.Green;
                // 紫
                case 8:
                    return (int)Eatoy.EEatoyColor.Purple;
            }
        }

        Debug.LogError("不正な値");
        return 0;
    }

    /// <summary>
    /// 三人でミキサー
    /// </summary>
    /// <returns></returns>
    private int DecisionThreeParsonPutEatoyID()
    {
        // イートイの色を格納
        Eatoy.EEatoyColor[] eatoyColor = new Eatoy.EEatoyColor[3];
        for (int i = 0; i < 3; i++)
        {
            eatoyColor[i] = eatoies[i].GetComponent<Eatoy>().GetEatoyColor();
        }

        // 全て同じ色だった場合
        if ((eatoyColor[0] == eatoyColor[1]) && (eatoyColor[0] == eatoyColor[2]))
        {
            return (int)eatoyColor[0]; // その色を返す
        }
        else
        {
            // 最後に入れたイートイの色を判定し、
            // その色を返す
            switch (eatoyColor[2])
            {
                case Eatoy.EEatoyColor.Yellow:
                    return (int)Eatoy.EEatoyColor.Orange;

                case Eatoy.EEatoyColor.Red:
                    return (int)Eatoy.EEatoyColor.Purple;

                case Eatoy.EEatoyColor.Bule:
                    return (int)Eatoy.EEatoyColor.Green;
            }
        }

        // ベースの色以外だった場合
        Debug.LogError("不正な値");
        return 0;
    }

    public int GetSumEatoyPoint()
    {
        int sum = 0;
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] != null)
            {
                sum += eatoies[i].GetComponent<Eatoy>().GetEatoyPoint();
            }
        }
        return sum;
    }

    private int CalcEatoyPoint() => mixer_cs.GetMiniGamePoint() * GetSumEatoyPoint();

    public void SetEatoy(GameObject playerHaveInEatoy)
    {
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] == null)
            {
                eatoies[i] = playerHaveInEatoy;
                break;
            }
        }
    }

    public void SetMixMode(MixMode mode) => mixMode = mode;

    public void HieenEatoySprite()
    {
        for (int i = 0; i < eatoies.Length; i++)
        {
            if (eatoies[i] != null)
            {
                eatoies[i].GetComponent<Eatoy>().HiddenSprite();
            }
        }
    }

    public Sprite[] GetEatoySprite() => eatoySprits;

    public GameObject[] GetEatoies() => eatoies;
}
