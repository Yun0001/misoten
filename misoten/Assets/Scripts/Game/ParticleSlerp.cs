using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲線移動するスクリプト
/// </summary>
public class ParticleSlerp : MonoBehaviour
{
	// ターゲット設定
	[SerializeField]
	private GameObject target;

	// 速度設定
	[SerializeField]
	private float spd;

	private Vector3 savePos;

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
		var vec = Vector3.Slerp(transform.position, target.transform.position, Time.deltaTime * spd);
		transform.LookAt(vec);
		transform.position = vec;

		if (transform.position == target.transform.position) { GetComponent<ParticleSystem>().Stop(); }
	}
}
