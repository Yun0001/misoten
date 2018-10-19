
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour
{
    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
    public enum EMWState { SwitchOff, Standby, SwitchOn, End, explostion }

    private GameObject microwaveCuisine;

    [SerializeField]
    private GameObject microwaveTimerImage;

    private MicrowaveTimerImage microwaveTimerImage_cs;
    /// <summary>
    /// セットタイマ-
    /// </summary>
    [SerializeField]
    private float explosionTime;

    /// <summary>
    /// タイマー
    /// </summary>
    private float timer;

    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    EMWState status;

    [SerializeField]
    private float[] successRangeMin;

    [SerializeField]
    private float[] successRangeMax;

    private int playerID;

    bool successFlg = false;

	// Use this for initialization
	private void Awake ()
    {
        microwaveTimerImage_cs = microwaveTimerImage.GetComponent<MicrowaveTimerImage>();
        timer = 0;
        status = EMWState.SwitchOff;
	}
	
	//更新処理
	void Update ()
    {
        switch (status)
        {
            //スイッチがオフ
            case EMWState.SwitchOff:
                //現状とくになし
                break;

            case EMWState.Standby:
                CountUpTimer();
                if (timer >= 1)
                {
                    status = EMWState.SwitchOn;
                    timer = 0;
                }
                break;

            //スイッチがオン
            case EMWState.SwitchOn:
                //タイマーカウントアップ
                CountUpTimer();
                break;

            case EMWState.End:
                CountUpTimer();
                break;

            case EMWState.explostion:
                CountUpTimer();
                break;
        }
	}

    /// <summary>
    ///  電子レンジスタート
    /// </summary>
    public bool StartCooking(int pID)
    {
        // 既にスイッチが入っていればリターン
        if (status != EMWState.SwitchOff) return false;
        microwaveCuisine = CuisineManager.GetInstance().GetMicrowaveController().OutputCuisine();
        
        if (microwaveCuisine == null) return false;
        // 待機状態
        status = EMWState.Standby;
        //タイマーセット
        timer = 0;

        microwaveTimerImage_cs.Init();

        playerID = pID;
        return true;
    }

    /// <summary>
    ///  電子レンジストップ
    /// </summary>
    public GameObject EndCooking()
    {
        //  小数点第２位以下切り捨て
        timer = Mathf.Floor(timer * 10) / 10;

        int playerRank = ScoreManager.GetInstance().GetPlayerRank(playerID);
        if (timer <= successRangeMax[playerRank - 1] && timer >= successRangeMin[playerRank - 1])
        {
            //成功
            // ここでtimeDifferenceを料理の変数にセットする
            microwaveCuisine.GetComponent<Food>().CalcTasteCoefficient(1);
            microwaveTimerImage_cs.ChangeSprite(MicrowaveTimerImage.EMicrowaveTimerTex.Success);
        }
        else
        {
            // 失敗
            // もう一度
            microwaveTimerImage_cs.ChangeSprite(MicrowaveTimerImage.EMicrowaveTimerTex.Failure);
            timer = 0;
            status = EMWState.End;
            return null;
        }

        // スイッチオフ
        status = EMWState.SwitchOff;

        // 電子レンジからプレイヤーに料理を渡す
        return microwaveCuisine;
    }

    /// <summary>
    /// タイマーカウントダウン
    /// </summary>
    private void CountUpTimer()
    {
        float OneScond = Mathf.Ceil(timer);
        timer += Time.deltaTime;


        if (status == EMWState.explostion)
        {
            // 爆発状態かつ3秒経過
            if (timer >= 3)
            {
                InterruptionCooking();
            }
        }
        else
        {
            if (OneScond <= 0) return;
            // タイマーが３以下かつ一秒経過毎
            if (timer >= OneScond && timer <= 3)
            {
                if (status == EMWState.End)
                {
                    RestartCooking();
                }
                else
                {
                    microwaveTimerImage_cs.ChangeSprite();
                }
            }
            if (timer >= explosionTime)
            {
                // 爆発処理
                microwaveTimerImage_cs.ChangeSprite(MicrowaveTimerImage.EMicrowaveTimerTex.Explosion);
                timer = 0;
                status = EMWState.explostion;
            }
        }
        
    }
    /// <summary>
    /// 状態取得
    /// </summary>
    /// <returns>状態</returns>
    public EMWState GetStatus() => status;

    private void RestartCooking()
    {
        // 待機状態
        status = EMWState.Standby;
        //タイマーセット
        timer = 0;

        microwaveTimerImage_cs.Init();
    }

    /// <summary>
    /// 中断
    /// </summary>
    public void InterruptionCooking()
    {
        timer = 0;
        status = EMWState.SwitchOff;
        CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(microwaveCuisine.GetComponent<Food>().GetFoodID());
        microwaveCuisine = null;
        microwaveTimerImage_cs.UnInit();
    }
}
