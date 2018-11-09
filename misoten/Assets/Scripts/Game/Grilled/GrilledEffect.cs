using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 焼き調理時のエフェクト呼び出し
/// </summary>
public class GrilledEffect : MonoBehaviour
{
	// 調理方法管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// 調理方法の種類
	/// </summary>
	private enum EOrderType
	{
		GRILLED = 0,	// 焼き
		SIMMER,			// 煮る
		MICROWAVE,		// 電子レンジ
		MAX				// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 調理方法
	[SerializeField]
	private EOrderType orderType;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 焼き調理を行っているかの判定用に使う
	private CookWareAnimCtrl[] cookWareAnimCtrl = new CookWareAnimCtrl[2];

	// エフェクトフラグ
	private bool[] effectFlag = { false, false };

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		switch(orderType)
		{
			case EOrderType.GRILLED:
				// コンポーネント取得
				cookWareAnimCtrl[0] = GameObject.Find("Stage/cookwares/pans/pan1/pan").gameObject.GetComponent<CookWareAnimCtrl>();
				break;
			case EOrderType.SIMMER:
				// コンポーネント取得
				cookWareAnimCtrl[1] = GameObject.Find("Stage/cookwares/nabes/nabe1/nabe").gameObject.GetComponent<CookWareAnimCtrl>();
				break;
			default: break;
		}

		// パーティクル停止
		GetComponent<ParticleSystem>().Stop();
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		switch (orderType)
		{
			case EOrderType.GRILLED:
				if (cookWareAnimCtrl[0].GetBool() && !effectFlag[0]) { GetComponent<ParticleSystem>().Play(); effectFlag[0] = !effectFlag[0]; }
				if (!cookWareAnimCtrl[0].GetBool() && effectFlag[0]) { GetComponent<ParticleSystem>().Stop(); effectFlag[0] = !effectFlag[0]; }
				break;
			case EOrderType.SIMMER:
				if (cookWareAnimCtrl[1].GetBool() && !effectFlag[1]) { GetComponent<ParticleSystem>().Play(); effectFlag[1] = !effectFlag[1]; }
				if (!cookWareAnimCtrl[1].GetBool() && effectFlag[1]) { GetComponent<ParticleSystem>().Stop(); effectFlag[1] = !effectFlag[1]; }
				break;
			default: break;
		}
	}
}
