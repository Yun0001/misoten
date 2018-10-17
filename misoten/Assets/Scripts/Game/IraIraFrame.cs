using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// イライラフレームの制御スクリプト
/// </summary>
public class IraIraFrame : MonoBehaviour
{
	// 指定されたオブジェクトの位置を取得
	[SerializeField]
	private GameObject targetObj;

	// 移動速度
	[SerializeField, Range(-100, 100.0f)]
	private float speed;

	// 移動範囲(幅、高さ、奥行き)
	[SerializeField]
	private Vector3 moveRange;

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
		//// 指定したオブジェクトが円運動(X,Y軸)に行う
		//transform.position = targetObj.transform.position + new Vector3(
		//	Mathf.Cos(Time.time * speed) * moveRange.x,
		//	Mathf.Sin(Time.time * speed) * moveRange.y, 0.0f);

		//var aim = this.targetObj.transform.position - this.transform.position;
		//var look = Quaternion.LookRotation(aim);
		//transform.rotation = new Quaternion(0.0f, 0.0f, look.z, look.w);

		transform.RotateAround(targetObj.transform.position, new Vector3(0.0f, 0.0f, -1.0f), 30.0f * Time.deltaTime);
	}
}