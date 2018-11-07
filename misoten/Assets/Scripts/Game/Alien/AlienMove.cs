using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがカウンター席に向かって移動を行うスクリプト
/// </summary>
public class AlienMove : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンの向き
	/// </summary>
	public enum EDirection
	{
		Right,
		Left
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定終点座標
	[SerializeField]
	Vector3[,,] counterSeatsPosition = new Vector3[9, 4, 2];	// [最大席の数、移動回数、入店&退店時移動]

	// 入店移動時間
	[SerializeField]
	private float[] WhenEnteringStoreMoveTime = new float[4];

	// 退店移動時間
	[SerializeField]
	private float WhenLeavingStoreMoveTime;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// ---------------------------------------------

	// 退店完了判定
	private static bool[] counterClosedCompletion = { false, false, false, false, false, false, false, false, false };

	// ローカル変数
	// ---------------------------------------------

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// 開始座標設定用
	private Vector3 startPosition;

	// 入店時移動状態
	private bool whenEnteringStoreMoveFlag = true;

	// 退店時移動状態
	private bool whenLeavingStoreFlag = false;

	// 終点座標ID
	private int setEndPositionId_1 = 0;
	private int setEndPositionId_2 = 0;

	// 時間更新用
	private float timeAdd = 0.0f;

	// 予定時間を割る用
	private float rate = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		alienCall = GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>();

		// カウンター席に座る為の座標設定
		CounterSeatsMoveInit();

		// 空いている席に座る
		setEndPositionId_1 = AlienCall.GetIdSave();

		// 歩行アニメーションになる
		GetComponent<AlienAnimation>().SetIsCatering(true);
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 入店時移動状態の時
		if (GetWhenEnteringStoreMoveFlag())
		{
			// カウンター席への移動処理
			CounterSeatsMove();
		}
		// 退店時移動状態の時
		if (GetWhenLeavingStoreFlag())
		{
			// カウンター席側のエイリアンの退店時移動処理
			CounterWhenLeavingStoreMove();
		}
	}

	/// <summary>
	/// カウンター席に移動する為の座標設定関数
	/// </summary>
	void CounterSeatsMoveInit()
	{
		// 一つ目の終点座標の設定(入店時)
		for (int i = 0; i < alienCall.GetCounterSeatsMax(); i++) { counterSeatsPosition[i, 0, 0] = new Vector3(0.0f, 0.8f, 5.0f); }

		// 二つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 1, 0] = counterSeatsPosition[1, 1, 0] = counterSeatsPosition[2, 1, 0] = counterSeatsPosition[3, 1, 0] = new Vector3(-7.0f, 0.8f, 5.0f);
		counterSeatsPosition[4, 1, 0] = counterSeatsPosition[5, 1, 0] = counterSeatsPosition[6, 1, 0] = counterSeatsPosition[7, 1, 0] = counterSeatsPosition[8, 1, 0] = new Vector3(7.0f, 0.8f, 5.0f);

		// 三つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 2, 0] = counterSeatsPosition[1, 2, 0] = counterSeatsPosition[2, 2, 0] = counterSeatsPosition[3, 2, 0] = new Vector3(-7.0f, 0.8f, 3.1f);
		counterSeatsPosition[4, 2, 0] = counterSeatsPosition[5, 2, 0] = counterSeatsPosition[6, 2, 0] = counterSeatsPosition[7, 2, 0] = counterSeatsPosition[8, 2, 0] = new Vector3(7.0f, 0.8f, 3.1f);

		// 四つ目の終点座標の設定(入店時)&一つ目の終点座標の設定(退店時)
		counterSeatsPosition[0, 3, 0] = new Vector3(-4.0f, 0.8f, 3.1f);
		counterSeatsPosition[1, 3, 0] = new Vector3(-3.0f, 0.8f, 3.09f);
		counterSeatsPosition[2, 3, 0] = new Vector3(-2.0f, 0.8f, 3.08f);
		counterSeatsPosition[3, 3, 0] = new Vector3(-1.0f, 0.8f, 3.07f);
		counterSeatsPosition[4, 3, 0] = new Vector3(0.0f, 0.8f, 3.06f);
		counterSeatsPosition[5, 3, 0] = new Vector3(1.0f, 0.8f, 3.07f);
		counterSeatsPosition[6, 3, 0] = new Vector3(2.0f, 0.8f, 3.08f);
		counterSeatsPosition[7, 3, 0] = new Vector3(3.0f, 0.8f, 3.09f);
		counterSeatsPosition[8, 3, 0] = new Vector3(4.0f, 0.8f, 3.1f);

		// 二つ目の終点座標の設定(退店時)
		counterSeatsPosition[0, 1, 1] = counterSeatsPosition[1, 1, 1] = counterSeatsPosition[2, 1, 1] = counterSeatsPosition[3, 1, 1] = new Vector3(-7.0f, 0.8f, 3.1f);
		counterSeatsPosition[4, 1, 1] = counterSeatsPosition[5, 1, 1] = counterSeatsPosition[6, 1, 1] = counterSeatsPosition[7, 1, 1] = counterSeatsPosition[8, 1, 1] = new Vector3(7.0f, 0.8f, 3.1f);
	}

	/// <summary>
	/// カウンター席への移動処理関数
	/// </summary>
	void CounterSeatsMove()
	{
		// 時間更新
		timeAdd += Time.deltaTime;

		// 予定時間を割る
		rate = timeAdd / WhenEnteringStoreMoveTime[setEndPositionId_2];

		// カウンター席に着席する管理
		switch (setEndPositionId_2)
		{
			case 0:
				// 一つ目の終点座標に到着
				if (timeAdd > WhenEnteringStoreMoveTime[0]) { setEndPositionId_2 = 1; timeAdd = 0.0f; }
				transform.position = Vector3.Lerp(new Vector3(0.0f, 0.8f, 7.0f), counterSeatsPosition[setEndPositionId_1, 0, 0], rate);

				// 右移動アニメーション
				RightMoveAnimation();
				break;
			case 1:
				// 二つ目の終点座標に到着
				if (timeAdd > WhenEnteringStoreMoveTime[1]) { setEndPositionId_2 = 2; timeAdd = 0.0f; }
				transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 0, 0], counterSeatsPosition[setEndPositionId_1, 1, 0], rate);

				if (setEndPositionId_1 < 4) { RightMoveAnimation(); }
				else { LeftMoveAnimation(); }
				break;
			case 2:
				// 三つ目の終点座標に到着
				if (timeAdd > WhenEnteringStoreMoveTime[2]) { setEndPositionId_2 = 3; timeAdd = 0.0f; }
				transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 1, 0], counterSeatsPosition[setEndPositionId_1, 2, 0], rate);

				if (setEndPositionId_1 < 4) { LeftMoveAnimation(); }
				else { RightMoveAnimation(); }
				break;
			case 3:
				// 四つ目の終点座標に到着
				if (timeAdd > WhenEnteringStoreMoveTime[3])
				{
					// 入店時の移動状態「OFF」
					AlienStatus.SetCounterStatusChangeFlag(false, setEndPositionId_1, (int)AlienStatus.EStatus.WALK);

					// 着席状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, setEndPositionId_1, (int)AlienStatus.EStatus.GETON);

					// 退店時の時の為に初期化
					timeAdd = 0.0f;

					// 入店時の移動終了
					whenEnteringStoreMoveFlag = false;

					// 移動終了時、BoxColliderを「ON」にする
					GetComponent<BoxCollider>().enabled = true;

					// 退店時の為に初期化
					setEndPositionId_2 = 0;

					// 待機アニメーションになる
					GetComponent<AlienAnimation>().SetIsCatering(false);

					// 右移動時のアニメーション
					RightMoveAnimation();

					// スクリプトを切る
					//enabled = false;
				}
				transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 2, 0], counterSeatsPosition[setEndPositionId_1, 3, 0], rate);
				break;
			default: break;
		}
	}

	/// <summary>
	/// カウンター客の退店時移動処理
	/// </summary>
	void CounterWhenLeavingStoreMove()
	{
		// 退店完了ではない時
		if(!GetCounterClosedCompletion(setEndPositionId_1))
		{
			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / WhenLeavingStoreMoveTime;

			// 移動終了時、BoxColliderを「OFF」にする
			GetComponent<BoxCollider>().enabled = false;

			// 歩行アニメーションになる
			GetComponent<AlienAnimation>().SetIsCatering(true);

			if (setEndPositionId_1 < 4) { RightMoveAnimation(); }
			else { LeftMoveAnimation(); }

			// 終点座標に到着
			if (timeAdd > WhenLeavingStoreMoveTime) { counterClosedCompletion[setEndPositionId_1] = true; }
			transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 3, 0], counterSeatsPosition[setEndPositionId_1, 1, 1], rate);
		}

		/*
		 counterSeatsPosition[0, 0, 1] = 
		 counterSeatsPosition[1, 0, 1] = 
		 counterSeatsPosition[2, 0, 1] = 
		 counterSeatsPosition[3, 0, 1] = 
		 counterSeatsPosition[4, 0, 1] = 
		 counterSeatsPosition[5, 0, 1] = 
		 counterSeatsPosition[6, 0, 1] = 
		 counterSeatsPosition[7, 0, 1] = 
		 counterSeatsPosition[8, 0, 1] = 
		 */
	}

	/// <summary>
	/// 右移動アニメーション
	/// </summary>
	void RightMoveAnimation()
	{
		GetComponent<AlienAnimation>().SetAlienRLDirection(EDirection.Right);
		GetComponent<AlienAnimation>().SetAlienUDDirection(EDirection.Right);
	}

	/// <summary>
	/// 左移動アニメーション
	/// </summary>
	void LeftMoveAnimation()
	{
		GetComponent<AlienAnimation>().SetAlienRLDirection(EDirection.Left);
		GetComponent<AlienAnimation>().SetAlienUDDirection(EDirection.Left);
	}

	/// <summary>
	/// 入店時移動状態を格納
	/// </summary>
	/// <param name="_whenEnteringStoreMoveFlag"></param>
	/// <returns></returns>
	public bool SetWhenEnteringStoreMoveFlag(bool _whenEnteringStoreMoveFlag) => whenEnteringStoreMoveFlag = _whenEnteringStoreMoveFlag;

	/// <summary>
	/// 入店時移動状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetWhenEnteringStoreMoveFlag() => whenEnteringStoreMoveFlag;

	/// <summary>
	/// 退店時移動状態を格納
	/// </summary>
	/// <param name="_whenLeavingStoreFlag"></param>
	/// <returns></returns>
	public bool SetWhenLeavingStoreFlag(bool _whenLeavingStoreFlag) => whenLeavingStoreFlag = _whenLeavingStoreFlag;

	/// <summary>
	/// 退店時移動状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetWhenLeavingStoreFlag() => whenLeavingStoreFlag;

	/// <summary>
	/// カウンター側のエイリアンが退店完了かの格納
	/// </summary>
	/// <param name="_counterClosedCompletion"></param>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static bool SetCounterClosedCompletion(bool _counterClosedCompletion, int seatId) => counterClosedCompletion[seatId] = _counterClosedCompletion;

	/// <summary>
	/// カウンター側のエイリアンが退店完了かの取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static bool GetCounterClosedCompletion(int seatId) => counterClosedCompletion[seatId];
}