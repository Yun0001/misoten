using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilled : MonoBehaviour {

    public enum GrilledState
    {
        unused,
        inCcoking
    }

    private GameObject grilledCuisine;

    private GameObject grilledGagePrefab;

    private GameObject grilledGage;

    private GrilledState grilledStatus;

    private Vector3 pPos;


	// Use this for initialization
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
        Vector3 pPos = pos;
        pPos.y++;
        pPos.z = -0.03f;
        grilledGage.transform.position = pPos;
        grilledGage.transform.Find("TimingPoint").GetComponent<TimingPoint>().SetPlayerNumber(pNumber);
    }

    public bool IsEndCooking() => grilledGage.GetComponent<GrilledGage>().GetStatus() == GrilledGage.EGrilledGageStatus.End;

    public void CalcGrilledFoodTasteCoefficient()=> grilledGage.GetComponent<Food>().CalcTasteCoefficient(CalcDifference());

    public void EndCooking()
    {
        grilledStatus = GrilledState.unused;
        grilledGage.SetActive(false);
    }

    public GrilledState GetStatus() => grilledStatus;

    public GameObject GetGrilledCuisine() => grilledCuisine;

    private float CalcDifference()
    {
        return grilledGage.GetComponent<MicroWaveGage>().GetSliderVal() == 0 ? 0 : grilledGage.GetComponent<MicroWaveGage>().GetSliderVal() / 100;
    }

    private void GrilledGageInstance()
    {
        grilledGagePrefab = (GameObject)Resources.Load("Prefabs/GrilledGage/GrilledGage");
        grilledGage = Instantiate(grilledGagePrefab, transform.position, Quaternion.identity);
        grilledGage.SetActive(false);
        
        grilledGage.transform.Find("TimingPoint").GetComponent<TimingPoint>().Init(GetComponent<Grilled>());
    }

    public GameObject GrilledCookingEnd()
    {
        grilledGage.GetComponent<GrilledGage>().SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage.GetComponent<GrilledGage>().ResetPosition();
        grilledGage.gameObject.SetActive(false);
        return grilledCuisine;
    }

}
