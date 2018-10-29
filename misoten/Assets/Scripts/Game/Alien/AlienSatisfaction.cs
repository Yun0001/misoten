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
			// 満足した吹き出しを出す
			satisfactionBalloon.SetActive(true);

			// 満足時間が指定時間を超えた場合
			if (satisfactionTimeAdd >= satisfactionTime)
			{
				satisfactionTimeAdd = 0.0f;

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
}
