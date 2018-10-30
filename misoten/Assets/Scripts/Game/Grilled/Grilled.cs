using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilled : MonoBehaviour {

    public enum GrilledState
    {
        unused,
        inCcoking
    }

    // 状態
    private GrilledState grilledStatus;

    // 焼き料理
    private GameObject grilledCuisine;

    // 焼きゲージのプレハブ
    private GameObject grilledGagePrefab;

    // 焼きゲージ
    private GameObject grilledGage;

    // 焼きゲージプレハブのパス
    private string grilledGagePass = "Prefabs/GrilledGage/GrilledGage";


    // 初期処理
    void Awake ()
    {
        GrilledGageInstance(); // ゲージ作成
        grilledStatus = GrilledState.unused;
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCooking(GamepadInput.GamePad.Index pNumber, int pRank, Vector3 pos)
    {
        grilledStatus = GrilledState.inCcoking;
        // 料理をコントローラーから取得
        grilledCuisine = CuisineManager.GetInstance().GetGrilledController().OutputCuisine();
        grilledGage.SetActive(true);
        grilledGage.GetComponent<GrilledGage>().Init(pRank);

        Vector3 gPos = transform.position;
        gPos.z-=1f;
        grilledGage.transform.position = gPos;
        grilledGage.transform.Find("TimingPoint").GetComponent<TimingPoint>().SetPlayerNumber(pNumber);
    }

    /// <summary>
    /// ゲージの状態がENDかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsEndCooking() => grilledGage.GetComponent<GrilledGage>().GetStatus() == GrilledGage.EGrilledGageStatus.End;

    public GrilledState GetStatus() => grilledStatus;

    public GameObject GetGrilledCuisine() => grilledCuisine;

    /// <summary>
    /// 焼きゲージ生成
    /// </summary>
    private void GrilledGageInstance()
    {
        grilledGagePrefab = Resources.Load(grilledGagePass) as GameObject;
        grilledGage = Instantiate(grilledGagePrefab, transform.position, Quaternion.identity);
        grilledGage.SetActive(false);
        
        grilledGage.transform.Find("TimingPoint").GetComponent<TimingPoint>().Init(GetComponent<Grilled>());
    }

    /// <summary>
    /// 調理終了
    /// </summary>
    /// <returns></returns>
    public GameObject GrilledCookingEnd()
    {
        grilledStatus = GrilledState.unused;
        grilledGage.GetComponent<GrilledGage>().SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage.GetComponent<GrilledGage>().ResetPosition();
        grilledGage.gameObject.SetActive(false);
        return grilledCuisine;
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    public void InterruptionCooking()
    {
       grilledStatus = GrilledState.unused;
        grilledGage.GetComponent<GrilledGage>().SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage.GetComponent<GrilledGage>().ResetPosition();
        grilledGage.gameObject.SetActive(false);

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(grilledCuisine.GetComponent<Food>().GetFoodID());
        grilledCuisine = null;
    }
}
