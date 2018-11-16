using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メニュー移動スクリプト
/// </summary>
public class MenuMove : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定終点座標
	[SerializeField]
	private Vector3 endPos;

	// 移動時間
	[SerializeField]
	private float moveTime;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// リザルト専用のエイリアン描画用フラグ
	private static bool resultAlienFlag = false;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 終点座標フラグ
	private bool setEndPositionFlag = true;

	// 時間更新用
	private float timeAdd = 0.0f;

	// 予定時間を割る用
	private float rate = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// リザルト専用のエイリアン描画用フラグの初期化
		resultAlienFlag = false;

		// 終点座標IDの初期化
		setEndPositionFlag = true;

		// 時間更新用、予定時間を割る用の初期化
		timeAdd = rate = 0.0f;
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 終点座標に到着していない時
		if(setEndPositionFlag)
		{
			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / moveTime;

			// 終点座標に到着
			if (timeAdd > moveTime)
			{
				// この処理に行かないようにする
				setEndPositionFlag = false;

				// 移動時間の初期化
				timeAdd = 0.0f;

				// リザルト専用のエイリアン描画「ON」
				resultAlienFlag = true;
			}

			// 終点座標への移動処理
			transform.position = Vector3.Lerp(new Vector3(-6.5f, 19.5f, 0.0f), endPos, rate);
		}
	}

	/// <summary>
	/// リザルト専用のエイリアン描画用フラグの取得
	/// </summary>
	/// <returns></returns>
	public static bool GetResultAlienFlag() => resultAlienFlag;
}
