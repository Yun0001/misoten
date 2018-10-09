using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンのオーダースクリプト
/// </summary>
public class AlienOrder : MonoBehaviour
{
	/// <summary>
	/// 注文の種類
	/// </summary>
	private enum EOrderType
	{
		MICROWAVE = 0,	// 電子レンジ
		BOIL,			// ゆでる
		GRILLED,		// 焼き
		MAX				// 最大
	}

	// 待ち時間設定
	[SerializeField, Range(7.0f, 15.0f)]
	float latency;

	// オーダー内容描画用
	[SerializeField]
	GameObject[] orderBalloon;

	// オーダーするまでの時間
	[SerializeField]
	private float orderTime;

	// 注文するまでの時間を測る
	private float timeAdd = 0.0f;

	// エイリアン移動状態の取得の為に必要
	private AlienMove alienMove;

	// オーダー中かの判定
	private bool isOrder = false;

	// 注文結果格納用
	private int orderType;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		alienMove = GetComponent<AlienMove>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エイリアンが席に座っている状態の時
		if (!alienMove.GetMoveStatus())
		{
			// エイリアンが席に座って、注文するまでの時間
			if (timeAdd >= orderTime)
			{
				// エイリアンが注文していない時
				if (!GetIsOrder())
				{
					// エイリアンの注文結果を出す
					SetOrderType(Random.Range((int)EOrderType.MICROWAVE, (int)EOrderType.MAX));

					// 注文したものをアクティブにする(吹き出し)
					orderBalloon[GetOrderType()].SetActive(true);

					// オーダー完了
					SetIsOrder(true);
				}
			}
			else
			{
				// 毎フレームの時間を加算
				timeAdd += Time.deltaTime;
			}
		}
	}

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
	///  注文の種類を格納
	/// </summary>
	/// <param name="_orderType"></param>
	/// <returns></returns>
	public int SetOrderType(int _orderType) => orderType = _orderType;

	/// <summary>
	/// 注文の種類を取得
	/// </summary>
	/// <returns></returns>
	public int GetOrderType() => orderType;
}
