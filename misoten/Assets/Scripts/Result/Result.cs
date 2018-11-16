using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルトエイリアンの描画用スクリプト
/// </summary>
public class Result : MonoBehaviour
{
	// 列挙型
	// ---------------------------------------------

	/// <summary>
	/// オブジェクト種類
	/// </summary>
	private enum EObjectType
	{
		USERSCOREBOARD = 0,			// ユーザースコアボード
		DEVELOPMENTTEAMSCOREBOARD,	// 開発陣スコアボード
		ALIEN,						// リザルト用エイリアン
		HAPPYEND,					// ユーザースコアが開発陣スコアを上回った時用
		BADEND,						// ユーザースコアが開発陣スコアを下回った時用
		MAX							// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// オブジェクト取得用
	[SerializeField]
	private GameObject[] obj = new GameObject[(int)EObjectType.MAX];

	// オブジェクト描画開始時間
	[SerializeField]
	private float[] time = new float[(int)EObjectType.MAX];

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------


	// オブジェクト描画用フラグ
	private bool[] objDrawflag = new bool[(int)EObjectType.MAX];

	// 時間更新
	private float[] timeAdd = new float[(int)EObjectType.MAX];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 時間更新、オブジェクト描画用フラグの初期化
		for (int i = 0; i < (int)EObjectType.MAX; i++) { obj[0].SetActive(false); objDrawflag[i] = true; timeAdd[i] = 0.0f; }
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// メニューの移動が終了すると、オブジェクトをアクティブ化
		if (MenuMove.GetResultAlienFlag())
		{
			if (objDrawflag[0])
			{
				if (timeAdd[0] >= time[0]) { obj[0].SetActive(true); objDrawflag[0] = !objDrawflag[0]; }
				else { timeAdd[0] += Time.deltaTime; }
			}

			if (objDrawflag[1])
			{
				if (obj[0].activeSelf)
				{
					if (timeAdd[1] >= time[1]) { obj[1].SetActive(true); objDrawflag[1] = !objDrawflag[1]; }
					else { timeAdd[1] += Time.deltaTime; }
				}
			}

			if (objDrawflag[2])
			{
				if (obj[1].activeSelf)
				{
					if (timeAdd[2] >= time[2]) { obj[2].SetActive(true); objDrawflag[2] = !objDrawflag[2]; }
					else { timeAdd[2] += Time.deltaTime; }
				}
			}

			if (objDrawflag[3])
			{
				if(ScoreCount.GetScore() > 8500)
				{
					if (obj[2].activeSelf)
					{
						if (timeAdd[3] >= time[3]) { obj[3].SetActive(true); objDrawflag[3] = !objDrawflag[3]; }
						else { timeAdd[3] += Time.deltaTime; }
					}
				}
				else
				{
					if (obj[2].activeSelf)
					{
						if (timeAdd[4] >= time[4]) { obj[4].SetActive(true); objDrawflag[4] = !objDrawflag[4]; }
						else { timeAdd[4] += Time.deltaTime; }
					}
				}
			}
		}
	}
}
