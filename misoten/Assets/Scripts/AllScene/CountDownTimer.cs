using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// カウントダウンのスクリプト
/// </summary>
public class CountDownTimer : MonoBehaviour
{
	// 定数、一分
	const int MINUTE = 60;

	private float m_oldSeconds;					// 前回Update時の秒数
	private float m_totalTime;					// トータル制限時間
	private Text m_timerText;					// 制限時間のテキスト
	[SerializeField] private int m_minute;		// 制限時間(分)
	[SerializeField] private float m_seconds;	// 制限時間(秒)

	/// <summary>
	/// 開始関数
	/// </summary>
	private void Start()
	{
		// 前回Update時の秒数の初期化 
		m_oldSeconds = 0.0f;

		// 設定された時間(分)と時間(秒)を加算して保存
		m_totalTime = (m_minute * MINUTE) + m_seconds;

		// テキストのコンポーネント取得
		m_timerText = GetComponentInChildren<Text>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	private void Update()
	{
		// 制限時間が0秒以下なら何もしない
		if (m_totalTime <= 0.0f) { return; }
		else
		{
			// 一旦トータルの制限時間を計測
			m_totalTime = m_minute * MINUTE + m_seconds;
			m_totalTime -= Time.deltaTime;

			// 再設定
			m_minute = (int)m_totalTime / MINUTE;
			m_seconds = m_totalTime - m_minute * MINUTE;

			// タイマー表示用UIテキストに時間を表示する
			if ((int)m_seconds != (int)m_oldSeconds)
			{
				m_timerText.text = m_minute.ToString("00") + ":" + ((int)m_seconds).ToString("00");
			}

			// 前回の秒数の更新
			m_oldSeconds = m_seconds;

			// 制限時間以下になったらコンソールに『制限時間終了』という文字列を表示する
			if (m_totalTime <= 0.0f) { Debug.Log("制限時間終了"); }
		}
	}
}
