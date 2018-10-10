using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilled : MonoBehaviour {

    public enum GrilledState
    {
        unused,
        inCcoking
    }

    private GameObject grilledFoodPrefab;

    [SerializeField]
    private GameObject sideGagePrefab;

    private GameObject sideGage;

    [SerializeField]
    private float COOKING_TIME;

    private float cookingTime;

    private GrilledState grilledStatus;

    private float grilledFoodScore;

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
        cookingTime = 0;
        grilledFoodScore = 0;
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCooking()
    {
        grilledStatus = GrilledState.inCcoking;
        cookingTime = COOKING_TIME;
        sideGage.SetActive(true);
    }

    public bool UpdateGrilled()
    {
        cookingTime -= Time.deltaTime;
        if (cookingTime <= 0) return true;
        return false;
    }

    public void CalcGrilledFoodTasteCoefficient()=> sideGagePrefab.GetComponent<Food>().CalcTasteCoefficient(CalcDifference());

    public void EndCooking()
    {
        grilledStatus = GrilledState.unused;
        grilledFoodScore = 0;
        sideGage.SetActive(false);
    }

    public GrilledState GetStatus() => grilledStatus;

    public GameObject GetGrilledFood() => grilledFoodPrefab;

    private float CalcDifference()
    {
        return sideGage.GetComponent<MicroWaveGage>().GetSliderVal() == 0 ? 0 : sideGage.GetComponent<MicroWaveGage>().GetSliderVal() / 100;
    }
}
