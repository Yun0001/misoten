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
		ICEBOX,			// 冷蔵庫
		MAX				// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 調理方法
	[SerializeField]
	private EOrderType orderType;

	// 冷蔵庫の種類
	[SerializeField]
	private bool iceBoxPattern;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 焼き調理を行っているかの判定用に使う
	private CookWareAnimCtrl[] cookWareAnimCtrl = new CookWareAnimCtrl[2];
	private mwAnimCtrl mwAnimCtrl;

	// エフェクトフラグ
	private bool[] effectFlag = { false, false, false, false };

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
			case EOrderType.MICROWAVE:
				// コンポーネント取得
				mwAnimCtrl = GameObject.Find("Stage/cookwares/microwaves/microwave1/microwave").gameObject.GetComponent<mwAnimCtrl>();
				break;
			case EOrderType.ICEBOX:
				// コンポーネント取得
				if (iceBoxPattern) { mwAnimCtrl = GameObject.Find("Stage/cookwares/iceboxes/icebox1/icebox 1").gameObject.GetComponent<mwAnimCtrl>(); }
				else { mwAnimCtrl = GameObject.Find("Stage/cookwares/iceboxes/icebox2/icebox 1").gameObject.GetComponent<mwAnimCtrl>(); }
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
			case EOrderType.MICROWAVE:
				if (mwAnimCtrl.GetBool() && !effectFlag[2]) { GetComponent<ParticleSystem>().Play(); effectFlag[2] = !effectFlag[2]; }
				if (!mwAnimCtrl.GetBool() && effectFlag[2]) { GetComponent<ParticleSystem>().Stop(); effectFlag[2] = !effectFlag[2]; }
				break;
			case EOrderType.ICEBOX:
				if (mwAnimCtrl.GetIsOpen() && !effectFlag[3]) { GetComponent<ParticleSystem>().Play(); effectFlag[3] = !effectFlag[3]; }
				if (!mwAnimCtrl.GetIsOpen() && effectFlag[3]) { GetComponent<ParticleSystem>().Stop(); effectFlag[3] = !effectFlag[3]; }
				break;
			default: break;
		}
	}
}
