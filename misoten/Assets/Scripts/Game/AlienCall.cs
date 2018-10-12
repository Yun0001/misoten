using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの呼び出しスクリプト
/// </summary>
public class AlienCall : MonoBehaviour
{
	// 呼び出し時間指定
	[SerializeField]
	private float callTime;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private GameObject obj;

	// 待ち時間の加算
	private float latencyAdd = 0.0f;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{

	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 毎フレームの時間を加算
		latencyAdd += Time.deltaTime;

		if (latencyAdd > callTime)
		{
			obj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;

			obj.transform.SetParent(transform);

			latencyAdd = 0.0f;
		}
	}
}
