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
		ICEBOX1,		// 冷蔵庫1
		ICEBOX2,		// 冷蔵庫2
		MIXER,			// ミキサー
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

	// 各調理のアニメーション判定用
	private CookWareAnimCtrl[] cookWareAnimCtrl = new CookWareAnimCtrl[2];
	private mwAnimCtrl[] mwAnimCtrl = new mwAnimCtrl[3];
	private iceboxAnimCtrl[] ibAnimCtrl = new iceboxAnimCtrl[2];
	private mixerAnimCtrl mixerAnimCtrl;

	// エフェクトフラグ
	private bool effectFlag = false;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		switch (orderType)
		{
			case EOrderType.GRILLED: cookWareAnimCtrl[0] = GameObject.Find("Stage/cookwares/pans/pan1/pan").gameObject.GetComponent<CookWareAnimCtrl>(); break;
			case EOrderType.SIMMER: cookWareAnimCtrl[1] = GameObject.Find("Stage/cookwares/nabes/nabe1/nabe").gameObject.GetComponent<CookWareAnimCtrl>(); break;
			case EOrderType.MICROWAVE: mwAnimCtrl[0] = GameObject.Find("Stage/cookwares/microwaves/microwave1/microwave").gameObject.GetComponent<mwAnimCtrl>(); break;
			case EOrderType.ICEBOX1: ibAnimCtrl[0] = GameObject.Find("Stage/cookwares/iceboxes/icebox1/icebox").gameObject.GetComponent<iceboxAnimCtrl>(); break;
			case EOrderType.ICEBOX2: ibAnimCtrl[1] = GameObject.Find("Stage/cookwares/iceboxes/icebox2/icebox").gameObject.GetComponent<iceboxAnimCtrl>(); break;
			case EOrderType.MIXER: mixerAnimCtrl = GameObject.Find("Stage/cookwares/mixer/mixer").gameObject.GetComponent<mixerAnimCtrl>(); break;
			default: break;
		}

		// エフェクトフラグの初期化
		effectFlag = false;

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
				if (cookWareAnimCtrl[0].GetBool() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!cookWareAnimCtrl[0].GetBool() && effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			case EOrderType.SIMMER:
				if (cookWareAnimCtrl[1].GetBool() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!cookWareAnimCtrl[1].GetBool() && effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			case EOrderType.MICROWAVE:
				if (mwAnimCtrl[0].GetBool() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!mwAnimCtrl[0].GetBool() && effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			case EOrderType.ICEBOX1:
				if (ibAnimCtrl[0].GetIsOpen() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!ibAnimCtrl[0].GetIsOpen() && effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			case EOrderType.ICEBOX2:
				if (ibAnimCtrl[1].GetIsOpen() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!ibAnimCtrl[1].GetIsOpen() &&  effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			case EOrderType.MIXER:
				if (mixerAnimCtrl.GetBool() && !effectFlag) { GetComponent<ParticleSystem>().Play(); effectFlag = !effectFlag; }
				if (!mixerAnimCtrl.GetBool() && effectFlag) { GetComponent<ParticleSystem>().Stop(); effectFlag = !effectFlag; }
				break;
			default: break;
		}
	}
}
