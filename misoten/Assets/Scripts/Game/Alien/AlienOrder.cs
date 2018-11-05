﻿using System.Collections;
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
	private enum EOrderType
	{
		GRILLED = 0,	// 焼き
		SIMMER,			// 煮る
		MICROWAVE,		// 電子レンジ
		MAX				// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// オーダー内容描画用
	[SerializeField]
	GameObject[] orderBalloon;

	// オーダーするまでの時間
	[SerializeField]
	private float orderTime;

	[SerializeField]
	private int individualOrderType;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// オーダーフラグ
	private static bool orderFlag = false;

	// 注文結果格納・取得用
	private static int orderType = 0;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// オーダー中かの判定
	private bool[] isOrder = new bool[(int)AlienCall.ESeatPattern.MAX];

	// 席の種類保存用
	private int seatPatternSave = 0;

	// オーダー内容をセーブ
	private int orderSave = 0;

	// エイリアン毎のID
	private int setId = 0;

	// 注文するまでの時間を測る
	private float orderTimeAdd = 0.0f;

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

		// エイリアンが座る席のパターン管理
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// IDの保存
				setId = AlienCall.GetIdSave(seatPatternSave);
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				// IDの保存
				setId = AlienCall.GetIdSave(seatPatternSave);
				break;
			default: break;
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エイリアンが座る席のパターン管理
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// クレーム状態の時or満足状態の時
				if (GetComponent<AlienClaim>().GetIsClaim() || GetComponent<AlienSatisfaction>().GetSatisfactionFlag())
				{
					// EAT状態が「ON」になった時、当たり判定が消える
					AlienStatus.SetCounterStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.EAT);
					GetComponents<BoxCollider>()[(int)AlienCall.ESeatPattern.COUNTERSEATS].enabled = false;
				}

				// カウンター席に座っているエイリアンからの注文処理
				counterOrder();
			break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				// クレーム状態の時or満足状態の時
				if (GetComponent<AlienClaim>().GetIsClaim() || GetComponent<AlienSatisfaction>().GetSatisfactionFlag())
				{
					// EAT状態が「ON」になった時、当たり判定が消える
					AlienStatus.SetTakeOutStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.EAT);
					GetComponents<BoxCollider>()[(int)AlienCall.ESeatPattern.TAKEAWAYSEAT].enabled = false;
				}

				// 持ち帰り席に座っているエイリアンからの注文処理
				TakeOutOrder();
				break;
			default: break;
		}
	}

	/// <summary>
	/// カウンター席に座っているエイリアンからのオーダー
	/// </summary>
	void counterOrder()
	{
		// エイリアンがカウンター席に座っている状態の時
		if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.GETON))
		{
			// エイリアンが注文していて、他のエイリアンがクレームを出している場合
			// エイリアンの注文内容が見えなくなる。
			if (GetIsOrder((int)AlienCall.ESeatPattern.COUNTERSEATS) && AlienClaim.GetClaimFlag()
				|| GetIsOrder((int)AlienCall.ESeatPattern.COUNTERSEATS) && GetComponent<AlienSatisfaction>().GetSatisfactionFlag()
				|| GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// 注文したものを非アクティブにする(吹き出し)
				orderBalloon[orderSave].SetActive(false);
			}

			// エイリアンがクレームを終えて、既に注文をしているエイリアンの注文内容を
			// 再び見えるようにする
			if (GetIsOrder((int)AlienCall.ESeatPattern.COUNTERSEATS) && !AlienClaim.GetClaimFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag()
				&& !GetComponent<AlienSatisfaction>().GetSatisfactionFlag() && !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// 注文したものをアクティブにする(吹き出し)
				orderBalloon[orderSave].SetActive(true);
			}

			// エイリアンが席に座って、注文するまでの時間
			if (orderTimeAdd >= orderTime)
			{
				// エイリアンが注文していない時
				if (!GetIsOrder((int)AlienCall.ESeatPattern.COUNTERSEATS) && !AlienClaim.GetClaimFlag()
					&& !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
				{
					// オーダー内容を保存
					orderSave = GetOrderType();

					// 注文したものをアクティブにする(吹き出し)
					orderBalloon[GetOrderType()].SetActive(true);

					// 注文状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.ORDER);

					// オーダー完了
					SetIsOrder(true, (int)AlienCall.ESeatPattern.COUNTERSEATS);

					individualOrderType = GetOrderType();

					// エイリアンの注文結果を出す(焼き=>煮る=>レンチン)
					SetOrderType(GetOrderType() + 1);

					// 注文をループさせる為に「0」で初期化
					if ((GetOrderType() >= (int)EOrderType.MAX)) { SetOrderType(0); }
				}
			}
			else
			{
				// 毎フレームの時間を加算
				orderTimeAdd += Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// 持ち帰り席に座っているエイリアンからのオーダー
	/// </summary>
	void TakeOutOrder()
	{
		// エイリアンが持ち帰り席に座っている状態の時
		if (AlienStatus.GetTakeOutStatusChangeFlag(setId, (int)AlienStatus.EStatus.GETON))
		{
			// エイリアンが注文していて、他のエイリアンがクレームを出している場合
			// エイリアンの注文内容が見えなくなる。
			if (GetIsOrder((int)AlienCall.ESeatPattern.TAKEAWAYSEAT) && AlienClaim.GetClaimFlag()
				|| GetIsOrder((int)AlienCall.ESeatPattern.TAKEAWAYSEAT) && GetComponent<AlienSatisfaction>().GetSatisfactionFlag()
				|| GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// 注文したものを非アクティブにする(吹き出し)
				orderBalloon[orderSave].SetActive(false);
			}

			// エイリアンがクレームを終えて、既に注文をしているエイリアンの注文内容を
			// 再び見えるようにする
			if (GetIsOrder((int)AlienCall.ESeatPattern.TAKEAWAYSEAT) && !AlienClaim.GetClaimFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag()
				&& !GetComponent<AlienSatisfaction>().GetSatisfactionFlag() && !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// 注文したものをアクティブにする(吹き出し)
				orderBalloon[orderSave].SetActive(true);
			}

			// エイリアンが席に座って、注文するまでの時間
			if (orderTimeAdd >= orderTime)
			{
				// エイリアンが注文していない時
				if (!GetIsOrder((int)AlienCall.ESeatPattern.TAKEAWAYSEAT) && !AlienClaim.GetClaimFlag()
					&& !GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
				{
					// オーダー内容を保存
					orderSave = GetOrderType();

					// 注文したものをアクティブにする(吹き出し)
					orderBalloon[GetOrderType()].SetActive(true);

					// 注文状態「ON」
					AlienStatus.SetTakeOutStatusChangeFlag(true, setId, (int)AlienStatus.EStatus.ORDER);

					// オーダー完了
					SetIsOrder(true, (int)AlienCall.ESeatPattern.TAKEAWAYSEAT);

					individualOrderType = GetOrderType();

					// エイリアンの注文結果を出す(焼き=>煮る=>レンチン)
					SetOrderType(GetOrderType() + 1);

					// 注文をループさせる為に「0」で初期化
					if ((GetOrderType() >= (int)EOrderType.MAX)) { SetOrderType(0); }
				}
			}
			else
			{
				// 毎フレームの時間を加算
				orderTimeAdd += Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// 注文状態を格納
	/// </summary>
	/// <param name="_isOrder"></param>
	/// <returns></returns>
	public bool SetIsOrder(bool _isOrder, int seatId) => isOrder[seatId] = _isOrder;

	/// <summary>
	/// 注文状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetIsOrder(int seatId) => isOrder[seatId];

	/// <summary>
	/// セットIDを取得
	/// </summary>
	/// <returns></returns>
	public int GetSetId() => setId;

	/// <summary>
	/// オーダーフラグの取得
	/// </summary>
	/// <returns></returns>
	public static bool GetOrderFlag() => orderFlag;

	/// <summary>
	///  注文の種類を格納
	/// </summary>
	/// <param name="_orderType"></param>
	/// <returns></returns>
	public static int SetOrderType(int _orderType) => orderType = _orderType;

	/// <summary>
	/// 注文の種類を取得
	/// </summary>
	/// <returns></returns>
	public static int GetOrderType() => orderType;

	/// <summary>
	/// エイリアンが注文した料理が来たかの判定関数
	/// </summary>
	/// <param name="cuisine"></param>
	public void EatCuisine(GameObject cuisine)
	{
		if (individualOrderType == cuisine.GetComponent<Food>().GetCategory())
		{
			GetComponent<AlienChip>().SetCuisineCoefficient(1f);
			//GetComponent<AlienChip>().SetCuisineCoefficient(cuisine.GetComponent<Food>().GetQualityTaste());
			GetComponent<AlienChip>().SetOpponentID(cuisine.GetComponent<Food>().GetOwnershipPlayerID());
			GetComponent<AlienChip>().SetCuisineCame(true);

			//AlienCall.SetClaimId(AlienCall.GetAddId());
			Debug.Log("注文内容が合ってる");

			GetComponent<AlienSatisfaction>().SetSatisfactionFlag(true);
		}
		else
		{
			GetComponent<AlienChip>().SetCuisineCoefficient(0.5f);
			GetComponent<AlienChip>().SetOpponentID(cuisine.GetComponent<Food>().GetOwnershipPlayerID());
			GetComponent<AlienChip>().SetCuisineCame(true);

			// クレームフラグが立つ
			//AlienCall.SetClaimFlag(true, AlienCall.GetClaimId());

			//AlienCall.SetClaimId(AlienCall.GetAddId());

			// クレーム状態「ON」
			//AlienStatus.SetStatusFlag(true, 1, (int)AlienStatus.EStatus.CLAIM);

			Debug.Log("注文内容が合ってない");

			GetComponent<AlienClaim>().SetIsClaim(true);
		}

		// 各席への移動をする為の設定を行う
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// スクリプトを切る
				if (AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT))
				{
					Debug.Log("BoxCollider停止");
					GetComponents<BoxCollider>()[(int)AlienCall.ESeatPattern.COUNTERSEATS].enabled = false;
				}
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				if (AlienStatus.GetTakeOutStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT))
				{
					GetComponents<BoxCollider>()[(int)AlienCall.ESeatPattern.TAKEAWAYSEAT].enabled = false;
				}
				break;
			// エラー
			default: break;
		}
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().SetHitAlienID(setId);
        }
     
    }
}
