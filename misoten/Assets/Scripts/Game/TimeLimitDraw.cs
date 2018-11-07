using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキスト描画スクリプト
/// </summary>
public class TimeLimitDraw : MonoBehaviour
{
	// ローカル変数
	// ---------------------------------------------

	// テキストフラグ
	private bool textFlag = true;

	// 残存時間の可視化の為
	private int timeFont = 0;

	// エイリアンの邪魔行動スクリプト変数
	AlienDisturbance alienDisturbance;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		alienDisturbance = gameObject.transform.parent.gameObject.GetComponent<AlienDisturbance>();

		// 残存時間の位置指定
		transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z - 0.1f);
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 残存時間可視化
		GetComponent<TextMesh>().text = alienDisturbance.GetTimeFont().ToString("00");
	}

	/// <summary>
	/// テキストフラグの格納
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public bool SetTextFlag(bool flag) => textFlag = flag;

	/// <summary>
	/// テキストフラグの取得
	/// </summary>
	/// <returns></returns>
	public bool GetTextFlag() => textFlag;

	/// <summary>
	/// 残存時間可視化の格納
	/// </summary>
	/// <param name="_timeFont"></param>
	/// <returns></returns>
	public int SetTimeFont(int _timeFont) => timeFont = _timeFont;

	/// <summary>
	/// 残存時間可視化の取得
	/// </summary>
	/// <returns></returns>
	public int GetTimeFont() => timeFont;
}