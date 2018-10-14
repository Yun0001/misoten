using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuisineManager : Singleton<CuisineManager> {

    public readonly static int MaxCuisine = 4;
    private GameObject microwaveControllerPrefab;
    private GameObject microwaveController;
    private MicrowaveFoodController microwaveController_cs;
    private GameObject potControllerPrefab;
    private GameObject potController;
    private PotFoodController potController_cs;
    private GameObject grilledControllerPrefab;
    private GameObject grilledController;
    private GrilledFoodController grilledController_cs;
    // Use this for initialization
     void Awake () {
        InstantiateMicrowaveController();
        InstantiatePotController();
        InstantiateGrilledController();  
    }

    /// <summary>
    /// 電子レンジ料理のコントローラー
    /// </summary>
    private void InstantiateMicrowaveController()
    {
        microwaveControllerPrefab = (GameObject)Resources.Load("Prefabs/Common/MicrowaveCuisineController");
        microwaveController = Instantiate(microwaveControllerPrefab, transform.position, Quaternion.identity);
        microwaveController_cs = microwaveController.GetComponent<MicrowaveFoodController>();
    }

    private void InstantiatePotController()
    {
        potControllerPrefab = (GameObject)Resources.Load("Prefabs/Common/PotCuisineController");
        potController = Instantiate(potControllerPrefab, transform.position, Quaternion.identity);
        potController_cs = potController.GetComponent<PotFoodController>();
    }

    private void InstantiateGrilledController()
    {
        grilledControllerPrefab = (GameObject)Resources.Load("Prefabs/Common/GrilledCuisineController");
        grilledController = Instantiate(grilledControllerPrefab, transform.position, Quaternion.identity);
        grilledController_cs = grilledController.GetComponent<GrilledFoodController>();
    }

    public MicrowaveFoodController GetMicrowaveController() => microwaveController_cs;
    public PotFoodController GetPotController() => potController_cs;
    public GrilledFoodController GetGrilledController() => grilledController_cs;

}
