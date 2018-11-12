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
        Pot,
        Microwave,
    }

	public enum CategoryChange
	{
		PURPLE,
		ORANGE,
		GREEN,
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
	private CategoryChange categoryChange;


	/// <summary>
	/// 初期処理
	/// </summary>
	void Awake() => Init();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init() => qualityTaste = 1;

    public void SubQualityTaste()
    {
        if (qualityTaste > 0.7f)
        {
            qualityTaste -= Time.deltaTime / 10;
            if (qualityTaste < 0.7f) qualityTaste = 0.7f;
        }
    }

    /// <summary>
    /// 旨味係数を計算
    /// </summary>
    /// <param name="coefficient"></param>
    public void CalcTasteCoefficient(float coefficient) => qualityTaste -= coefficient;
    
    /// <summary>
    /// 所有プレイヤーIDセット
    /// </summary>
    /// <param name="ID">プレイヤーID</param>
    public void SetOwnershipPlayerID(int ID) => ownershipPlayerID = ID;

    /// <summary>
    /// 所有プレイヤー取得
    /// </summary>
    /// <returns>所有プレイヤーID</returns>
    public int GetOwnershipPlayerID() => ownershipPlayerID;

    /// <summary>
    /// 旨味係数取得
    /// </summary>
    /// <returns>旨味係数</returns>
    public float GetQualityTaste() => qualityTaste;

    /// <summary>
    /// 料理IDセット
    /// </summary>
    /// <param name="ID">料理ID</param>
    public void SetFoodID(int ID) => foodID = ID;

    /// <summary>
    /// 料理ID取得
    /// </summary>
    /// <returns>料理ID</returns>
    public int GetFoodID() => foodID;

	/// <summary>
	/// 料理の種類セット
	/// </summary>
	/// <param name="_category"></param>
	public void SetCategory(Category _category) => category = _category;

    /// <summary>
    /// 料理の種類取得
    /// </summary>
    /// <returns>料理の種類</returns>
    public Category GetCategory() => category;

	/// <summary>
	/// 料理の種類(チェンジ)セット
	/// </summary>
	/// <param name="_categoryChange"></param>
	public void SetCategoryChange(CategoryChange _categoryChange) => categoryChange = _categoryChange;

	/// <summary>
	/// 料理の種類(チェンジ)取得
	/// </summary>
	/// <returns></returns>
	public CategoryChange GetCategoryChange() => categoryChange;

	public void AddQualityTaste(int point) => qualityTaste += point;
}
