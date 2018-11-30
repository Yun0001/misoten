using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UFO移動スクリプト
/// </summary>
public class UfoMove : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// UFOの速度
	[SerializeField]
	private float spd;

	// 幅
	[SerializeField]
	private float width;

	// 高さ
	[SerializeField]
	private float height;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 位置更新用
	private Vector3 pos;

	// ---------------------------------------------

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		pos.x = Mathf.Cos(Time.time + spd) * width;
		pos.y = Mathf.Sin(Time.time + spd) * height;

		transform.position = pos;
	}
}
