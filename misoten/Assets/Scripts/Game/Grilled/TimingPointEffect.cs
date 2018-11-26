using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 焼き調理のノーツをタイミングよく押せた時
/// 種火から大きい炎に変わるスクリプト
/// </summary>
public class TimingPointEffect : MonoBehaviour
{
	// 時間更新
	private float timeAdd = 0.0f;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 時間更新の初期化
		timeAdd = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if (Flyingpan.effectFlag)
		{
			GetComponent<ParticleSystem>().startSize = 0.8f;
			if (timeAdd >= 0.8f)
			{
				timeAdd = 0.0f;
				Flyingpan.effectFlag = false;
			}
			else { timeAdd += Time.deltaTime; }
		}
		else
		{
			GetComponent<ParticleSystem>().startSize = 0.3f;
		}
	}
}
