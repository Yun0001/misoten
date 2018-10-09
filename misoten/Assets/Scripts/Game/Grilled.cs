using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilled : MonoBehaviour {

    public enum GrilledState
    {
        unused,
        inCcoking
    }

    [SerializeField]
    private GameObject sideGagePrefab;

    private GameObject sideGage;

    [SerializeField]
    private float COOKING_TIME;

    private float cookingTime;

    private GrilledState grilledStatus;

	// Use this for initialization
	void Awake () {
        sideGagePrefab = (GameObject)Resources.Load("Prefabs/UI/SideGage");
        grilledStatus = GrilledState.unused;
        cookingTime = 0;
        sideGage = Instantiate(sideGagePrefab, transform.position, Quaternion.identity);
        Vector3 pos = transform.position;
        Vector3 scale = new Vector3(0.1f, 0.3f, 1);
        sideGage.transform.position = pos;
        sideGage.transform.localScale = scale;
        sideGage.SetActive(false);
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
}
