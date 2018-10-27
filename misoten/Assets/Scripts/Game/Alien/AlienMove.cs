using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがカウンター席に向かって移動を行うスクリプト
/// </summary>
public class AlienMove : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定終点座標(カウンター席に座る)
	[SerializeField]
	Vector3[,,] counterSeatsPosition = new Vector3[7, 5, 2];	// [最大席の数、移動回数、入店&退店時移動]

	// 指定終点座標(持ち帰り用の席に座る)
	[SerializeField]
	Vector3[,,] takeAwaySeatPosition = new Vector3[4, 4, 2];	// [最大席の数、移動回数、入店&退店時移動]

	// 入店移動時間
	[SerializeField]
	private float[] WhenEnteringStoreMoveTime = new float[7];

	// 退店移動時間
	[SerializeField]
	private float[] WhenLeavingStoreMoveTime = new float[2];

	// ---------------------------------------------

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

	// 席の種類保存用
	private int seatPatternSave = 0;

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

		// 座る席の種類保存
		seatPatternSave = alienCall.GetSeatPattern();

		// 各席への移動をする為の設定を行う
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// カウンター席に座る為の座標設定
				CounterSeatsMoveInit();

				// 空いている席に座る
				setEndPositionId_1 = AlienCall.GetIdSave((int)AlienCall.ESeatPattern.COUNTERSEATS);
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				// 持ち帰り用の席に座る為の座標設定
				TakeOutMoveInit();

				// 空いている席に座る
				setEndPositionId_1 = AlienCall.GetIdSave((int)AlienCall.ESeatPattern.TAKEAWAYSEAT);
				break;
			// エラー
			default: Debug.Log("Error:カウンター席でも持ち帰り用の席でもありません"); break;
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// カウンター席or持ち帰り用の席への移動
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// カウンター席への移動処理
				CounterSeatsMove();
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				// 持ち帰り用の席への移動処理
				TakeAwaySeatMove();
				break;
			default: break;
		}

		//// 退店時移動処理
		//WhenLeavingStoreMove();
	}

	/// <summary>
	/// カウンター席に移動する為の座標設定関数
	/// </summary>
	void CounterSeatsMoveInit()
	{
		// 一つ目の終点座標の設定(入店時)
		for (int i = 0; i < alienCall.GetCounterSeatsMax(); i++) { counterSeatsPosition[i, 0, 0] = new Vector3(0.0f, 0.55f, 5.0f); }

		// 二つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 1, 0] = new Vector3(-7.0f, 0.55f, 5.0f);
		counterSeatsPosition[1, 1, 0] = new Vector3(-7.0f, 0.55f, 5.0f);
		counterSeatsPosition[2, 1, 0] = new Vector3(-7.0f, 0.55f, 5.0f);
		counterSeatsPosition[3, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);
		counterSeatsPosition[4, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);
		counterSeatsPosition[5, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);
		counterSeatsPosition[6, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);

		// 三つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 2, 0] = new Vector3(-7.0f, 0.55f, 4.0f);
		counterSeatsPosition[1, 2, 0] = new Vector3(-7.0f, 0.55f, 4.0f);
		counterSeatsPosition[2, 2, 0] = new Vector3(-7.0f, 0.55f, 4.0f);
		counterSeatsPosition[3, 2, 0] = new Vector3(7.0f, 0.55f, 4.0f);
		counterSeatsPosition[4, 2, 0] = new Vector3(7.0f, 0.55f, 4.0f);
		counterSeatsPosition[5, 2, 0] = new Vector3(7.0f, 0.55f, 4.0f);
		counterSeatsPosition[6, 2, 0] = new Vector3(7.0f, 0.55f, 4.0f);

		// 四つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 3, 0] = new Vector3(-3.0f, 0.55f, 4.0f);
		counterSeatsPosition[1, 3, 0] = new Vector3(-2.0f, 0.55f, 4.0f);
		counterSeatsPosition[2, 3, 0] = new Vector3(-1.0f, 0.55f, 4.0f);
		counterSeatsPosition[3, 3, 0] = new Vector3(0.0f, 0.55f, 4.0f);
		counterSeatsPosition[4, 3, 0] = new Vector3(1.0f, 0.55f, 4.0f);
		counterSeatsPosition[5, 3, 0] = new Vector3(2.0f, 0.55f, 4.0f);
		counterSeatsPosition[6, 3, 0] = new Vector3(3.0f, 0.55f, 4.0f);

		// 五つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 4, 0] = new Vector3(-3.0f, 0.92f, 3.5f);
		counterSeatsPosition[1, 4, 0] = new Vector3(-2.0f, 0.92f, 3.5f);
		counterSeatsPosition[2, 4, 0] = new Vector3(-1.0f, 0.92f, 3.5f);
		counterSeatsPosition[3, 4, 0] = new Vector3(0.0f, 0.92f, 3.5f);
		counterSeatsPosition[4, 4, 0] = new Vector3(1.0f, 0.92f, 3.5f);
		counterSeatsPosition[5, 4, 0] = new Vector3(2.0f, 0.92f, 3.5f);
		counterSeatsPosition[6, 4, 0] = new Vector3(3.0f, 0.92f, 3.5f);

		// 一つ目の終点座標の設定(退店時)
		//counterSeatsPosition[0, 0, 1] = new Vector3(-6.0f, 4.5f, -0.1f);
		//counterSeatsPosition[1, 0, 1] = new Vector3(-3.0f, 4.5f, -0.1f);
		//counterSeatsPosition[2, 0, 1] = new Vector3(0.0f, 4.5f, -0.1f);
		//counterSeatsPosition[3, 0, 1] = new Vector3(3.0f, 4.5f, -0.1f);
		//counterSeatsPosition[4, 0, 1] = new Vector3(6.0f, 4.5f, -0.1f);

		// 二つ目の終点座標の設定(退店時)
		//counterSeatsPosition[0, 1, 1] = new Vector3(10.0f, 4.5f, -0.1f);
		//counterSeatsPosition[1, 1, 1] = new Vector3(10.0f, 4.5f, -0.1f);
		//counterSeatsPosition[2, 1, 1] = new Vector3(10.0f, 4.5f, -0.1f);
		//counterSeatsPosition[3, 1, 1] = new Vector3(-10.0f, 4.5f, -0.1f);
		//counterSeatsPosition[4, 1, 1] = new Vector3(-10.0f, 4.5f, -0.1f);
	}

	/// <summary>
	/// 持ち帰り用の席に移動する為の座標設定関数
	/// </summary>
	void TakeOutMoveInit()
	{
		// 一つ目の終点座標の設定(入店時)
		for (int i = 0; i < alienCall.GetTakeAwaySeatMax(); i++) { takeAwaySeatPosition[i, 0, 0] = new Vector3(0.0f, 0.55f, 5.0f); }

		// 二つ目の終点座標の設定(入店時)
		takeAwaySeatPosition[0, 1, 0] = new Vector3(-7.0f, 0.55f, 5.0f);
		takeAwaySeatPosition[1, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);
		takeAwaySeatPosition[2, 1, 0] = new Vector3(-7.0f, 0.55f, 5.0f);
		takeAwaySeatPosition[3, 1, 0] = new Vector3(7.0f, 0.55f, 5.0f);

		// 三つ目の終点座標の設定(入店時)
		takeAwaySeatPosition[0, 2, 0] = new Vector3(-7.0f, 0.55f, 0.5f);
		takeAwaySeatPosition[1, 2, 0] = new Vector3(7.0f, 0.55f, 0.5f);
		takeAwaySeatPosition[2, 2, 0] = new Vector3(-7.0f, 0.55f, -2.0f);
		takeAwaySeatPosition[3, 2, 0] = new Vector3(7.0f, 0.55f, -2.0f);

		// 四つ目の終点座標の設定(入店時)
		takeAwaySeatPosition[0, 3, 0] = new Vector3(-4.7f, 0.55f, 0.5f);
		takeAwaySeatPosition[1, 3, 0] = new Vector3(4.7f, 0.55f, 0.5f);
		takeAwaySeatPosition[2, 3, 0] = new Vector3(-4.7f, 0.55f, -2.0f);
		takeAwaySeatPosition[3, 3, 0] = new Vector3(4.7f, 0.55f, -2.0f);
	}

	/// <summary>
	/// カウンター席への移動処理関数
	/// </summary>
	void CounterSeatsMove()
	{
		// 入店時移動状態の時
		if (whenEnteringStoreMoveFlag)
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
					transform.position = Vector3.Lerp(new Vector3(0.0f, 0.55f, 7.0f), counterSeatsPosition[setEndPositionId_1, 0, 0], rate);
					break;
				case 1:
					// 二つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[1]) { setEndPositionId_2 = 2; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 0, 0], counterSeatsPosition[setEndPositionId_1, 1, 0], rate);
					break;
				case 2:
					// 三つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[2]) { setEndPositionId_2 = 3; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 1, 0], counterSeatsPosition[setEndPositionId_1, 2, 0], rate);
					break;
				case 3:
					// 四つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[3]) { setEndPositionId_2 = 4; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 2, 0], counterSeatsPosition[setEndPositionId_1, 3, 0], rate);
					break;
				case 4:
					// 五つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[4])
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

						// スクリプトを切る
						//enabled = false;
					}
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 3, 0], counterSeatsPosition[setEndPositionId_1, 4, 0], rate);
					break;
			}
		}
	}

	/// <summary>
	/// 持ち帰り用の席への移動処理関数
	/// </summary>
	void TakeAwaySeatMove()
	{
		// 入店時移動状態の時
		if (whenEnteringStoreMoveFlag)
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
					transform.position = Vector3.Lerp(new Vector3(0.0f, 0.55f, 7.0f), takeAwaySeatPosition[setEndPositionId_1, 0, 0], rate);
					break;
				case 1:
					// 二つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[1]) { setEndPositionId_2 = 2; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(takeAwaySeatPosition[setEndPositionId_1, 0, 0], takeAwaySeatPosition[setEndPositionId_1, 1, 0], rate);
					break;
				case 2:
					// 三つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[2]) { setEndPositionId_2 = 3; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(takeAwaySeatPosition[setEndPositionId_1, 1, 0], takeAwaySeatPosition[setEndPositionId_1, 2, 0], rate);
					break;
				case 3:
					// 四つ目の終点座標に到着
					if (timeAdd > WhenEnteringStoreMoveTime[3])
					{
						// 入店時の移動状態「OFF」
						AlienStatus.SetTakeOutStatusChangeFlag(false, setEndPositionId_1, (int)AlienStatus.EStatus.WALK);

						// 着席状態「ON」
						AlienStatus.SetTakeOutStatusChangeFlag(true, setEndPositionId_1, (int)AlienStatus.EStatus.GETON);

						// 退店時の時の為に初期化
						timeAdd = 0.0f;

						// 入店時の移動終了
						whenEnteringStoreMoveFlag = false;

						// 移動終了時、BoxColliderを「ON」にする
						GetComponent<BoxCollider>().enabled = true;

						// 退店時の為に初期化
						setEndPositionId_2 = 0;

						// スクリプトを切る
						//enabled = false;
					}
					transform.position = Vector3.Lerp(takeAwaySeatPosition[setEndPositionId_1, 2, 0], takeAwaySeatPosition[setEndPositionId_1, 3, 0], rate);
					break;
			}
		}
	}

	/// <summary>
	/// 退店時移動処理
	/// </summary>
	void WhenLeavingStoreMove()
	{
		// 退店時移動状態の時
		if (whenLeavingStoreFlag)
		{
			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / WhenLeavingStoreMoveTime[setEndPositionId_2];

			// 移動終了時、BoxCollider2Dを「OFF」にする
			GetComponent<BoxCollider2D>().enabled = false;

			// カウンター席に着席する管理
			switch (setEndPositionId_2)
			{
				case 0:
					// 一つ目の終点座標に到着
					if (timeAdd > WhenLeavingStoreMoveTime[0]) { setEndPositionId_2 = 1; timeAdd = 0.0f; }
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 2, 0], counterSeatsPosition[setEndPositionId_1, 0, 1], rate);
					break;
				case 1:
					// 二つ目の終点座標に到着
					if (timeAdd > WhenLeavingStoreMoveTime[1])
					{

					}
					transform.position = Vector3.Lerp(counterSeatsPosition[setEndPositionId_1, 0, 1], counterSeatsPosition[setEndPositionId_1, 1, 1], rate);
					break;
			}
		}
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
}