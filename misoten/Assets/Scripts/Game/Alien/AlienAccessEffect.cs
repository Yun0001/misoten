using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーからエイリアンにアクセス可能時に出るエフェクト制御スクリプト
/// </summary>
public class AlienAccessEffect : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// プレイヤーの設定
	[SerializeField]
	private GameObject[] playerObj = new GameObject[4];

	// アクセスエリアの設定
	[SerializeField]
	private GameObject[] dishObj = new GameObject[7];

	// アクセスエフェクトの設定
	[SerializeField]
	private ParticleSystem prefab;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// アクセス用のオブジェクト設定用
	private ParticleSystem[] accessObj = new ParticleSystem[7];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 席分ループ
		for(int i = 0; i < 7 ;i++)
		{
			// エフェクト生成
			accessObj[i] = Instantiate(prefab, dishObj[i].transform.position, Quaternion.identity) as ParticleSystem;
			accessObj[i].transform.SetParent(transform);
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		if (playerObj[0].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			|| playerObj[1].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			|| playerObj[2].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			|| playerObj[3].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien))
		{
			for(int i = 0; i < dishObj.Length; i++)
			{
				if (HitDish.hitDish[i])
				{
					accessObj[i].GetComponent<ParticleSystem>().Play();
				}
				else { accessObj[i].GetComponent<ParticleSystem>().Stop(); }
			}
		}

		if (!playerObj[0].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			&& !playerObj[1].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			&& !playerObj[2].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien)
			&& !playerObj[3].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien))
		{
			for (int i = 0; i < 7; i++)
			{
				accessObj[i].GetComponent<ParticleSystem>().Stop();
			}
		}
	}
}