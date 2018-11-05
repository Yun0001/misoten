using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

    [SerializeField]
    private GameObject cuisine;

    private GameObject potGage;
    private GameObject frame;

    private bool isCooking = false;

    [SerializeField]
    private GameObject PotUICamera;

   


    private void Awake()
    {
        potGage = Instantiate(Resources.Load("Prefabs/PotMiniGame") as GameObject, transform.position, Quaternion.identity);

        potGage.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = PotUICamera.GetComponent<Camera>();
        frame = potGage.transform.Find("Canvas/JoyStick/SecondHandFrame").gameObject;
        potGage.SetActive(false);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void ResetStatus()
    {
        SetIsCooking(false);
        potGage.SetActive(false);
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public bool StartCookingPot(int playerID)
    {
        if (isCooking) return false;

        cuisine = CuisineManager.GetInstance().GetPotController().OutputCuisine();
        if (cuisine == null) return false; // 料理が取得できなければ関数を抜ける

        SetIsCooking(true);
        
        potGage.SetActive(true);
        potGage.transform.Find("Canvas/JoyStick").GetComponent<Joystick>().Init(playerID);

        return true;
    }

    /// <summary>
    /// 調理中の更新処理
    /// </summary>
    /// <param name="stickVec">スティックが倒れている方向</param>
    public bool UpdateCooking()
    {
        
        if (frame.GetComponent<IraIraFrame>().GetOneRotationFlag())
        {
            // 調理終了
            ResetStatus();         // 初期化
            return true;
        }
        
        return false;
    }



    /// <summary>
    /// ステータス取得
    /// </summary>
    /// <returns>ステータス</returns>
    public bool IsCooking() => isCooking;

    /// <summary>
    /// 茹で料理を返す
    /// </summary>
    /// <returns>茹で料理</returns>
    public GameObject GetPotCuisine() => cuisine;

    /// <summary>
    /// キャンセル
    /// </summary>
    public void InterruptionCooking()
    {
        ResetStatus();

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(cuisine.GetComponent<Food>().GetFoodID());
        cuisine = null;
    }

    public void SetIsCooking(bool flg) => isCooking = flg;
}
