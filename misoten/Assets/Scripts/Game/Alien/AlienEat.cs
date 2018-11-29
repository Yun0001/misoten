using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンが食べているアニメーションスクリプト
/// </summary>
public class AlienEat : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	[SerializeField]
	private GameObject[] obj = new GameObject[3];

	// オブジェクト描画開始時間
	[SerializeField]
	private float time;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// オブジェクトID
	private int objId = 0;

	// 時間更新
	private float timeAdd = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// オブジェクトIDの初期化
		objId = 0;

		// 時間更新の初期化
		timeAdd = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 食べる状態の時
		if (AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT))
		{
			if (timeAdd >= time)
			{
				// 指定のオブジェクト描画
				switch (objId)
				{
					case 0: for (int i = 0; i < 3; i++) { obj[i].SetActive(false); } obj[0].SetActive(true); break;
					case 1: for (int i = 0; i < 3; i++) { obj[i].SetActive(false); } obj[1].SetActive(true); break;
					case 2: for (int i = 0; i < 3; i++) { obj[i].SetActive(false); } obj[2].SetActive(true); break;
					default: break;
				}

				// ID更新
				objId++;

				// オブジェクトID初期化
				if (objId >= 3) { objId = 0; }

				// EATアニメーションを行う為に初期化
				timeAdd = 0.0f;
			}
			else { timeAdd += Time.deltaTime; }
		}
		else
		{
			for (int i = 0; i < 3; i++) { obj[i].SetActive(false); }
		}

		// Fadeが開始された時
		if (AlienCall.alienCall.GetCoinFoObj().GetComponent<CoinFO>().GetIsStartingCoinFO())
		{
			for (int i = 0; i < 3; i++) { obj[i].SetActive(false); }
		}
	}
}