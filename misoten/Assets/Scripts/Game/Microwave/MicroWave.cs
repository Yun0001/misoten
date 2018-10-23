
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 電子レンジ
public class MicroWave : MonoBehaviour
{
    /// <summary>
    /// 電子レンジの状態　列挙
    /// </summary>
    public enum EMWState { SwitchOff, Standby, SwitchOn, Explostion, Failure, Success }

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
    [SerializeField]
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

    private Vector3 playerPos;

    // Use this for initialization
    private void Awake()
    {
        microwaveTimerImage_cs = microwaveTimerImage.GetComponent<MicrowaveTimerImage>();
        ResetTimer();
        SetStatus(EMWState.SwitchOff);
    }

    //更新処理
    void Update()
    {
        // スイッチオフならリターン
        if (status == EMWState.SwitchOff) return;

        float OneScond = Mathf.Ceil(timer);

        // タイマーカウントアップ
        CountUpTimer();

        //ステータスにより分岐
        BranchStatus(OneScond);
    }

    /// <summary>
    ///  電子レンジスタート
    /// </summary>
    public bool StartCooking(int pID, Vector3 pPos)
    {
        // 既にスイッチが入っていればリターン
        if (status != EMWState.SwitchOff) return false;

        microwaveCuisine = CuisineManager.GetInstance().GetMicrowaveController().OutputCuisine();
        if (microwaveCuisine == null) return false;

        // 待機状態
        SetStatus(EMWState.Standby);

        //タイマーセット
        ResetTimer();

        // タイマー表示
        playerPos = pPos;
        microwaveTimerImage_cs.Display(playerPos);

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

        // 順位に応じた範囲で判定
        if (IsInSuccessRange())
        {
            //成功
            ProcessSucccess();
        }
        else
        {
            // 失敗
            ProcessFailure();
            return null;
        }

        // スイッチオフ
        SetStatus(EMWState.Success);

        // 電子レンジからプレイヤーに料理を渡す
        return microwaveCuisine;
    }

    /// <summary>
    /// タイマーカウントアップ
    /// </summary>
    private void CountUpTimer()
    {
        timer += Time.deltaTime;

        // 爆発判定　　　　　　　　爆発
        if (IsOverExplosionTime()) ProcessExplosion();
    }

    /// <summary>
    /// 状態取得
    /// </summary>
    /// <returns>状態</returns>
    public EMWState GetStatus() => status;

    private void RestartCooking()
    {
        // 待機状態
        SetStatus(EMWState.Standby);
        //タイマーセット
        ResetTimer();

        microwaveTimerImage_cs.Display(playerPos);
    }

    /// <summary>
    /// 中断
    /// </summary>
    public void InterruptionCooking()
    {
        // タイマーゼロ
        ResetTimer();

        // スイッチオフ
        SetStatus(EMWState.SwitchOff);

        // 中の料理をnull
        CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(microwaveCuisine.GetComponent<Food>().GetFoodID());
        microwaveCuisine = null;

        // タイマ非表示
        microwaveTimerImage_cs.HiddenTimer();
    }

    /// <summary>
    /// 成功処理
    /// </summary>
    private void ProcessSucccess()
    {
        // ここでtimeDifferenceを料理の変数にセットする
        microwaveCuisine.GetComponent<Food>().CalcTasteCoefficient(1);
        microwaveTimerImage_cs.SetSprite(MicrowaveTimerImage.EMicrowaveTimerSprite.Success);
        ResetTimer();
    }

    /// <summary>
    /// 失敗処理
    /// </summary>
    private void ProcessFailure()
    {
        microwaveTimerImage_cs.SetSprite(MicrowaveTimerImage.EMicrowaveTimerSprite.Failure);
        // もう一度
        ResetTimer();
        SetStatus(EMWState.Failure);
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    private void ProcessExplosion()
    {
        microwaveTimerImage_cs.SetSprite(MicrowaveTimerImage.EMicrowaveTimerSprite.Explosion);
        ResetTimer();
        SetStatus(EMWState.Explostion);
    }

    /// <summary>
    /// 爆発したか
    /// </summary>
    /// <returns></returns>
    private bool IsOverExplosionTime() => timer >= explosionTime;

    /// <summary>
    /// 一秒経過したか
    /// </summary>
    /// <param name="onesecond"></param>
    /// <returns></returns>
    private bool IsElapsedOneSecond(float onesecond) => timer >= onesecond && (timer <= 4 && onesecond > 0);

    /// <summary>
    /// 順位に応じた範囲で判定
    /// </summary>
    /// <returns>成否</returns>
    private bool IsInSuccessRange()
    {
        int playerRank = ScoreManager.GetInstance().GetPlayerRank(playerID);
        return timer <= successRangeMax[playerRank - 1] && timer >= successRangeMin[playerRank - 1];
    }

    private void SetStatus(EMWState state) => status = state;

    private void ResetTimer() => timer = 0;

    private void BranchStatus(float onesecond)
    {
        switch (status)
        {
            case EMWState.Standby:
                if (timer >= 1)
                {
                    SetStatus(EMWState.SwitchOn);
                    microwaveTimerImage_cs.SetNextSprite();
                    ResetTimer();
                }
                break;

            //スイッチがオン
            case EMWState.SwitchOn:
                if (IsElapsedOneSecond(onesecond)) microwaveTimerImage_cs.SetNextSprite();
                break;

            case EMWState.Failure:
                if (IsElapsedOneSecond(onesecond)) RestartCooking();
                break;

            case EMWState.Explostion:
                // 爆発状態かつ3秒経過
                if (timer >= 3) InterruptionCooking();
                break;

            case EMWState.Success:
                // 成功状態かつ0.5秒経過
                if (timer >= 0.5f)
                {
                    SetStatus(EMWState.SwitchOff);
                    microwaveTimerImage_cs.HiddenTimer();
                }
                break;
        }
    }
}
