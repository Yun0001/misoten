using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		WALK,           // 入店時
		GETON,          // 着席
		ORDER,          // 注文
		STAY,           // 待つ(注文待機)
		CLAIM,          // クレーム
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
	[SerializeField, Range(0, 7)]
	private int debugSeatId;

	// エイリアンの状態を目視(Debug用)
	[SerializeField]
	private EStatus debugStatusId;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 状態遷移(カウンター用)フラグ
	private static bool[,] counterStatusChangeFlag = new bool[7, (int)EStatus.MAX];

	// 状態遷移(持ち帰り用)フラグ
	private static bool[,] takeOutStatusChangeFlag = new bool[6, (int)EStatus.MAX];

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// ステータスを初期化
		for (int i = 0; i < GetComponent<AlienCall>().GetCounterSeatsMax(); i++)
		{
			// 全ての状態を「OFF」にする
			for (int j = 0; j < (int)EStatus.MAX; j++) { counterStatusChangeFlag[i, j] = false; }

			// 待機状態にする
			counterStatusChangeFlag[i, (int)EStatus.STAND] = true;
		}

		// ステータスを初期化
		for (int i = 0; i < GetComponent<AlienCall>().GetTakeAwaySeatMax(); i++)
		{
			// 全ての状態を「OFF」にする
			for (int j = 0; j < (int)EStatus.MAX; j++) { takeOutStatusChangeFlag[i, j] = false; }

			// 待機状態にする
			takeOutStatusChangeFlag[i, (int)EStatus.STAND] = true;
		}
	}

	/// <summary>
	/// デバッグ用関数
	/// </summary>
	public void DebugText(int id)
	{
		// エイリアンが座る席のパターン管理
		switch (alienCall.GetSeatPattern())
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				switch (debugStatusId)
				{
					case EStatus.STAND: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の待機状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.STAND)); break;
					case EStatus.WALK: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の入店時状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.WALK)); break;
					case EStatus.GETON: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の着席状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.GETON)); break;
					case EStatus.ORDER: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の注文状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.ORDER)); break;
					case EStatus.STAY: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の待つ(注文待機)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.STAY)); break;
					case EStatus.CLAIM: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "のクレーム状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.CLAIM)); break;
					case EStatus.EAT: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の食べる状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.EAT)); break;
					case EStatus.RETURN_GOOD: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の帰る(良)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.RETURN_GOOD)); break;
					case EStatus.RETURN_BAD: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の帰る(悪)状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.RETURN_BAD)); break;
					case EStatus.SCREEN_OUT: Debug.Log("エイリアン(カウンター側)「" + id + "」" + "の画面外状態は" + GetCounterStatusChangeFlag(id, (int)EStatus.SCREEN_OUT)); break;
					default: break;
				}
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				switch (debugStatusId)
				{
					case EStatus.STAND: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の待機状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.STAND)); break;
					case EStatus.WALK: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の入店時状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.WALK)); break;
					case EStatus.GETON: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の着席状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.GETON)); break;
					case EStatus.ORDER: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の注文状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.ORDER)); break;
					case EStatus.STAY: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の待つ(注文待機)状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.STAY)); break;
					case EStatus.CLAIM: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "のクレーム状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.CLAIM)); break;
					case EStatus.EAT: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の食べる状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.EAT)); break;
					case EStatus.RETURN_GOOD: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の帰る(良)状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.RETURN_GOOD)); break;
					case EStatus.RETURN_BAD: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の帰る(悪)状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.RETURN_BAD)); break;
					case EStatus.SCREEN_OUT: Debug.Log("エイリアン(持ち帰り側)「" + id + "」" + "の画面外状態は" + GetTakeOutStatusChangeFlag(id, (int)EStatus.SCREEN_OUT)); break;
					default: break;
				}
				break;
			default: break;
		}
	}

	/// <summary>
	/// 状態管理(カウンター用)の格納
	/// </summary>
	/// <param name="_status"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetCounterStatusChangeFlag(bool _statusChangeFlag, int numId, int statusId) => counterStatusChangeFlag[numId, statusId] = _statusChangeFlag;

	/// <summary>
	/// 状態管理(カウンター用)の取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetCounterStatusChangeFlag(int numId, int statusId) => counterStatusChangeFlag[numId, statusId];

	/// <summary>
	/// 状態管理(持ち帰り用)の格納
	/// </summary>
	/// <param name="_status"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetTakeOutStatusChangeFlag(bool _statusChangeFlag, int numId, int statusId) => takeOutStatusChangeFlag[numId, statusId] = _statusChangeFlag;

	/// <summary>
	/// 状態管理(持ち帰り用)の取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetTakeOutStatusChangeFlag(int numId, int statusId) => takeOutStatusChangeFlag[numId, statusId];
}
