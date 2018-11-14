using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KitchenwareBase : MonoBehaviour
{
    [SerializeField]
    protected bool isDebugMode;

    // 状態
    protected bool isCooking = false;

    [SerializeField]
    protected bool isEnd = false;

    // 各料理のミニゲームUI
    protected GameObject miniGameUI;

    // 料理
    protected GameObject cuisine;

    // キャンバスカメラ
    [SerializeField]
    protected GameObject canvasCamera;

    /// <summary>
    /// 調理開始
    /// </summary>
    /// <returns></returns>
    public bool CookingStart(GameObject eatoy)
    {
        if (!eatoy.GetComponent<Eatoy>().IsIcing()) return false;

        if (isDebugMode)
        {
            // とりあえずデバッグモードはポイント１
            cuisine = eatoy;
            cuisine.GetComponent<Eatoy>().AddPoint(1);
            cuisine.GetComponent<Eatoy>().Thawing();
            return true;
        }

        // 既に調理中なら抜ける
        if (isCooking) return false;

        // イートイをセット
        cuisine = eatoy;

        // 料理をコントローラーから取得できなければ抜ける
        if (cuisine == null) return false;

        // 状態を調理中に変更
        SetIsCooking(true);

        SetIsEnd(false);

        // ここに各料理のUI初期化関数を記述
        InitMiniGameUI();

        // 調理開始
        return true;

}


    public GameObject UpdateMiniGame()
    {
        if (isDebugMode) return cuisine;

        // 調理をすすめる
        if (Cooking())
        {
            ResetMiniGameUI();
            SetIsCooking(false);
            SetIsEnd(true);
            cuisine.GetComponent<Eatoy>().Thawing();
            return cuisine;
        }

        // nullを返す
        return null;
    }

    protected void SetIsEnd(bool flg) => isEnd = flg;

    protected void SetIsCooking(bool flg) => isCooking = flg;

    public bool IsCooking() => isCooking;

    public GameObject GetCuisine() => cuisine;


    /// <summary>
    /// ミニゲームUIを実体化
    /// </summary>
    protected abstract void InstanceMiniGameUI();

    /// <summary>
    /// 料理をコントローラーから取得
    /// </summary>
    /// <returns></returns>
    protected abstract GameObject SetCuisine();

    /// <summary>
    /// ミニゲームの初期化
    /// </summary>
    protected abstract void InitMiniGameUI();

    protected abstract void ResetMiniGameUI();

    public abstract void CookingInterruption();

    protected abstract bool Cooking();
}
