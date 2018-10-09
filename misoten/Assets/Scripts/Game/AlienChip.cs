using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがプレイヤーに対して、チップを渡すスクリプト
/// </summary>
public class AlienChip : MonoBehaviour
{
	/// <summary>
	/// エイリアンがチッププレイヤーに渡す種類
	/// </summary>
	private enum EChipPattern
	{
		PUT = 0,	// チップを置いていく
		HANDOVER,	// チップを直接渡す
		MAX			// 最大
	}

	// チップの渡し方
	[SerializeField]
	private EChipPattern chipPattern;

	// エイリアンのオーダー
	private AlienOrder alienOrder;

	// 料理が来たかの判定用
	private bool isCuisineCame = false;

	// チップの値
	private float chipVal = 0.0f;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();
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

			// チップの渡し方の管理
			switch (chipPattern)
			{
				case EChipPattern.PUT:		// チップを置いていく

					break;
				case EChipPattern.HANDOVER: // チップを直接渡す

					break;
				default:
					// 例外処理
					break;
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
}