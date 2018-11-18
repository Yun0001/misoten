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
	Vector3[,,] counterSeatsPosition = new Vector3[7, 4, 2];	// [最大席の数、移動回数、入店&退店時移動]

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
	private int setEndPositionId = 0;

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

		// 歩行アニメーションになる
		GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.WORK);

		// 入店時移動状態の初期化
		whenEnteringStoreMoveFlag = true;

		// 退店時移動状態の初期化
		whenLeavingStoreFlag = false;

		// 終点座標IDの初期化
		setEndPositionId = 0;

		// 時間更新用の初期化
		timeAdd = 0.0f;

		// 予定時間を割る用の初期化
		rate = 0.0f;
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

            for (int i = 0; i < alienCall.GetCounterSeatsMax(); i++)
            {
                if (AlienStatus.GetCounterStatusChangeFlag(i, (int)AlienStatus.EStatus.WALK_SIDE))
                {
                    PlayWalkSE();
                    break;
                }
            }
        }
		// 退店時移動状態の時
		if (GetWhenLeavingStoreFlag())
		{
			// カウンター席側のエイリアンの退店時移動処理
			CounterWhenLeavingStoreMove();

            for (int i = 0; i < alienCall.GetCounterSeatsMax(); i++)
            {
                if (AlienStatus.GetCounterStatusChangeFlag(i, (int)AlienStatus.EStatus.WALK_SIDE))
                {
                    PlayWalkSE();
                    break;
                }
            }
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
		counterSeatsPosition[4, 1, 0] = counterSeatsPosition[5, 1, 0] = counterSeatsPosition[6, 1, 0] = new Vector3(7.0f, 0.8f, 5.0f);

		// 三つ目の終点座標の設定(入店時)
		counterSeatsPosition[0, 2, 0] = counterSeatsPosition[1, 2, 0] = counterSeatsPosition[2, 2, 0] = counterSeatsPosition[3, 2, 0] = new Vector3(-7.0f, 0.8f, 3.1f);
		counterSeatsPosition[4, 2, 0] = counterSeatsPosition[5, 2, 0] = counterSeatsPosition[6, 2, 0] = new Vector3(7.0f, 0.8f, 3.1f);

		// 四つ目の終点座標の設定(入店時)&一つ目の終点座標の設定(退店時)
		counterSeatsPosition[0, 3, 0] = new Vector3(-3.35f, 0.8f, 3.1f);
		counterSeatsPosition[1, 3, 0] = new Vector3(-2.25f, 0.8f, 3.09f);
		counterSeatsPosition[2, 3, 0] = new Vector3(-1.15f, 0.8f, 3.08f);
		counterSeatsPosition[3, 3, 0] = new Vector3(0.0f, 0.8f, 3.07f);
		counterSeatsPosition[4, 3, 0] = new Vector3(3.35f, 0.8f, 3.06f);
		counterSeatsPosition[5, 3, 0] = new Vector3(2.25f, 0.8f, 3.07f);
		counterSeatsPosition[6, 3, 0] = new Vector3(1.15f, 0.8f, 3.08f);

		// 二つ目の終点座標の設定(退店時)
		counterSeatsPosition[0, 1, 1] = counterSeatsPosition[1, 1, 1] = counterSeatsPosition[2, 1, 1] = counterSeatsPosition[3, 1, 1] = new Vector3(-7.0f, 0.8f, 3.1f);
		counterSeatsPosition[4, 1, 1] = counterSeatsPosition[5, 1, 1] = counterSeatsPosition[6, 1, 1] = new Vector3(7.0f, 0.8f, 3.1f);
	}

	/// <summary>
	/// カウンター席への移動処理関数
	/// </summary>
	void CounterSeatsMove()
	{
		// 時間更新
		timeAdd += Time.deltaTime;

		// 予定時間を割る
		rate = timeAdd / WhenEnteringStoreMoveTime[setEndPositionId];

		// カウンター席に着席する管理
		switch (setEndPositionId)
		{
			case 0:
				// 一つ目の終点座標に到着(画面外に向かって歩いている状態「ON」)
				if (timeAdd > WhenEnteringStoreMoveTime[0])
                {
                    Sound.PlaySe(GameSceneManager.seKey[1]);
                    setEndPositionId = 1; timeAdd = 0.0f;
                    AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.WALK_SIDE);
                }
				transform.position = Vector3.Lerp(new Vector3(0.0f, 0.8f, 7.0f), counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 0, 0], rate);

				// 右移動アニメーション
				RightMoveAnimation();
				break;
			case 1:
				// 二つ目の終点座標に到着(画面外に消えた状態「ON」)(画面外に向かって歩いている状態「OFF」)
				if (timeAdd > WhenEnteringStoreMoveTime[1])
				{
					setEndPositionId = 2;
					timeAdd = 0.0f;
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.OUT_SCREEN);
					AlienStatus.SetCounterStatusChangeFlag(false, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.WALK_SIDE);
				}
				transform.position = Vector3.Lerp(counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 0, 0], counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 1, 0], rate);

				if (GetComponent<AlienOrder>().GetSetId() < 4) { RightMoveAnimation(); }
				else { LeftMoveAnimation(); }

				// ドアのアニメーションを行う
				AlienCall.SetdoorAnimationFlag(false);
				break;
			case 2:
				// 三つ目の終点座標に到着(客席に向かって歩いている「ON」)
				if (timeAdd > WhenEnteringStoreMoveTime[2]) { setEndPositionId = 3; timeAdd = 0.0f; AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.WALK_IN); }
				transform.position = Vector3.Lerp(counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 1, 0], counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 2, 0], rate);

				if (GetComponent<AlienOrder>().GetSetId() < 4) { LeftMoveAnimation(); }
				else { RightMoveAnimation(); }
				break;
			case 3:
				// 四つ目の終点座標に到着
				if (timeAdd > WhenEnteringStoreMoveTime[3])
				{
					// 訪問時の移動状態「OFF」
					AlienStatus.SetCounterStatusChangeFlag(false, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.VISIT);

					// 着席状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.GETON);

					// 退店時の時の為に初期化
					timeAdd = 0.0f;

					// 入店時の移動終了
					whenEnteringStoreMoveFlag = false;

					// 退店時の為に初期化
					setEndPositionId = 0;

					// 待機アニメーションになる
					GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.WAIT);

					// 右移動時のアニメーション
					RightMoveAnimation();

					for (int i = 0; i < alienCall.GetCounterSeatsMax(); i++)
					{
						if (AlienStatus.GetCounterStatusChangeFlag(i, (int)AlienStatus.EStatus.WALK_SIDE))
						{
							break;
						}
						if (i == alienCall.GetCounterSeatsMax())
						{
							Sound.SetLoopFlgSe(GameSceneManager.seKey[6], false, 9);
							Sound.PlaySe(GameSceneManager.seKey[6], 9);
						}
					}

					// スクリプトを切る
					//enabled = false;
				}
				transform.position = Vector3.Lerp(counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 2, 0], counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 3, 0], rate);
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
		if(!GetCounterClosedCompletion(GetComponent<AlienOrder>().GetSetId()))
		{
			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / WhenLeavingStoreMoveTime;

			// BoxColliderを「OFF」にする
			GetComponent<BoxCollider>().enabled = false;

			// 歩行アニメーションになる
			GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.WORK);

			if (GetComponent<AlienOrder>().GetSetId() < 4) { RightMoveAnimation(); }
			else { LeftMoveAnimation(); }

			// 終点座標に到着
			if (timeAdd > WhenLeavingStoreMoveTime)
            {
                Sound.PlaySe(GameSceneManager.seKey[3]);
                counterClosedCompletion[GetComponent<AlienOrder>().GetSetId()] = true;
            }
			transform.position = Vector3.Lerp(counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 3, 0], counterSeatsPosition[GetComponent<AlienOrder>().GetSetId(), 1, 1], rate);
		}
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


    private void PlayWalkSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[6], true, 9);
        Sound.PlaySe(GameSceneManager.seKey[6], 9);
    }

    private void StopWalkSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[6], false, 9);
        Sound.StopSe(GameSceneManager.seKey[6], 9);
    }
}