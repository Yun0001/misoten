using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasteGage : MonoBehaviour
{
    /*
    enum ETasteGageStatus
    {
        Start,
        Stay,
        Increase,
        Stop,
        End
    }

    public enum ESmokeLevel
    {
        Zero,
        First,
        Second,
        Full,
        Max
    }

    /// <summary>
    /// smokeLevelによる限界値
    /// </summary>
    private readonly static float[] LIMITATION = { 0.33f, 0.66f, 1.0f };

    // 限界値から引く値
    private readonly static float SUB_LIMITATION = 0.03f;

    private ETasteGageStatus tasteGageStatus;

    [SerializeField]
    private ESmokeLevel smokeLevel;

    [SerializeField][Range(0.0f,1.0f)]
    private float stayTime;

    [SerializeField][Range(0.0f,1.0f)]
    private float meterIncreaseSpeed;

    [SerializeField]
    private GameObject hindranceItem;

    private HindranceItem hindranceItem_cs;

    private Image meter;

    private void Awake()
    {
        meter = GetComponent<Image>();
        hindranceItem_cs = GetComponent<HindranceItem>();
    }

    public void SetHindranceItemCs(HindranceItem hindranceitem)
    {
        hindranceItem_cs = hindranceitem;
    }

    // Use this for initialization
    public void Init()
    {
        tasteGageStatus = ETasteGageStatus.Stay;
        smokeLevel = ESmokeLevel.Zero;
        meter.fillAmount = 0;
    }

    // Update is called once per frame
    public void UpdateGage ()
    {
        // 状態がStayの時は一定時間待つ
        if (tasteGageStatus == ETasteGageStatus.Stay)
        {
            stayTime -= Time.deltaTime;
            if (stayTime <= 0)
            {
                //一定時間経過でゲージ増加状態に
                tasteGageStatus = ETasteGageStatus.Increase;
            }
        }
        else if (tasteGageStatus == ETasteGageStatus.Increase)
        {
            //ゲージが最大でないなら
            if (smokeLevel != ESmokeLevel.Full)
            {
                MeterIncrease();                           // ゲージ増加
                DecisionRemainigTaste();               // SmokeLevelを残り回数以上にならないようにする
                LevelUpSmoke();                          // 煙レベルアップ
            }
        }
	}


    /// <summary>
    /// ゲージ増加
    /// </summary>
    public void MeterIncrease()
    {
        // ゲージ増加
        meter.fillAmount += meterIncreaseSpeed;
        if (meter.fillAmount >= 1.0f) meter.fillAmount = 1.0f;
    }

    /// <summary>
    /// 煙レベルアップ
    /// </summary>
    public void LevelUpSmoke()
    {
        // レベルアップ
        if (meter.fillAmount >= LIMITATION[(int)smokeLevel]) smokeLevel++;
    }

    /// <summary>
    /// SmokeLevelを残り回数以上にならないようにする
    /// </summary>
    private void DecisionRemainigTaste()
    {
        if (hindranceItem_cs.GetYummyBottle() == HindranceItem.MAXTASTE) return;
        // 使用回数を超えないように
        if (meter.fillAmount > LIMITATION[hindranceItem_cs.GetYummyBottle()] - SUB_LIMITATION)
        {
            meter.fillAmount = LIMITATION[(int)smokeLevel] - SUB_LIMITATION;
            tasteGageStatus = ETasteGageStatus.Stop;
        } 
    }

    public ESmokeLevel GetSmokeLevel() => smokeLevel;
    */
}
