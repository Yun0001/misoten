using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 料理コントローラーを管理するクラス（シングルトン）
/// </summary>
public class CuisineManager : Singleton<CuisineManager>
{
    private enum ECuisineController
    {
        MicrowaveCuisineController,
        PotCuisineController,
        GrilledCuisineController,
        MaxController
    }

    /// <summary>
    /// Prefabが保存されている階層へのパス
    /// </summary>
    private string[] prefabPass = 
    {
        "Prefabs/Common/MicrowaveCuisineController",
        "Prefabs/Common/PotCuisineController",
        "Prefabs/Common/GrilledCuisineController"
    };

    /// <summary>
    /// 料理コントローラーのPrefab
    /// </summary>
    private GameObject[] cuisineControllerPrefab = new GameObject[(int)ECuisineController.MaxController];

    /// <summary>
    /// 料理コントローラー
    /// </summary>
    private GameObject[] cuisineController = new GameObject[(int)ECuisineController.MaxController];

    // 各料理コントローラースクリプト
    private MicrowaveCuisineController microwaveController_cs;
    private PotCuisineController potController_cs;
    private GrilledCuisineController grilledController_cs;

    // 初期処理
    private new void Awake()
    {
        for (int i = 0; i < (int)ECuisineController.MaxController; i++)
        {
            cuisineControllerPrefab[i] = Resources.Load(prefabPass[i]) as GameObject;
            cuisineController[i] = Instantiate(cuisineControllerPrefab[i], transform.position, Quaternion.identity);
        }

        microwaveController_cs = cuisineController[(int)ECuisineController.MicrowaveCuisineController].GetComponent<MicrowaveCuisineController>();
        potController_cs = cuisineController[(int)ECuisineController.PotCuisineController].GetComponent<PotCuisineController>();
        grilledController_cs = cuisineController[(int)ECuisineController.GrilledCuisineController].GetComponent<GrilledCuisineController>();
    }

    // 料理コントローラースクリプト取得
    public MicrowaveCuisineController GetMicrowaveController() => microwaveController_cs;
    public PotCuisineController GetPotController() => potController_cs;
    public GrilledCuisineController GetGrilledController() => grilledController_cs;

}
