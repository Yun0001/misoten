using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの状態管理スクリプト
/// </summary>
public class AlienStatus : MonoBehaviour
{
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

	// 初期化フラグ
	private bool initFlag = false;

	// 状態遷移フラグ
	private static bool[] statusChangeFlag = new bool[(int)EStatus.MAX];

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 初期化
		if (!initFlag) { Init(); initFlag = true; }

		// 各エイリアンの状態チェックを行う
		StatusCheck();

		// Debug用
		//DebugText();
	}

	/// <summary>
	/// 初期化関数
	/// </summary>
	void Init()
	{
		// 状態管理の初期化
		for (int i = 0; i < (int)EStatus.MAX; i++) { statusChangeFlag[i] = false; }

		// 待機状態にする
		statusChangeFlag[(int)EStatus.STAND] = true;
	}

	/// <summary>
	/// 状態チェック関数
	/// </summary>
	void StatusCheck()
	{
		// 待機状態終了の合図が出ると、OFFにする
		if (AlienCall.GetStand(AlienCall.GetAddId())) { SetStatusFlag(false, (int)EStatus.STAND); }

		// 入店時状態へ移行の合図が出ると、ONにする
		if (GetComponent<AlienMove>().GetWalk()) { SetStatusFlag(true, (int)EStatus.WALK); }

		// 入店時状態終了の合図が出ると、OFFにする
		else { SetStatusFlag(false, (int)EStatus.WALK); }
	}

	/// <summary>
	/// デバッグ用関数
	/// </summary>
	void DebugText()
	{
		Debug.Log("待機状態は" + GetStatusFlag((int)EStatus.STAND));
		Debug.Log("入店時状態は" + GetStatusFlag((int)EStatus.WALK));
		Debug.Log("着席状態は" + GetStatusFlag((int)EStatus.GETON));
		Debug.Log("注文状態は" + GetStatusFlag((int)EStatus.ORDER));
		Debug.Log("待つ(注文待機)状態は" + GetStatusFlag((int)EStatus.STAY));
		Debug.Log("クレーム状態は" + GetStatusFlag((int)EStatus.CLAIM));
		Debug.Log("食べる状態は" + GetStatusFlag((int)EStatus.EAT));
		Debug.Log("帰る(良)状態は" + GetStatusFlag((int)EStatus.RETURN_GOOD));
		Debug.Log("帰る(悪)状態は" + GetStatusFlag((int)EStatus.RETURN_BAD));
		Debug.Log("画面外状態は" + GetStatusFlag((int)EStatus.SCREEN_OUT));
	}

	/// <summary>
	/// 状態管理の格納
	/// </summary>
	/// <param name="_status"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetStatusFlag(bool _status, int id) => statusChangeFlag[id] = _status;

	/// <summary>
	/// 状態管理の取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetStatusFlag(int id) => statusChangeFlag[id];
}
