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

    [SerializeField]
    private GameObject checkClock;

    private CheckClock checkClock_cs;

    [SerializeField]
    private GameObject clock;

    [SerializeField]
    private GameObject successAreaParent;

    MicrowaveState status = MicrowaveState.Wait;

    [SerializeField]
    private float rotateSpped;

    private System.Random rand = new System.Random();

    private float[] successAreaRotation = { 112.5f, 157.5f, 202.5f, 247.5f, 292.5f };

    int oldRand = 5;

    [SerializeField]
    GameObject microwave;


    // Use this for initialization
    void Awake()
    {
        // 子オブジェクトである時計の針を取得
        checkClock = transform.Find("Canvas/Center/CheckClock").gameObject;
        clock = transform.Find("Canvas/Center/Clock").gameObject;
        checkClock_cs = transform.Find("Canvas/Center/CheckClock/CheckClockImage").gameObject.GetComponent<CheckClock>();
        successAreaParent = transform.Find("Canvas/Center/SuccessAreaParent").gameObject;

        microwave = GameObject.Find("microwave1");

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
                if (timerCount <= timerMin) return true;

                checkClock.transform.Rotate(new Vector3(0, 0, rotateSpped));
                break;

            case MicrowaveState.Success:
                successWaitFrame++;
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
    }

    public void DecisionCheckClockCollision()
    {
        if (status == MicrowaveState.Start)
        {
            checkClock_cs.GetComponent<CheckClock>().DecisionArea();
            status = MicrowaveState.Success;
        }
    }
}
