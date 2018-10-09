using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

    public enum PotState
    {
        unused,
        inCcoking
    }

    [SerializeField]
    private float cookingTime;

    [SerializeField]
    private float COOKING_TIME;

    [SerializeField]
    private GameObject potFoodPrefab;

    private enum StickState
    {
        UpLeft,
        DownLeft,
        UpRight,
        DownRight,
        None
    }

    [SerializeField]
    private PotState potStatus;

    [SerializeField]
    private StickState stickStatus;
    private StickState oldStickStatus;
    [SerializeField]
    private StickState startStatus;

    [SerializeField]
    private bool[] rotationFlg = new bool[4];
    [SerializeField]
    private int rotationCount;


    private void Awake()
    {
        potFoodPrefab = (GameObject)Resources.Load("Prefabs/PotFood");
        Init();
    }

    private void Init()
    {
        potStatus = PotState.unused;
        stickStatus = StickState.None;
        oldStickStatus = StickState.None;
        startStatus = StickState.None;
        for (int i = 0; i < rotationFlg.Length; i++)
        {
            rotationFlg[i] = false;
        }
        rotationCount = 0;
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCookingPot()
    {
        cookingTime = COOKING_TIME;
        potStatus = PotState.inCcoking;
    }
    /// <summary>
    /// 調理中の更新処理
    /// </summary>
    /// <param name="stickVec"></param>
    public bool UpdateCooking(Vector2 stickVec)
    {
        RotationDetectionStick(stickVec);
        cookingTime -= Time.deltaTime;
        //調理終了
        if (cookingTime <= 0)
        {
            //　初期化
            Init();
            //　茹で料理を返す
            return true;
        }

        return false;
    }

    /// <summary>
    /// スティックの回転状態を判定
    /// </summary>
    /// <param name="stickVec"></param>
    private void RotationDetectionStick(Vector2 stickVec)
    {
        switch (stickStatus)
        {
            case StickState.UpRight:
                if (stickVec.x < 0 && stickVec.y < 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.UpLeft;
                }
                else if (stickVec.x < 0 && stickVec.y > 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.DownRight;
                }

                break;

            case StickState.UpLeft:
                if (stickVec.x > 0 && stickVec.y < 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.UpRight;
                }
                else if (stickVec.x < 0 && stickVec.y > 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.DownLeft;
                }
                break;

            case StickState.DownLeft:
                if (stickVec.x > 0 && stickVec.y > 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.DownRight;
                }
                else if (stickVec.x > 0 && stickVec.y < 0 )
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.UpRight;
                }
                break;

            case StickState.DownRight:
                if (stickVec.x < 0 && stickVec.y > 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.DownLeft;
                }
                else if (stickVec.y < 0 && stickVec.x > 0)
                {
                    oldStickStatus = stickStatus;
                    stickStatus = StickState.UpRight;
                }
                break;

            case StickState.None:
                if (stickVec.x < 0)
                {
                    if (stickVec.y < 0) stickStatus = StickState.UpLeft;
                    else if (stickVec.y > 0) stickStatus = StickState.DownLeft;
                }
                else if (stickVec.x > 0)
                {
                    if (stickVec.y < 0) stickStatus = StickState.UpRight;
                    else if (stickVec.y > 0) stickStatus = StickState.DownRight;
                }
                startStatus = stickStatus;
                break;
        }
        if (stickStatus == StickState.None) return;
        rotationFlg[(int)stickStatus] = true;
        DetectionOneturn();
    }


    /// <summary>
    /// 1回転したか判定
    /// </summary>
    private void DetectionOneturn()
    {
        for (int i = 0; i < rotationFlg.Length; i++)
        {
            if (!rotationFlg[i]) return;
        }

        for (int i = 0; i < rotationFlg.Length; i++)
            rotationFlg[i] = false;

        //rotationFlg[(int)startStatus] = true;
        stickStatus = StickState.None;
        oldStickStatus = StickState.None;
        startStatus = StickState.None;
        rotationCount++;
    }


    public PotState GetStatus() => potStatus;

    public GameObject GetPotFood() => potFoodPrefab;
}
