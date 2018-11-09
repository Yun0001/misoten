using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの満足スクリプト
/// </summary>
public class AlienSatisfaction : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 満足描画用
	[SerializeField]
	GameObject satisfactionBalloon;

	// 満足を行う時間
	[SerializeField, Range(1.0f, 10.0f)]
	float satisfactionTime;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 満足フラグ(チップ取得時用)
	private static bool satisfactionChipFlag = false;

	// 満足時間
	private static float satisfactionTimeAdd = 0.0f;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 満足フラグ
	private bool satisfactionFlag = false;

	// ---------------------------------------------

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 満足した場合
		if(GetSatisfactionFlag())
		{
			// 満足を指定ない時
			if (!AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD))
			{
				// 満足した吹き出しを出す
				satisfactionBalloon.SetActive(true);

				// 満足フラグ(チップ取得時用)をON
				satisfactionChipFlag = true;
			}

			// 満足時間が指定時間を超えた場合
			if (satisfactionTimeAdd >= satisfactionTime)
			{
				// 時間の初期化
				satisfactionTimeAdd = 0.0f;

				// 満足した吹き出しを消す
				satisfactionBalloon.SetActive(false);

				// 帰る(良)状態「ON」
				AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD);

				// 退店時の移動開始
				GetComponent<AlienMove>().SetWhenLeavingStoreFlag(true);
			}

			// 毎フレームの時間を加算
			satisfactionTimeAdd += Time.deltaTime;
		}
	}

	/// <summary>
	/// 満足フラグの格納
	/// </summary>
	/// <param name="_satisfactionFlag"></param>
	/// <returns></returns>
	public bool SetSatisfactionFlag(bool _satisfactionFlag) => satisfactionFlag = _satisfactionFlag;

	/// <summary>
	/// 満足フラグの取得
	/// </summary>
	/// <returns></returns>
	public bool GetSatisfactionFlag() => satisfactionFlag;

	/// <summary>
	/// 満足フラグ(チップ取得時用)の格納
	/// </summary>
	/// <param name="_satisfactionChipFlag"></param>
	/// <returns></returns>
	public static bool SetSatisfactionChipFlag(bool _satisfactionChipFlag) => satisfactionChipFlag = _satisfactionChipFlag;

	/// <summary>
	/// 満足フラグ(チップ取得時用)の取得
	/// </summary>
	/// <returns></returns>
	public static bool GetSatisfactionChipFlag() => satisfactionChipFlag;
}
