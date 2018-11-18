using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flyingpan : KitchenwareBase
{
    // 焼きゲージスクリプト
    private GrilledGage grilledGage_cs;
    
    private int[] eatoyPoint = Enumerable.Repeat(0, 4).ToArray();

    private int basePoint;
    private int chain;

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
        basePoint = 1;

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
        miniGameUI.SetActive(false);
    }

    /// <summary>
    /// 調理キャンセル
    /// </summary>
    public override void CookingInterruption()
    {
        grilledGage_cs.SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage_cs.ResetPosition();
        grilledGage_cs.ResetSuccessArea();
        miniGameUI.gameObject.SetActive(false);
        SetIsCooking(false);
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
        if (hitSuccessArea == null)
        {
            ResetChain();
            return;
        }

        switch (hitSuccessArea.tag)
        {
            case "GrilledSuccessAreaNormal1":
                hitSuccessArea.transform.parent.gameObject.SetActive(false);
                grilledGage_cs.ResetIsHit(0);
                AddEatoyPoint(0, 1);
                Sound.PlaySe(GameSceneManager.seKey[8]);
                break;

            case "GrilledSuccessAreaNormal2":
                hitSuccessArea.transform.parent.gameObject.SetActive(false);
                grilledGage_cs.ResetIsHit(1);
                AddEatoyPoint(1, 2);
                Sound.PlaySe(GameSceneManager.seKey[9]);
                break;

            case "GrilledSuccessAreaHard":
                hitSuccessArea.SetActive(false);
                grilledGage_cs.ResetIsHit(2);
                AddEatoyPoint(2, 2);
                Sound.PlaySe(GameSceneManager.seKey[9]);
                break;

            case "GrilledSuccessAreaHell":
                hitSuccessArea.SetActive(false);
                grilledGage_cs.ResetIsHit(3);
                AddEatoyPoint(3, 3);
                Sound.PlaySe(GameSceneManager.seKey[10]);
                break;
        }
        chain++;
    }

    protected override int CalcEatoyPoint()
    {
        int sum = 0;
        for (int i = 0; i < eatoyPoint.Length; i++)
        {
            sum += eatoyPoint[i];
        }
        return (int)(basePoint * chain * 0.25f * sum);
    }

    public void ResetChain() => chain = 1;

    public void AddEatoyPoint(int e, int point) => eatoyPoint[e] += point;
}
