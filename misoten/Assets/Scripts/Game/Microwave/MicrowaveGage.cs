using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveGage : MonoBehaviour
{
    private enum MicrowaveState
    {
        Access,
        Wait,
        Roll,
        Start,
        Success,
        End,
    }
    private int timer = 600;
    private float timerMin = 0;
    private float timerMax = 20;
    private float timerCount = 0;
    private float TIMER_COUNT;
    private int SUCCESS_WAIT_FRAME = 10;
    private int successWaitFrame = 0;

    [SerializeField]
    private float missPoint;
    [SerializeField]
    private int waitFrame = 20;
    private int waitCount;

    private GameObject checkClock;

    private CheckClock checkClock_cs;

    private GameObject clock;

    [SerializeField]
    private GameObject successAreaParent;

    [SerializeField]
    MicrowaveState status = MicrowaveState.Wait;

    [SerializeField]
    private float rotateSpeed;

    private System.Random rand = new System.Random();

    private float[] successAreaRotation = { -90f, -45f, 0, 45f, 90f };

    int oldRand = 5;

    [SerializeField]
    private GameObject missText;

    [SerializeField]
    private GameObject pointText;

    // Use this for initialization
    void Awake()
    {
        // 子オブジェクトである時計の針を取得
        checkClock = transform.Find("Canvas/Center/CheckClock").gameObject;
        clock = transform.Find("Canvas/Center/Clock").gameObject;
        checkClock_cs = transform.Find("Canvas/Center/CheckClock/CheckClockImage").gameObject.GetComponent<CheckClock>();
        successAreaParent = transform.Find("Canvas/Center/SuccessAreaParent").gameObject;


        // 最初は非表示
        gameObject.SetActive(false);

        // 今だけ
        gameObject.SetActive(true);
        checkClock.SetActive(false);
        successAreaParent.SetActive(false);
    }

    // Update is called once per frame
    public bool UpdateMicrowaveGage()
    {

        switch (status)
        {
            case MicrowaveState.Wait:
                waitCount++;
                if (waitCount >= waitFrame)
                {
                    status = MicrowaveState.Roll;
                    PlayStartSE();
                    waitCount = 0;
                }
                break;

            case MicrowaveState.Roll:
                clock.transform.Rotate(new Vector3(0, 0, -270 / timerMax));
                timerCount += 270f / timerMax;
                if (timerCount >= 270f)
                {
                    TIMER_COUNT = timerCount;
                    status = MicrowaveState.Start;
                    successAreaParent.SetActive(true);
                    DecisionSuceesAreaPosition();
                    checkClock.SetActive(true);
                }
                break;

            case MicrowaveState.Start:

                if (waitCount < 20)
                {
                    waitCount++;
                    break;
                }

                timerCount -= TIMER_COUNT / timer;
                clock.transform.Rotate(new Vector3(0, 0, TIMER_COUNT / timer));
                if (timerCount <= timerMin)
                {
                    status = MicrowaveState.Wait;
                    StopStartSE();
                    PlayChinSE();
                    HiddenText();
                    return true;
                }

                checkClock.transform.Rotate(new Vector3(0, 0, rotateSpeed));
                break;

            case MicrowaveState.Success:
                successWaitFrame++;
                timerCount -= TIMER_COUNT / timer;
                clock.transform.Rotate(new Vector3(0, 0, TIMER_COUNT / timer));
                if (timerCount <= timerMin)
                {
                    status = MicrowaveState.Wait;
                    StopStartSE();
                    PlayChinSE();
                    HiddenText();
                    return true;
                }
                if (successWaitFrame >= SUCCESS_WAIT_FRAME)
                {
                    status = MicrowaveState.Start;
                    checkClock.SetActive(true);
                }

                break;
        }

        return false;
    }

    /// <summary>
    /// 成功エリア再決定
    /// </summary>
    public void DecisionSuceesAreaPosition()
    {
        timer = (int)(timer / 1.25);
        int arrayElement = rand.Next(5);
        while (arrayElement == oldRand)
        {
            arrayElement = rand.Next(5);
        }
        successWaitFrame = 0;
        checkClock.transform.rotation = Quaternion.identity;
        checkClock.SetActive(false);
        oldRand = arrayElement;
        float rot = successAreaRotation[arrayElement];
        successAreaParent.transform.rotation = Quaternion.identity;
        successAreaParent.transform.Rotate(new Vector3(0, 0, rot));
    }

    public void ResetMicrowaveGage()
    {
        timer = 600;
        status = MicrowaveState.Wait;
        waitCount = 0;
        timerCount = 0;
        oldRand = 5;
        clock.transform.rotation = Quaternion.identity;
        checkClock.transform.rotation = Quaternion.identity;
        successAreaParent.transform.rotation = Quaternion.identity;
    }

    public void ChinMiss()
    {
        timerCount += missPoint;
        clock.transform.Rotate(new Vector3(0, 0, -missPoint));
        missText.GetComponent<MissAnnounce>().DisplayText();
    }

    public void DecisionCheckClockCollision()
    {
        if (status == MicrowaveState.Start)
        {
            checkClock_cs.GetComponent<CheckClock>().DecisionArea();
            status = MicrowaveState.Success;
        }
    }



    private void PlayStartSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[17], true, 4);
        Sound.PlaySe(GameSceneManager.seKey[17], 4);
    }

    public void StopStartSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[17], false, 4);
        Sound.StopSe(GameSceneManager.seKey[17], 4);
    }

    private void PlayChinSE()
    {
        Sound.PlaySe(GameSceneManager.seKey[15],4);
    }

    public void DisplayPoint(int point)
    {
        pointText.GetComponent<PointAnnouce>().DisplayText(point);
    }

    public void HiddenText()
    {
        pointText.SetActive(false);
        missText.SetActive(false);
    }

    public void SetCheckClockInMicrowave_cs(CookWareMw cs)
    {
        if (checkClock_cs == null)
        {
            checkClock_cs = transform.Find("Canvas/Center/CheckClock/CheckClockImage").gameObject.GetComponent<CheckClock>();
        }
        checkClock_cs.SetMicrowave_cs(cs);
    }
}
