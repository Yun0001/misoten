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
	[SerializeField, Range(0, 4)]
	private int debugSeatId;

	// エイリアンの状態を目視(Debug用)
	[SerializeField]
	private EStatus debugStatusId;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 状態遷移フラグ
	private static bool[,] statusChangeFlag;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 初期化フラグ
	private bool initFlag = false;

	// エイリアン最大数
	private int alienMaxNumber = 0;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 各エイリアンの状態遷移
		statusChangeFlag = new bool[GetComponent<AlienCall>().GetSeatMax(), (int)EStatus.MAX];

		// ステータスを初期化
		for (int i = 0; i < GetComponent<AlienCall>().GetSeatMax(); i++)
		{
			for (int j = 0; j < (int)EStatus.MAX; j++)
			{
				statusChangeFlag[i, j] = false;
			}

			// 待機状態にする
			statusChangeFlag[i, (int)EStatus.STAND] = true;
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// Debug用
		DebugText(debugSeatId);
	}

	/// <summary>
	/// デバッグ用関数
	/// </summary>
	void DebugText(int id)
	{
		switch (debugStatusId)
		{
			case EStatus.STAND: Debug.Log("エイリアン「" + id + "」" + "の待機状態は" + GetStatusFlag(id, (int)EStatus.STAND)); break;
			case EStatus.WALK: Debug.Log("エイリアン「" + id + "」" + "の入店時状態は" + GetStatusFlag(id, (int)EStatus.WALK)); break;
			case EStatus.GETON: Debug.Log("エイリアン「" + id + "」" + "の着席状態は" + GetStatusFlag(id, (int)EStatus.GETON)); break;
			case EStatus.ORDER: Debug.Log("エイリアン「" + id + "」" + "の注文状態は" + GetStatusFlag(id, (int)EStatus.ORDER)); break;
			case EStatus.STAY: Debug.Log("エイリアン「" + id + "」" + "の待つ(注文待機)状態は" + GetStatusFlag(id, (int)EStatus.STAY)); break;
			case EStatus.CLAIM: Debug.Log("エイリアン「" + id + "」" + "のクレーム状態は" + GetStatusFlag(id, (int)EStatus.CLAIM)); break;
			case EStatus.EAT: Debug.Log("エイリアン「" + id + "」" + "の食べる状態は" + GetStatusFlag(id, (int)EStatus.EAT)); break;
			case EStatus.RETURN_GOOD: Debug.Log("エイリアン「" + id + "」" + "の帰る(良)状態は" + GetStatusFlag(id, (int)EStatus.RETURN_GOOD)); break;
			case EStatus.RETURN_BAD: Debug.Log("エイリアン「" + id + "」" + "の帰る(悪)状態は" + GetStatusFlag(id, (int)EStatus.RETURN_BAD)); break;
			case EStatus.SCREEN_OUT: Debug.Log("エイリアン「" + id + "」" + "の画面外状態は" + GetStatusFlag(id, (int)EStatus.SCREEN_OUT)); break;
			default: break;
		}

		//Debug.Log(id + "の待機状態は" + GetStatusFlag(id, (int)EStatus.STAND));
		//Debug.Log(id + "の入店時状態は" + GetStatusFlag(id, (int)EStatus.WALK));
		//Debug.Log(id + "の着席状態は" + GetStatusFlag(id, (int)EStatus.GETON));
		//Debug.Log(id + "の注文状態は" + GetStatusFlag(id, (int)EStatus.ORDER));
		//Debug.Log(id + "の待つ(注文待機)状態は" + GetStatusFlag(id, (int)EStatus.STAY));
		//Debug.Log(id + "のクレーム状態は" + GetStatusFlag(id, (int)EStatus.CLAIM));
		//Debug.Log(id + "の食べる状態は" + GetStatusFlag(id, (int)EStatus.EAT));
		//Debug.Log(id + "の帰る(良)状態は" + GetStatusFlag(id, (int)EStatus.RETURN_GOOD));
		//Debug.Log(id + "の帰る(悪)状態は" + GetStatusFlag(id, (int)EStatus.RETURN_BAD));
		//Debug.Log(id + "の画面外状態は" + GetStatusFlag(id, (int)EStatus.SCREEN_OUT));
	}

	/// <summary>
	/// 状態管理の格納
	/// </summary>
	/// <param name="_status"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetStatusFlag(bool _statusChangeFlag, int numId, int statusId) => statusChangeFlag[numId, statusId] = _statusChangeFlag;

	/// <summary>
	/// 状態管理の取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetStatusFlag(int numId, int statusId) => statusChangeFlag[numId, statusId];
}
