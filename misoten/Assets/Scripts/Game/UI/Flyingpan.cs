using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingpan : KitchenwareBase
{
    // 焼きゲージスクリプト
    private GrilledGage grilledGage_cs;


    void Awake () {
        InstanceMiniGameUI();
        grilledGage_cs = miniGameUI.GetComponent<GrilledGage>();
    }
	
   /// <summary>
   /// ミニゲームUI実体化
   /// </summary>
    protected override void InstanceMiniGameUI()
    {
        miniGameUI = Instantiate(Resources.Load("Prefabs/GrilledGage/GrilledGage") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.SetActive(false);
   }

    /// <summary>
    /// ミニゲーム初期化
    /// </summary>
    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        grilledGage_cs.Init(1);

        Vector3 gPos = transform.position;
        gPos.z -= 1f;
        miniGameUI.transform.position = gPos;
    }

    /// <summary>
    /// 調理
    /// </summary>
    protected override bool Cooking()
    {
        return grilledGage_cs.CookingGrilledCuisine();
    } 

    /// <summary>
    /// UIリセット
    /// </summary>
    protected override void ResetMiniGameUI()
    {
        grilledGage_cs.SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage_cs.ResetPosition();
        grilledGage_cs.ResetSuccessArea();
        miniGameUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 調理キャンセル
    /// </summary>
    public override void CookingInterruption()
    {
        ResetMiniGameUI();

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(cuisine.GetComponent<Food>().GetFoodID());
        cuisine = null;
    }

    /// <summary>
    /// コントローラーから料理を取得
    /// </summary>
    /// <returns>料理</returns>
    protected override GameObject SetCuisine() => CuisineManager.GetInstance().GetGrilledController().OutputCuisine();

    public void DecisionTimingPointCollision()
    {
        GameObject hitSuccessArea = grilledGage_cs.DecisionIsHit();
        if (hitSuccessArea == null) return;

        cuisine.GetComponent<Food>().AddQualityTaste(hitSuccessArea.GetComponent<GrilledPoint>().GetPoint());
        switch (hitSuccessArea.tag)
        {
            case "GrilledSuccessAreaNormal1":
                hitSuccessArea.transform.parent.gameObject.SetActive(false);
                grilledGage_cs.ResetIsHit(0);
                break;

            case "GrilledSuccessAreaNormal2":
                hitSuccessArea.transform.parent.gameObject.SetActive(false);
                grilledGage_cs.ResetIsHit(1);
                break;

            case "GrilledSuccessAreaHard":
                hitSuccessArea.SetActive(false);
                grilledGage_cs.ResetIsHit(2);
                break;

            case "GrilledSuccessAreaHell":
                hitSuccessArea.SetActive(false);
                grilledGage_cs.ResetIsHit(3);
                break;
        }
    }
}
