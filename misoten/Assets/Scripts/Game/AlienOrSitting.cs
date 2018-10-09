using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがその席に座っているかの判定スクリプト
/// </summary>
public class AlienOrSitting : MonoBehaviour
{
	// エイリアンの席移動
	private AlienMove alienMove;

	// 座っているかの判定
	private static bool[] orSitting = new bool[5];

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		alienMove = GameObject.Find("Alien_1").GetComponent<AlienMove>();

		// 初期化(全ての席が座られていない状態)
		for (int i = 0; i < 5 ; i++) { SetOrSitting(false, i); }
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void  Update ()
	{
		// IDを取得して、その席が座られている状態にする
		if (!GetOrSitting(alienMove.GetEndPositionsID()))
		{
			SetOrSitting(true, alienMove.GetEndPositionsID());
		}

		if (Input.GetKey(KeyCode.A))
		{
			Debug.Log(GetOrSitting(0));
			Debug.Log(GetOrSitting(1));
			Debug.Log(GetOrSitting(2));
			Debug.Log(GetOrSitting(3));
			Debug.Log(GetOrSitting(4));
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