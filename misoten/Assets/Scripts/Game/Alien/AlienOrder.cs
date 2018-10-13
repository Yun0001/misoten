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
		GRILLED = 0,	// 焼き
		SIMMER,			// 煮る
		MICROWAVE,		// 電子レンジ
		MAX				// 最大
	}

	// オーダー内容描画用
	[SerializeField]
	GameObject[] orderBalloon;

	// オーダーするまでの時間
	[SerializeField]
	private float orderTime;

	// 注文するまでの時間を測る
	private float timeAdd = 0.0f;

	// オーダー待ち時間の加算
	private float orderLatencyAdd = 0.0f;

	// オーダー中かの判定
	private bool isOrder = false;

	// 注文結果格納・取得用
	private static int orderType = 0;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{

	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エイリアンが席に座って、注文するまでの時間
		if (timeAdd >= orderTime)
		{
			// エイリアンが注文していない時
			if (!GetIsOrder())
			{
				// 注文したものをアクティブにする(吹き出し)
				orderBalloon[GetOrderType()].SetActive(true);

				// オーダー完了
				SetIsOrder(true);

				// エイリアンの注文結果を出す(焼き=>煮る=>レンチン)
				SetOrderType(GetOrderType() + 1);

				// 注文をループさせる為に「0」で初期化
				if ((GetOrderType() >= (int)EOrderType.MAX)) { SetOrderType(0); }
			}
		}
		else
		{
			// 毎フレームの時間を加算
			timeAdd += Time.deltaTime;
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
	public static int SetOrderType(int _orderType) => orderType = _orderType;

	/// <summary>
	/// 注文の種類を取得
	/// </summary>
	/// <returns></returns>
	public static int GetOrderType() => orderType;

    public void EatCuisine(GameObject cuisine)
    {
        if (orderType == cuisine.GetComponent<Food>().GetCategory())
        {
            GetComponent<AlienChip>().SetCuisineCoefficient(cuisine.GetComponent<Food>().GetQualityTaste());
            GetComponent<AlienChip>().SetOpponentID(cuisine.GetComponent<Food>().GetOwnershipPlayerID());
            GetComponent<AlienChip>().SetCuisineCame(true);
        }
        else
        {
            GetComponent<AlienChip>().SetCuisineCoefficient(0.1f);
            GetComponent<AlienChip>().SetOpponentID(cuisine.GetComponent<Food>().GetOwnershipPlayerID());
            GetComponent<AlienChip>().SetCuisineCame(true);
        }
    }
}
