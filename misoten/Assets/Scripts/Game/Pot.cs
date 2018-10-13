using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

    public enum PotState
    {
        unused,
        inCcoking
    }

    private enum StickState
    {
        UpLeft,
        DownLeft,
        UpRight,
        DownRight,
        None
    }
    [SerializeField]
    private GameObject potCuisine;


    private PotState potStatus;
    private StickState stickStatus;
    private StickState oldStickStatus;
    private StickState startStatus;
    bool isOneturn;

    [SerializeField]
    private bool[] rotationFlg = new bool[4];
    private int rotationCount;


    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        potStatus           = PotState.unused;
        stickStatus         = StickState.None;
        oldStickStatus    = StickState.None;
        startStatus         = StickState.None;
        rotationCount       = 0;
        isOneturn = false;
        for (int i = 0; i < rotationFlg.Length; i++)
            rotationFlg[i] = false;
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCookingPot()
    {
        potStatus     = PotState.inCcoking;
        potCuisine = CuisineManager.GetInstance().GetPotController().OutputCuisine();
    }

    /// <summary>
    /// 調理中の更新処理
    /// </summary>
    /// <param name="stickVec">スティックが倒れている方向</param>
    public bool UpdateCooking(Vector2 stickVec)
    {
        RotationDetectionStick(stickVec);
        if (isOneturn)
        {
            // 調理終了
            Init();         // 初期化
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
            if (!rotationFlg[i]) return;

        isOneturn = true;
    }

    /// <summary>
    /// ステータス取得
    /// </summary>
    /// <returns>ステータス</returns>
    public PotState GetStatus() => potStatus;

    /// <summary>
    /// 茹で料理を返す
    /// </summary>
    /// <returns>茹で料理</returns>
    public GameObject GetPotCuisine() => potCuisine;
}
