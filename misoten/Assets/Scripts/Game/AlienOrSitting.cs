using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがその席に座っているかの判定スクリプト
/// </summary>
public class AlienOrSitting : MonoBehaviour
{
	// 座っているかの判定
	private static bool[] orSitting = { false, false , false , false , false };

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 初期化(全ての席が座られていない状態)
		for (int i = 0; i < 5; i++) { SetOrSitting(false, i); }
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		GetComponent<AlienCall>().AlienForm();

		// 席が空いている場合
		if (!AlienCall.GetSeat(AlienCall.GetAddId()))
		{
			// IDを取得して、その席が座られている状態にする
			//if (!GetOrSitting(AlienCall.GetSeat()))
			//{
			//	SetOrSitting(true, AlienCall.GetSeat());
			//}
			SetOrSitting(true, AlienCall.GetAddId());
		}
	}

	/// <summary>
	/// 席の状態を格納
	/// </summary>
	/// <param name="_is"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetOrSitting(bool _is, int id) => orSitting[id] = _is;

	/// <summary>
	/// 席の状態を取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetOrSitting(int id) => orSitting[id];
}