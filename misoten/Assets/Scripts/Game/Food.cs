using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 食材クラス
/// </summary>
public class Food : MonoBehaviour {

    public enum Category
    {
        Grilled,
        Microwave,
        Pot,
    }

    /// <summary>
    /// この食材の所有プレイヤーID
    /// </summary>
    private int ownershipPlayerID;

    /// <summary>
    /// 宇宙人に料理を渡した時にもらえるチップ割合Max100%Min70%
    /// </summary>
   [SerializeField]
    private float qualityTaste;

    private int foodID;

    private Category category;


    // Use this for initialization
    void Start()
    {
        qualityTaste = 1;
    }

    public void SubQualityTaste()
    {
        if (qualityTaste > 0.7f)
        {
            qualityTaste -= Time.deltaTime / 10;
            if (qualityTaste < 0.7f)
            {
                qualityTaste = 0.7f;
            }
        }
    }

    /// <summary>
    /// 旨味係数を計算
    /// </summary>
    /// <param name="coefficient"></param>
    public void CalcTasteCoefficient(float coefficient)
    {
        qualityTaste -= coefficient;
    }

    public void SetOwnershipPlayerID(int ID) => ownershipPlayerID = ID;

    public int GetOwnershipPlayerID() => ownershipPlayerID;

    public float GetQualityTaste()
    {
        return qualityTaste;
    }

    public void SetFoodID(int ID) => foodID = ID;

    public int GetFoodID() => foodID;

    public void Init()
    {
        qualityTaste = 1;
    }

    public void SetCategory(Category _category) => category = _category;

    public int GetCategory() => (int)category;
}
