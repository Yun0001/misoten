using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがプレイヤーに対して、チップを渡すスクリプト
/// </summary>
public class AlienChip : MonoBehaviour
{
    /// <summary>
    /// エイリアンがチップをプレイヤーに渡す種類
    /// </summary>
    private enum EChipPattern
    {
        PUT = 0,    // チップを置いていく
        HANDOVER,   // チップを直接渡す
        MAX         // 最大
    }

    /// <summary>
    /// エイリアンがお金を多く持っているかの種類
    /// </summary>
    private enum ERichDegree
    {
        Poverty = 0,    // 貧乏
        NORMAL,         // 普通
        RICHMAN,        // 金持ち
        RAND            // ランダム
    }

    // チップの渡し方
    [SerializeField]
    private EChipPattern chipPattern;

    // 金持ち度
    [SerializeField]
    private ERichDegree richDegree;

    // エイリアンのオーダー
    private AlienOrder alienOrder;

    // 料理が来たかの判定用
    private bool isCuisineCame = false;

    // チップの値
    private float chipVal = 0.0f;

    private int[] baseChip = { 100, 200, 300 };

    // 料理の旨味係数
    private float cisineTasteCoefficient = 0f;

    /// <summary>
    /// 開始関数
    /// </summary>
    void Start ()
	{
		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();

		// エイリアンの金持ち度をランダムで設定
		if(richDegree == ERichDegree.RAND) { richDegree = (ERichDegree)Random.Range((int)ERichDegree.Poverty, (int)ERichDegree.RAND); }

        // ベースチップをセット
        SetChipValue(baseChip[(int)richDegree]);
    }
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エイリアンが注文している時
		if (alienOrder.GetIsOrder() && Input.GetKey(KeyCode.Space))
		{
			// 料理が来た判定
			SetCuisineCame(true);
            if (isCuisineCame)
            {
                // 渡すor置くチップの計算
                SetChipValue(CalcChipValue());

                // チップの渡し方の管理
                switch (chipPattern)
                {
                    case EChipPattern.PUT:      // チップを置いていく

                        break;
                    case EChipPattern.HANDOVER: // チップを直接渡す

                        break;
                    default:
                        // 例外処理
                        break;
                }
            }
		
		}
	}

	/// <summary>
	/// 料理が来たかの判定を格納
	/// </summary>
	/// <param name="_isCuisineCame"></param>
	/// <returns></returns>
	public bool SetCuisineCame(bool _isCuisineCame) => isCuisineCame = _isCuisineCame;

	/// <summary>
	/// 料理が来たかの判定を取得
	/// </summary>
	/// <returns></returns>
	public bool GetCuisineCame() => isCuisineCame;

	/// <summary>
	/// エイリアンの金持ち度を取得
	/// </summary>
	/// <returns></returns>
	public int GetRichDegree() => (int)richDegree;

	/// <summary>
	/// チップの値を格納
	/// </summary>
	/// <param name="_chipVal"></param>
	/// <returns></returns>
	public float SetChipValue(float _chipVal) => chipVal = _chipVal;

	/// <summary>
	/// チップの値を取得
	/// </summary>
	/// <returns></returns>
	public float GetChipValue() => chipVal;

    /// <summary>
    /// ベースチップに掛ける旨味係数セット
    /// </summary>
    /// <param name="coefficient">料理の旨味係数</param>
    public void SetCuisineCoefficient(float coefficient) => cisineTasteCoefficient = coefficient;

    /// <summary>
    /// 渡すor置くチップの計算
    /// </summary>
    /// <returns>渡すor置くチップの値</returns>
    public int CalcChipValue() => (int)(chipVal * cisineTasteCoefficient);
}