using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ゲージのスクリプト
/// </summary>
public class Gage : MonoBehaviour
{
	[SerializeField] private Image objUI;		// イメージオブジェクト設定用
	[SerializeField] private bool roop;			// ゲージ処理がループするか
	[SerializeField] private float countTime;	// カウント時間を値で管理

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 処理内容無し
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// trueなら永久ループ
		if (roop)
		{
			// 設定した値によってゲージの進む速度が変わる処理
			objUI.fillAmount -= 1.0f / -countTime * Time.deltaTime;
		}
	}
}