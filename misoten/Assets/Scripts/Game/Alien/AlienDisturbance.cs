﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンが厨房に向けて邪魔な行為をするスクリプト
/// </summary>
public class AlienDisturbance : MonoBehaviour
{
	/// <summary>
	/// エイリアンの機嫌
	/// </summary>
	private enum EAlienMood
	{
		NORMAL = 0, // 通常状態
		ANGER,		// 怒り状態
		FAVORABLE,	// 良好状態
		MAX			// 最大
	}

	/// <summary>
	/// エイリアンの種類設定
	/// </summary>
	private enum EAlienPattern
	{
		MARTIAN,    // 火星人
		MERCURY,    // 水星人
		VENUSIAN,   // 金星人
		MAX         // 最大
	}

	// Prefabを指定して、そのPrefabを生成する為の物
	[SerializeField]
	GameObject prefab;

	// エイリアンの機嫌が変わるまでの時間を設定
	[SerializeField]
	private float moodChangeTime;

	// エイリアンのオーダー
	private AlienOrder alienOrder;

	// エイリアンのチップ
	private AlienChip alienChip;

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// 種類のID
	private int patternId = 0;

	// 待ち時間の加算
	private float latencyAdd = 0.0f;

	// エイリアンの機嫌
	private EAlienMood mood = EAlienMood.NORMAL;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();
		alienChip = GetComponent<AlienChip>();

		alienCall = GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// エイリアンが注文している時
		if (alienOrder.GetIsOrder())
		{
			// エイリアンの機嫌管理
			switch (mood)
			{
				case EAlienMood.NORMAL: // 通常状態

					// 料理が来ていない時
					if (!alienChip.GetCuisineCame())
					{
						// 毎フレームの時間を加算
						latencyAdd += Time.deltaTime;
					}
					break;
				case EAlienMood.ANGER:  // 怒り状態
					// エイリアンの種類管理
					switch (alienCall.GetAlienPattern(patternId))
					{
						case 0: // 火星人の場合

							// 火星人特有の邪魔行為
							//MartianDisturbance();
							break;
						case 1: // 水星人の場合

							// 水星人特有の邪魔行為
							MercuryDisturbance();
							break;
						case 2:// 金星人の場合

							// 金星人特有の邪魔行為
							VenusianDisturbance();
							break;
						default:
							// 例外処理
							break;
					}

					// エイリアンの種類IDをチェック
					if (patternId < 4) { patternId++; }
					else { patternId = 0; }

					break;
				case EAlienMood.FAVORABLE:  // 良好状態

					break;
				default:
					// 例外処理
					break;
			}

			// 注文して指定時間以上立つと怒り状態になる
			if (latencyAdd >= alienCall.GetOrderLatencyAdd(patternId)) { mood = EAlienMood.ANGER; }

			// Debug用
			//Debug.Log("オーダー待ち時間" + alienCall.GetOrderLatencyAdd(patternId));
		}
	}

	/// <summary>
	/// 火星人の邪魔行為
	/// </summary>
	void MartianDisturbance()
	{
		// Debug用
		//Debug.Log("火星人の邪魔行動");

		// 5個ランダムな場所にcubeを生成する
		//for (int i = 0; i < 5; i++)
		//{
		//	// Instantiateの引数にPrefabを渡すことでインスタンスを生成する
		//	GameObject ball = Instantiate(prefab) as GameObject;

		//	// ランダムな場所に配置する
		//	ball.transform.position = new Vector3(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f), 0.0f);
		//}
	}

	/// <summary>
	/// 水星人の邪魔行為
	/// </summary>
	void MercuryDisturbance()
	{
		// Debug用
		//Debug.Log("水星人の邪魔行動");
	}

	/// <summary>
	/// 金星人の邪魔行為
	/// </summary>
	void VenusianDisturbance()
	{
		// Debug用
		//Debug.Log("金星人の邪魔行動");
	}
}
