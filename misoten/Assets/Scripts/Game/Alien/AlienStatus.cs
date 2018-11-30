using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// エイリアンの状態管理スクリプト
/// </summary>
public class AlienStatus : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンの状態管理
	/// 補足：ごめん、ここだけpublicにさせて...
	/// </summary>
	public enum EStatus
	{
		STAND = 0,      // 待機
		VISIT,          // 訪問時
		WALK_SIDE,      // 画面外に向かって歩いている
		OUT_SCREEN,     // 画面外に消えた
		WALK_IN,        // 客席に向かって歩いている
		GETON,          // 着席
		ORDER,          // 注文
		STAY,           // 待つ(注文待機)
		CLAIM,          // クレーム
		SATISFACTION,	// 満足
		EAT,            // 食べる
		RETURN_GOOD,    // 帰る(良)
		RETURN_BAD,     // 帰る(悪)
		SCREEN_OUT,     // 画面外
		MAX             // 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定席に座るエイリアンのID(Debug用)
	[SerializeField, Range(0, 6)]
	private int debugSeatId;

	// エイリアンの状態を目視(Debug用)
	[SerializeField]
	private EStatus debugStatusId;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 状態遷移フラグ
	private static bool[,] counterStatusChangeFlag = new bool[7, (int)EStatus.MAX];

	// ---------------------------------------------

	/// <summary>
	/// ステータス初期化関数
	/// </summary>
	public void StatusInit(int id)
	{
		// 全ての状態を「OFF」にする
		for (int i = 0; i < (int)EStatus.MAX; i++) { counterStatusChangeFlag[id, i] = false; }

		// 待機状態にする
		counterStatusChangeFlag[id, (int)EStatus.STAND] = true;
	}

	/// <summary>
	/// デバッグ用関数
	/// </summary>
	public void DebugText(int id)
	{
		switch (debugStatusId)
		{
			case EStatus.STAND: Debug.Log("エイリアン「" + id + "」" + "の待機状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.STAND)); break;
			case EStatus.VISIT: Debug.Log("エイリアン「" + id + "」" + "の訪問時状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.VISIT)); break;
			case EStatus.WALK_SIDE: Debug.Log("エイリアン「" + id + "」" + "の画面外に向かって歩いている状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.WALK_SIDE)); break;
			case EStatus.OUT_SCREEN: Debug.Log("エイリアン「" + id + "」" + "の画面外に消えた状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.OUT_SCREEN)); break;
			case EStatus.WALK_IN: Debug.Log("エイリアン「" + id + "」" + "の客席に向かって歩いている状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.WALK_IN)); break;
			case EStatus.GETON: Debug.Log("エイリアン「" + id + "」" + "の着席状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.GETON)); break;
			case EStatus.ORDER: Debug.Log("エイリアン「" + id + "」" + "の注文状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.ORDER)); break;
			case EStatus.STAY: Debug.Log("エイリアン「" + id + "」" + "の待つ(注文待機)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.STAY)); break;
			case EStatus.CLAIM: Debug.Log("エイリアン「" + id + "」" + "のクレーム状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.CLAIM)); break;
			case EStatus.SATISFACTION: Debug.Log("エイリアン「" + id + "」" + "の満足状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.SATISFACTION)); break;
			case EStatus.EAT: Debug.Log("エイリアン「" + id + "」" + "の食べる状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.EAT)); break;
			case EStatus.RETURN_GOOD: Debug.Log("エイリアン「" + id + "」" + "の帰る(良)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.RETURN_GOOD)); break;
			case EStatus.RETURN_BAD: Debug.Log("エイリアン「" + id + "」" + "の帰る(悪)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.RETURN_BAD)); break;
			case EStatus.SCREEN_OUT: Debug.Log("エイリアン「" + id + "」" + "の画面外状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.SCREEN_OUT)); break;
			default: break;
		}
	}

	/// <summary>
	/// 状態管理の格納
	/// </summary>
	/// <param name="_status"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetCounterStatusChangeFlag(bool _statusChangeFlag, int numId, int statusId) => counterStatusChangeFlag[numId, statusId] = _statusChangeFlag;

	/// <summary>
	/// 状態管理の取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetCounterStatusChangeFlag(int numId, int statusId) => counterStatusChangeFlag[numId, statusId];

}
