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

    [SerializeField]
    private GameObject sideGagePrefab;

    private GameObject sideGage;

    private GrilledState grilledStatus;


	// Use this for initialization
	void Awake () {
        sideGagePrefab = (GameObject)Resources.Load("Prefabs/UI/SideGage");
        sideGage = Instantiate(sideGagePrefab, transform.position, Quaternion.identity);
        Vector3 pos = transform.position;
        Vector3 scale = new Vector3(0.1f, 0.3f, 1);
        sideGage.transform.position = pos;
        sideGage.transform.localScale = scale;
        sideGage.SetActive(false);
        grilledStatus = GrilledState.unused;
        sideGage.GetComponent<Canvas>().sortingOrder = 2;
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCooking()
    {
        grilledStatus = GrilledState.inCcoking;
        sideGage.SetActive(true);
        grilledCuisine = CuisineManager.GetInstance().GetGrilledController().OutputCuisine();
    }

    public bool UpdateGrilled()
    {
        return false;
    }

    public void CalcGrilledFoodTasteCoefficient()=> sideGagePrefab.GetComponent<Food>().CalcTasteCoefficient(CalcDifference());

    public void EndCooking()
    {
        grilledStatus = GrilledState.unused;
        sideGage.SetActive(false);
    }

    public GrilledState GetStatus() => grilledStatus;

    public GameObject GetGrilledCuisine() => grilledCuisine;

    private float CalcDifference()
    {
        return sideGage.GetComponent<MicroWaveGage>().GetSliderVal() == 0 ? 0 : sideGage.GetComponent<MicroWaveGage>().GetSliderVal() / 100;
    }
}
