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
    private GameObject potCuisine;

    private GameObject potGagePrefab;
    private GameObject potGage;
    private GameObject frame;

    private PotState potStatus;

   


    private void Awake()
    {
        potGagePrefab = Resources.Load("Prefabs/PotMiniGame") as GameObject;
        potGage = Instantiate(potGagePrefab, transform.position, Quaternion.identity);
        potGage.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        frame = potGage.transform.Find("Canvas/JoyStick/SecondHandFrame").gameObject;
        potGage.SetActive(false);
        potStatus = PotState.unused;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        potStatus = PotState.unused;
        potGage.SetActive(false);
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCookingPot(int playerID)
    {
        potStatus     = PotState.inCcoking;
        potCuisine = CuisineManager.GetInstance().GetPotController().OutputCuisine();
        potGage.transform.Find("Canvas/JoyStick").GetComponent<Joystick>().Init(playerID);
        potGage.SetActive(true);
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
            Init();         // 初期化
            return true;
        }
        
        return false;
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

    /// <summary>
    /// キャンセル
    /// </summary>
    public void InterruptionCooking()
    {
        potStatus = PotState.unused;
        potGage.gameObject.SetActive(false);

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(potCuisine.GetComponent<Food>().GetFoodID());
        potCuisine = null;
    }
}
