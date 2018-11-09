using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チップ取得時のエフェクト呼び出し
/// </summary>
public class ChipGetEffectCall : MonoBehaviour
{
	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// パーティクル停止
		GetComponent<ParticleSystem>().Stop();
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if(AlienSatisfaction.GetSatisfactionChipFlag())
		{
			// パーティクル再生
			GetComponent<ParticleSystem>().Play();

			// 一度しか通らなくする
			AlienSatisfaction.SetSatisfactionChipFlag(false);
		}
	}
}
