using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 食材クラス
/// </summary>
public class Food : MonoBehaviour {

    /// <summary>
    /// この食材の所有プレイヤーID
    /// </summary>
    private int ownershipPlayerID;

    /// <summary>
    /// 味の質
    /// </summary>
    private float qualityTaste;

    // Use this for initialization
    void Start() {
        //　テスト用
        ownershipPlayerID = 1;
        qualityTaste = 1;
            }

    public void SetOwnershipPlayerID(int ID)
    {
        ownershipPlayerID = ID;
    }

    public int GetOwnershipPlayerID()
    {
        return ownershipPlayerID;
    }

    public void SubQualityTaste()
    {
        qualityTaste -= Time.deltaTime/10;
    }

    /// <summary>
    /// 旨味係数を計算
    /// </summary>
    /// <param name="coefficient"></param>
    public void CalcTasteCoefficient(float coefficient) => qualityTaste -= coefficient;
}
