﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilled : MonoBehaviour {


    // 状態
    [SerializeField]
    private bool isCooking = false;

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
        SetIsCooking(false);
    }

    /// <summary>
    /// 調理開始
    /// </summary>
    public void StartCooking(GamepadInput.GamePad.Index pNumber, int pRank)
    {
        SetIsCooking(true);
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
        SetIsCooking(false);
        grilledGage.GetComponent<GrilledGage>().SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage.GetComponent<GrilledGage>().ResetPosition();
        grilledGage.GetComponent<GrilledGage>().ResetSuccessArea();
        grilledGage.gameObject.SetActive(false);
        return grilledCuisine;
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    public void InterruptionCooking()
    {
        SetIsCooking(false);
        grilledGage.GetComponent<GrilledGage>().SetStatus(GrilledGage.EGrilledGageStatus.Standby);
        grilledGage.GetComponent<GrilledGage>().ResetPosition();
        grilledGage.GetComponent<GrilledGage>().ResetSuccessArea();
        grilledGage.gameObject.SetActive(false);

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(grilledCuisine.GetComponent<Food>().GetFoodID());
        grilledCuisine = null;
    }

    private void SetIsCooking(bool flg) => isCooking = flg;

    public bool IsCooking() => isCooking;
}
