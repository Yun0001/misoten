using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テキスト色のスクリプト
/// </summary>
public class TextColor : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// テキスト色更新時間
	[SerializeField]
	private float time;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 時間更新
	private float timeAdd = 0.0f;

	// カラー更新用
	private Color col;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 時間更新の初期化
		timeAdd = 0.0f;

		// 色の保存
		col = GetComponent<TextMesh>().color;
		col.a = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if (col.a < 1.0f)
		{
			if (timeAdd >= time)
			{
				// アルファ値更新
				col.a += 0.1f;
				GetComponent<TextMesh>().color = col;

				// 時間更新の初期化
				timeAdd = 0.0f;
			}
			else { timeAdd += Time.deltaTime; }
		}
	}
}
