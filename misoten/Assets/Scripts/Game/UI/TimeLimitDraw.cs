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

	// エイリアンの邪魔行動スクリプト変数
	AlienDisturbance alienDisturbance;

	// ゲーム制限時間スクリプト変数
	GameTimeManager gameTimeManager;

	// テキストフラグ
	private bool textFlag = true;

	// 残存時間の可視化の為
	private int timeFont = 0;

	// カラー保存用
	private Color color;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		alienDisturbance = gameObject.transform.parent.gameObject.GetComponent<AlienDisturbance>();

		gameTimeManager = GameObject.Find("TimeManager").gameObject.GetComponent<GameTimeManager>();

		// 残存時間の位置指定
		transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z - 0.4f);

		// カラーの保存
		color = GetComponent<TextMesh>().color;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		if(gameTimeManager.GetCountTime() > 0)
		{
			// 残存時間可視化
			GetComponent<TextMesh>().text = alienDisturbance.GetTimeFont().ToString("00");

			// 10秒を切ると、文字が黄色くなる
			if (alienDisturbance.GetTimeFont() <= 10.0f)
			{
				color.r = color.g = 255.0f;
			}
			// 5秒を切ると、文字が赤くなる
			if (alienDisturbance.GetTimeFont() <= 5.0f)
			{
				color.r = 255.0f;
				color.g = 0.0f;
			}

			// カラー更新
			GetComponent<TextMesh>().color = color;
		}
		else { GetComponent<TextMesh>().text = ""; }
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