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
	private GameObject[] dishObj = new GameObject[6];

	// アクセスエフェクトの設定
	[SerializeField]
	private ParticleSystem prefab;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// アクセス用のオブジェクト設定用
	private ParticleSystem[] accessObj = new ParticleSystem[6];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 席分ループ
		for(int i = 0; i < 6 ;i++)
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
		for (int i = 0; i < dishObj.Length; i++)
		{
			if (HitDish.hitDish[i])
			{
				accessObj[i].GetComponent<ParticleSystem>().Play();
				for (int j = 0; j < playerObj.Length; j++)
				{
					if (playerObj[j].GetComponent<PlayerCollision>().GetHitObj(PlayerCollision.hitObjName.Alien))
					{
						HitDish.hitDish[i] = false;
					}
				}
			}
			else { accessObj[i].GetComponent<ParticleSystem>().Stop(); }
		}
	}
}