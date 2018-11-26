using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDish : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// プレイヤーの設定
	[SerializeField]
	private GameObject[] playerObj = new GameObject[4];

	// アクセス時の当たり判定ID
	[SerializeField]
	private int id;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// アクセス時の当たり判定フラグ
	static public bool[] hitDish = new bool[7];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// アクセス時の当たり判定フラグの初期化
		for(int i = 0; i < hitDish.Length; i++) { hitDish[i] = false; }
	}

	/// <summary>
	/// 衝突していない時
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerExit(Collider collision)
	{
		hitDish[id] = false;
	}

	/// <summary>
	/// 衝突している間
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerStay(Collider collision)
	{
		// プレイヤーからエイリアンへアクセス可能時
		if (playerObj[0].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
	|| playerObj[1].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
	|| playerObj[2].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
	|| playerObj[3].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien))
		{
			hitDish[id] = true;
		}
	}
}
