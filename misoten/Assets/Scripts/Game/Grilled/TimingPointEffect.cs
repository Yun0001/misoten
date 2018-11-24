using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 焼き調理のノーツをタイミングよく押せた時
/// 種火から大きい炎に変わるスクリプト
/// </summary>
public class TimingPointEffect : MonoBehaviour
{

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start () {
		
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		for(int i = 0; i < 4 ;i++)
		{
			if (GetComponent<TimingPoint>().IsHit(i))
			{
				GetComponent<ParticleSystem>().startSize = 0.8f;
				break;
			}
			else
			{
				GetComponent<ParticleSystem>().startSize = 0.3f;
			}
		}
	}
}
