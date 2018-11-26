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
			accessObj[i].GetComponent<ParticleSystem>().Stop();
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		for (int i = 0; i < playerObj.Length; i++)
		{
			if (playerObj[i].GetComponent<PlayerAccessController>().IsAccessPossible(PlayerAccessController.AccessObjectName.Alien))
			{
				for (int j = 0; j < dishObj.Length; j++)
				{
					if (HitDish.hitDish[j])
					{
						accessObj[j].GetComponent<ParticleSystem>().Play();
					}
					else { accessObj[j].GetComponent<ParticleSystem>().Stop(); }
				}
			}
			else
			{
				for (int j = 0; j < dishObj.Length; j++)
				{
					if (!HitDish.hitDish[j])
					{
						accessObj[j].GetComponent<ParticleSystem>().Stop();
					}
				}
			}
		}
	}
}