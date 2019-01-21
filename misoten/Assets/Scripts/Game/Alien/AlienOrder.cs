using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンのオーダースクリプト
/// </summary>
public class AlienOrder : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// 注文の種類
	/// </summary>
	public enum EOrderType
	{
		BASE = 0,	// 注文内容(ベース用)
		CHANGE,		// 注文内容(チェンジ用)
		MAX			// 最大
	}

	/// <summary>
	/// 注文の種類(ベース用)
	/// </summary>
	private enum EOrderBaseType
	{
		RED = 3,	// 赤
		BLUE = 5,	// 青
		YELLOW = 1,	// 黄
		MAX			// 最大
	}

	/// <summary>
	/// 注文の種類(チェンジ用)
	/// </summary>
	private enum EOrderChangeType
	{
		PURPLE = 4, // 紫
		GREEN = 6,	// 緑
		ORANGE = 2,	// 橙
		MAX			// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// オーダー内容描画用
	[SerializeField]
	GameObject[] orderBalloon = new GameObject[6];

	// プレイヤー吹き出し描画用
	[SerializeField]
	GameObject[] playerBalloon = new GameObject[4];

	// オーダーするまでの時間
	[SerializeField]
	private float orderTime;

	// 画面外に移動する時に指定した時間だけ注文内容を出す用
	[SerializeField]
	private float orderPlanDispCount;

	// イート行動を行う時間
	[SerializeField]
	private float eatingCount;

	// 各エイリアンの注文タイプの確率用
	[SerializeField, Range(2, 99)]
	private int eachAlienOrderTypeValue = 0;

	// オーダーベース用
	[SerializeField]
	private int individualOrderBaseType;

	// オーダーチェンジ用
	[SerializeField]
	private int individualOrderChangeType;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// オーダーフラグ
	private static bool orderFlag = false;

	// 例外処理用のカウント
	private static int exceptionCount = 0;

	// 例外処理用のオーダー
	private static int exceptionOrderType = 0;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// プレイヤーへの参照の為
	private string[] nameObj = { "Player1", "Player2", "Player3", "Player4" };

	// プレイヤーオブジェクト
	private GameObject[] playerObj = new GameObject[4];

	// プレイヤーから渡されたイートイの管理用
	private GameObject eatoyObj;

	// オーダー中かの判定
	private bool isOrder = false;

	// 状態移行フラグ
	private bool statusMoveFlag = false;

	// 当たり判定フラグ
	private bool boxColliderFlag = true;

	// オーダーの種類
	private int orderType = 0;

	// 各エイリアンの注文タイプ
	private int eachAlienOrderType = 0;

	// オーダー内容をセーブ(チェンジ)
	private int orderChangeSave = 0;

	// エイリアン毎のID
	private int setId = 0;

	// 注文傾向
	private int orderTrend = 0;

	// オーダー内容をセーブ(ベース)
	private int orderBaseSave = 0;

	// 注文するまでの時間を測る
	private float orderTimeAdd = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		for(int i = 0; i < 4 ;i++)
		{
			playerObj[i] = GameObject.Find(nameObj[i]).gameObject;
		}

		// IDの保存
		setId = AlienCall.GetIdSave();

		// 3種のエイリアンの色に合わせてテーブルを作成しテーブルを決定
		OrderTable();

		// オーダー中かの判定の初期化
		isOrder = false;

		// 状態移行フラグの初期化
		statusMoveFlag = false;

		// 当たり判定フラグ
		boxColliderFlag = true;

		// 注文するまでの時間の初期化
		orderTimeAdd = 0.0f;
}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 食べる状態の時
		if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT))
		{
			// イートアニメーションになる
			GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.EAT);

			// 当たり判定が消える
			GetComponent<BoxCollider>().enabled = false;

			// 「0」になると、クレーム状態or満足状態に移行
			if (eatingCount <= 0.0f)
			{
				// イートイオブジェクトを削除
				Destroy(GetComponent<AlienOrder>().GetEatoyObj());
				statusMoveFlag = true;
				AlienStatus.SetCounterStatusChangeFlag(false, setId, (int)AlienStatus.EStatus.EAT);
			}
			else { eatingCount -= Time.deltaTime; }
		}

		// カウンター席に座っているエイリアンからの注文処理
		CounterOrder();

		// Fadeが開始された時
		if (AlienCall.alienCall.GetCoinFoObj().GetComponent<CoinFO>().GetIsStartingCoinFO())
		{
			// 注文したものを非アクティブにする(吹き出し)
			OrderType(false);
		}
	}

	/// <summary>
	/// カウンター席に座っているエイリアンからのオーダー
	/// </summary>
	void CounterOrder()
	{
		// エイリアンがカウンター席に座っている状態の時
		if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.GETON))
		{
			// エイリアンが注文していて、他のエイリアンがクレームを出している場合
			// エイリアンの注文内容が見えなくなる。
			if (GetIsOrder() && AlienClaim.GetClaimFlag() || GetIsOrder()
				&& GetComponent<AlienSatisfaction>().GetSatisfactionFlag()
				|| GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// 注文したものを非アクティブにする(吹き出し)
				OrderType(false);
			}

			// エイリアンがクレームを終えて、既に注文をしているエイリアンの注文内容を
			// 再び見えるようにする
			if (GetIsOrder() && !AlienClaim.GetClaimFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag()
				&& !GetComponent<AlienSatisfaction>().GetSatisfactionFlag()
				&& !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag()
				&& !GetComponent<AlienMove>().GetWhenLeavingStoreFlag()
				&& !AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.CLAIM))
			{
				// 注文したものをアクティブにする(吹き出し)
				OrderType(true);
			}

			// 食べる状態の時
			if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT))
			{
				// 注文したものを非アクティブにする(吹き出し)
				OrderType(false);
			}

			// エイリアンが席に座って、注文するまでの時間
			if (orderTimeAdd >= orderTime)
			{
				// エイリアンが注文していない時
				if (!GetIsOrder() && !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienSatisfaction>().GetSatisfactionFlag()
					&& !GetComponent<AlienMove>().GetWhenLeavingStoreFlag() && !AlienClaim.GetClaimFlag())
				{
					// オーダーセット
					OrderSet();

					// 注文したものをアクティブにする(吹き出し)
					OrderType(true);

					// 注文状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.ORDER);

					// オーダー完了
					SetIsOrder(true);

					// オーダーの種類管理
					switch (orderType)
					{
						case (int)EOrderType.BASE: individualOrderBaseType = orderBaseSave; break;
						case (int)EOrderType.CHANGE: individualOrderChangeType = orderChangeSave; break;
						default: break;
					}
				}
			}
			else
			{
				// 毎フレームの時間を加算
				orderTimeAdd += Time.deltaTime;
			}

			// プレイヤーの顔の吹き出しを出す為の関数
			for (int i = 0; i < 4; i++) { CorrespondingToEatoy(i); }
		}

		// 画面外に向かって歩いている状態の時
		if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.WALK_SIDE))
		{
			// 「0」になるまで、注文内容を描画する
			if (orderPlanDispCount <= 0.0f)
			{
				OrderType(false);
				AlienStatus.SetCounterStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.SCREEN_OUT);
				AlienStatus.SetCounterStatusChangeFlag(false, setId, (int)AlienStatus.EStatus.GETON);
			}
			else { OrderType(true); orderPlanDispCount -= Time.deltaTime; }
		}
	}

	/// <summary>
	/// オーダーのセット関数
	/// </summary>
	void OrderSet()
	{
		// 例外処理
		if (AlienCall.GetExceptionFlag())
		{
			if (exceptionCount < (int)AlienStatus.EStatus.ORDER) { exceptionCount++; }
			else
			{
				// 例外カウント初期化
				exceptionCount = 0;

				// 例外処理終了
				AlienCall.SetExceptionFlag(false);

				// 次回も同じ注文が続くようにする
				if (exceptionOrderType < (int)AlienStatus.EStatus.ORDER) { exceptionOrderType++; }
				else { exceptionOrderType = 0; }
			}
		}
	}

	/// <summary>
	/// エイリアンのオーダーテーブル関数
	/// </summary>
	void OrderTable()
	{
		// 各エイリアンの注文タイプ
		eachAlienOrderType = Random.Range(1, 101);

		// エイリアンの種類によって、注文傾向が変わる
		switch ((AlienCall.EAlienPattern)AlienCall.alienCall.GetAlienPattern(GetSetId()))
		{
			case AlienCall.EAlienPattern.MARTIAN:   // 火星人(赤)
				//orderBaseSave = (int)EOrderBaseType.RED; orderType = (int)EOrderType.BASE;
				if (eachAlienOrderType >= 1 && eachAlienOrderType <= eachAlienOrderTypeValue)
				{
					OrderConfiguration(EOrderChangeType.PURPLE, EOrderChangeType.ORANGE,
					EOrderBaseType.BLUE, EOrderBaseType.YELLOW, EOrderBaseType.RED);
				}
				if (eachAlienOrderType >= eachAlienOrderTypeValue + 1 && eachAlienOrderType <= 100)
				{
					ChangeOrderConfiguration(EOrderChangeType.PURPLE, EOrderChangeType.ORANGE);
				}
				break;
			case AlienCall.EAlienPattern.MERCURY:   // 水星人(青)
				//orderBaseSave = (int)EOrderBaseType.RED; orderType = (int)EOrderType.BASE;
				if (eachAlienOrderType >= 1 && eachAlienOrderType <= eachAlienOrderTypeValue)
				{
					OrderConfiguration(EOrderChangeType.PURPLE, EOrderChangeType.GREEN,
					EOrderBaseType.RED, EOrderBaseType.YELLOW, EOrderBaseType.BLUE);
				}
				if (eachAlienOrderType >= eachAlienOrderTypeValue + 1 && eachAlienOrderType <= 100)
				{
					ChangeOrderConfiguration(EOrderChangeType.PURPLE, EOrderChangeType.GREEN);
				}
				break;
			case AlienCall.EAlienPattern.VENUSIAN:  // 金星人(黄)
				//orderBaseSave = (int)EOrderBaseType.RED; orderType = (int)EOrderType.BASE;
				if (eachAlienOrderType >= 1 && eachAlienOrderType <= eachAlienOrderTypeValue)
				{
					OrderConfiguration(EOrderChangeType.ORANGE, EOrderChangeType.GREEN,
					EOrderBaseType.RED, EOrderBaseType.BLUE, EOrderBaseType.YELLOW);
				}
				if (eachAlienOrderType >= eachAlienOrderTypeValue + 1 && eachAlienOrderType <= 100)
				{
					ChangeOrderConfiguration(EOrderChangeType.ORANGE, EOrderChangeType.GREEN);
				}
				break;
            //ボスcase orderBaseSave
            case AlienCall.EAlienPattern.BOSS:  // ボス
				//orderBaseSave = (int)EOrderBaseType.RED; orderType = (int)EOrderType.BASE;
				if (eachAlienOrderType >= 1 && eachAlienOrderType <= eachAlienOrderTypeValue)
				{
					OrderConfiguration(EOrderChangeType.ORANGE, EOrderChangeType.GREEN,
					EOrderBaseType.RED, EOrderBaseType.BLUE, EOrderBaseType.YELLOW);
				}
				if (eachAlienOrderType >= eachAlienOrderTypeValue + 1 && eachAlienOrderType <= 100)
				{
					ChangeOrderConfiguration(EOrderChangeType.ORANGE, EOrderChangeType.GREEN);
				}
				break;

			default: Debug.LogError("エイリアンの注文傾向が設定されていません！"); break;
		}
	}

	/// <summary>
	/// オーダー設定関数
	/// </summary>
	void OrderConfiguration(EOrderChangeType change1, EOrderChangeType change2,
		EOrderBaseType base1, EOrderBaseType base2, EOrderBaseType base3)
	{
		// 乱数で注文傾向を決める
		orderTrend = Random.Range(1, 101);

		if (orderTrend >= 1 && orderTrend <= 5) { orderChangeSave = (int)change1; orderType = (int)EOrderType.CHANGE; return; }
		if (orderTrend >= 6 && orderTrend <= 10) { orderChangeSave = (int)change2; orderType = (int)EOrderType.CHANGE; return; }
		if (orderTrend >= 11 && orderTrend <= 30) { orderBaseSave = (int)base1; orderType = (int)EOrderType.BASE; return; }
		if (orderTrend >= 31 && orderTrend <= 50) { orderBaseSave = (int)base2; orderType = (int)EOrderType.BASE; return; }
		if (orderTrend >= 51 && orderTrend <= 100) { orderBaseSave = (int)base3; orderType = (int)EOrderType.BASE; return; }
	}

	/// <summary>
	/// チェンジオーダー設定関数
	/// </summary>
	void ChangeOrderConfiguration(EOrderChangeType change1, EOrderChangeType change2)
	{
		// 乱数で注文傾向を決める
		orderTrend = Random.Range(1, 101);

		if (orderTrend >= 1 && orderTrend <= 50) { orderChangeSave = (int)change1; orderType = (int)EOrderType.CHANGE; return; }
		if (orderTrend >= 51 && orderTrend <= 100) { orderChangeSave = (int)change2; orderType = (int)EOrderType.CHANGE; return; }
	}

    //ToDo ボスオーダーtureでもう一度呼ぶ
	/// <summary>
	/// オーダーの種類関数
	/// </summary>
	/// <param name="value"></param>
	void OrderType(bool value)
	{
		// オーダーの種類管理
		switch (orderType)
		{
			case (int)EOrderType.BASE:
				// 注文内容を描画・非描画する(ベース用)
				orderBalloon[orderBaseSave].SetActive(value);
				break;
			case (int)EOrderType.CHANGE:
				// 注文内容を描画・非描画する(チェンジ用)
				orderBalloon[orderChangeSave].SetActive(value);
				break;
			default: break;
		}
	}

	/// <summary>
	/// エイリアンが注文した料理が来たかの判定関数
	/// </summary>
	/// <param name="cuisine"></param>
	public void EatCuisine(GameObject eatoy)
	{
		if(!GetComponent<AlienMove>().GetWhenLeavingStoreFlag() && !AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT))
		{
			// 当たり判定が消える
			GetComponent<BoxCollider>().enabled = false;

			// EAT状態が「ON」になる
			AlienStatus.SetCounterStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.EAT);

			// 料理が来た判定
			GetComponent<AlienChip>().SetCuisineCame(true);

			// オーダーの種類管理
			switch (orderType)
			{
				case (int)EOrderType.BASE:
					if (individualOrderBaseType == (int)eatoy.GetComponent<Eatoy>().GetEatoyColor()) { Satisfaction(eatoy); }
					else { Claim(eatoy); }
					break;
				case (int)EOrderType.CHANGE:
					if (individualOrderChangeType == (int)eatoy.GetComponent<Eatoy>().GetEatoyColor()) { Satisfaction(eatoy); }
					else { Claim(eatoy); }
					break;
				default: break;
			}

			// イートイのセット
			SetEatoyObj(eatoy);
		}
	}

	/// <summary>
	/// 満足
	/// </summary>
	void Satisfaction(GameObject eatoy)
	{
		// 通常の旨味係数
		GetComponent<AlienChip>().SetCuisineCoefficient(eatoy.GetComponent<Eatoy>().GetEatoyPoint());

		// エイリアンが満足する
		GetComponent<AlienSatisfaction>().SetSatisfactionFlag(true);
	}

	/// <summary>
	/// クレーム
	/// </summary>
	void Claim(GameObject eatoy)
	{
		// 半分の旨味係数
		GetComponent<AlienChip>().SetCuisineCoefficient(eatoy.GetComponent<Eatoy>().GetEatoyPoint());

		// エイリアンがクレームをする
		GetComponent<AlienClaim>().SetIsClaim(true);

		// イベントエイリアンに間違った料理を渡した時
		if (GameTimeManager.eventAlienFlg) { GameTimeManager.eventAlienFlg = false; }
	}

	/// <summary>
	/// いずれかのプレイヤーが対応したイートイを持っている場合
	/// そのプレイヤーの顔の吹き出しを出す為の関数
	/// </summary>
	private void CorrespondingToEatoy(int id)
	{
		if (playerObj[id].GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Catering)
		{
			// オーダーの種類管理
			switch (orderType)
			{
				case (int)EOrderType.BASE:
					if (individualOrderBaseType == IsPlayerEatoy(id))
					{
						// 注文したものを非アクティブにする(吹き出し)
						OrderType(false);
						playerBalloon[id].SetActive(true);
					}
					break;
				case (int)EOrderType.CHANGE:
					if (individualOrderChangeType == IsPlayerEatoy(id))
					{
						// 注文したものを非アクティブにする(吹き出し)
						OrderType(false);
						playerBalloon[id].SetActive(true);
					}
					break;
				default: break;
			}
		}
		else { playerBalloon[id].SetActive(false); }
	}

	/// <summary>
	/// プレイヤーが現在持っているイートイ判定用
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	private int IsPlayerEatoy(int id) => (int)playerObj[id].GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().GetEatoyColor();

	/// <summary>
	/// プレイヤーから渡されたイートイの管理用格納
	/// </summary>
	/// <param name="_eatoyObj"></param>
	/// <returns></returns>
	public GameObject SetEatoyObj(GameObject _eatoyObj) => eatoyObj = _eatoyObj;

	/// <summary>
	/// プレイヤーから渡されたイートイの管理用取得
	/// </summary>
	/// <returns></returns>
	public GameObject GetEatoyObj() => eatoyObj;

	/// <summary>
	/// 状態移行フラグの格納
	/// </summary>
	/// <param name="_statusMoveFlag"></param>
	/// <returns></returns>
	public bool SetStatusMoveFlag(bool _statusMoveFlag) => statusMoveFlag = _statusMoveFlag;

	/// <summary>
	/// 状態移行フラグの取得
	/// </summary>
	/// <returns></returns>
	public bool GetStatusMoveFlag() => statusMoveFlag;

	/// <summary>
	/// 注文状態を格納
	/// </summary>
	/// <param name="_isOrder"></param>
	/// <returns></returns>
	public bool SetIsOrder(bool _isOrder) => isOrder = _isOrder;

	/// <summary>
	/// 注文状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetIsOrder() => isOrder;

	/// <summary>
	/// セットIDを取得
	/// </summary>
	/// <returns></returns>
	public int GetSetId() => setId;

	/// <summary>
	/// 保存したオーダー(ベース)の取得
	/// </summary>
	/// <returns></returns>
	public int GetOrderBaseSave() => orderBaseSave;

	/// <summary>
	/// 保存したオーダー(チェンジ)の取得
	/// </summary>
	/// <returns></returns>
	public int GetOrderChangeSave() => orderChangeSave;

	/// <summary>
	/// 注文の種類を格納
	/// </summary>
	/// <param name="_orderType"></param>
	/// <returns></returns>
	public int SetOrderType(int _orderType) => orderType = _orderType;

	/// <summary>
	/// 注文の種類を取得
	/// </summary>
	/// <returns></returns>
	public int GetOrderType() => orderType;

	/// <summary>
	/// オーダーフラグの取得
	/// </summary>
	/// <returns></returns>
	public static bool GetOrderFlag() => orderFlag;
}