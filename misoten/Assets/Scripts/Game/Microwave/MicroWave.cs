using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Microwave : MonoBehaviour
{

    private GameObject microwaveGage;

    private GameObject microwaveCuisine;

    private int usingPlayerID;

    [SerializeField]
    private GameObject canvadsCamera;

    private bool isCooking = false;

	// Use this for initialization
	void Awake () {
        microwaveGage = Instantiate(Resources.Load("Prefabs/MicrowaveMiniGame") as GameObject, transform.position, Quaternion.identity);
        microwaveGage.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = canvadsCamera.GetComponent<Camera>();
        microwaveGage.SetActive(false);

    }

    public bool StartCookingMicrowave(int pID, GamePad.Index index)
    {
        if (isCooking) return false;  // すでに調理中なら抜ける

        microwaveCuisine = CuisineManager.GetInstance().GetMicrowaveController().OutputCuisine();   // 料理を取得
        if (microwaveCuisine == null) return false; // 料理が取得できなければ関数を抜ける
        SetIsCooking(true); // 調理中に変更
        SetUsingPlayerID(pID);    // 使用プレイヤーIDセット

        microwaveGage.SetActive(true);
        microwaveGage.GetComponent<MicrowaveGage2>().ResetMicrowaveGage();  // ゲージの状態をリセット
        
        return true;
    }

    public GameObject UpdateMicrowaveGage()
    {
        if (microwaveGage.GetComponent<MicrowaveGage2>().UpdateMicrowaveGage())
        {
            return EndCookingMicrowave();
        }

        return null;
    }

    public GameObject EndCookingMicrowave()
    {
        // ゲージを非表示
        microwaveGage.SetActive(false);
        SetIsCooking(false);

        return microwaveCuisine;
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    public bool InterruptionCooking()
    {
        // 中の料理をnull
        CuisineManager.GetInstance().GetMicrowaveController().OfferCuisine(microwaveCuisine.GetComponent<Food>().GetFoodID());
        microwaveCuisine = null;

        // ゲージを非表示
        microwaveGage.SetActive(false);

        return true;
    }

    private void SetIsCooking(bool flg) => isCooking = flg;

    public bool IsCooking() => isCooking;

    private void SetUsingPlayerID(int pID) => usingPlayerID = pID;

    public int GetUsingPlayerID() => usingPlayerID;

    public void AddCuisinePoint(int point) => microwaveCuisine.GetComponent<Food>().AddQualityTaste(point);

    public void DecisionCheckClockCollision()
    {
        microwaveGage.GetComponent<MicrowaveGage2>().DecisionCheckClockCollision();
    }
}
