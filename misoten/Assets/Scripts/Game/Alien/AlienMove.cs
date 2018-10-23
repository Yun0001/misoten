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
	/// 終点座標の種類
	/// </summary>
	private enum EEndPositionPattern
	{
		PATTERN_1 = 0,	// 左端
		PATTERN_2,		// 左
		PATTERN_3,		// 中央
		PATTERN_4,		// 右
		PATTERN_5,		// 右端
		THEORDER		// 順番
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定終点座標(席に座る)
	[SerializeField]
	Vector3[,] endPosition = new Vector3[5, 3];

	// アニメーションのカーブ進行率設定
	[SerializeField]
	AnimationCurve curve;

	// 終点座標に行くまでの時間指定
	[SerializeField]
	private float[] endPositionTime;

	// 終点座標の種類
	[SerializeField]
	private EEndPositionPattern endPositionPattern;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// 開始座標設定用
	private Vector3 startPosition;

	// 店を出ていくかの判定用
	private bool withdrawal = false;

	// 移動状態の判定用
	private bool isMove = false;

	// 終点座標ID
	private int setEndPositionId_1 = 0;
	private int setEndPositionId_2 = 0;

	// 時間更新用
	private float[] timeAdd = { 0.0f, 0.0f, 0.0f };

	// 予定時間を割る用
	private float rate = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 終点座標設定
		EndPositionInit();

		// エイリアンがどの席に向かうかの設定
		if (endPositionPattern != EEndPositionPattern.THEORDER)
		{
			// インスペクターで指定した席に向かう
			//SetEndPositionsID((int)endPositionPattern);
			setEndPositionId_1 = (int)endPositionPattern;
		}
		else
		{
			// 空いている席に座る
			//SetEndPositionsID(AlienCall.GetAddId());
			setEndPositionId_1 = AlienCall.GetIdSave();
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		if (!withdrawal)
		{
			//// 実際の経過時間を求める
			//var diff = Time.timeSinceLevelLoad - startTime[0];

			//if (diff > endPositionTime)
			//{
			//	// 入店時の移動状態「OFF」
			//	AlienStatus.SetStatusFlag(false, AlienCall.GetIdSave(), (int)AlienStatus.EStatus.WALK);

			//	// 着席状態「ON」
			//	AlienStatus.SetStatusFlag(true, AlienCall.GetIdSave(), (int)AlienStatus.EStatus.GETON);

			//	// 移動状態終了する
			//	isMove = true;

			//	// スクリプトを切る
			//	enabled = false;
			//}

			//// 予定時間を割る
			//var rate = diff / endPositionTime;
			//var curvePos = curve.Evaluate(rate);

			//// カーブの位置を照らし合わせる
			//transform.position = Vector3.Lerp(startPosition, endPosition[setEndPositionId_1, 0], curvePos);

			//// 下に行けば行くほど手前になる
			//if (transform.position.y >= 5.0f) { transform.position += new Vector3(0.0f, 0.0f, -0.1f); }
			//else if (transform.position.y >= 4.0f) { transform.position += new Vector3(0.0f, 0.0f, -0.2f); }
			//else if (transform.position.y >= 3.5f) { transform.position += new Vector3(0.0f, 0.0f, -0.3f); }

			//// 実際の経過時間を求める
			//var diff = Time.timeSinceLevelLoad - startTime[0];

			//if (diff > endPositionTime)
			//{
			//	// 入店時の移動状態「OFF」
			//	AlienStatus.SetStatusFlag(false, AlienCall.GetIdSave(), (int)AlienStatus.EStatus.WALK);

			//	// 着席状態「ON」
			//	AlienStatus.SetStatusFlag(true, AlienCall.GetIdSave(), (int)AlienStatus.EStatus.GETON);

			//	// 移動状態終了する
			//	isMove = true;

			//	// スクリプトを切る
			//	enabled = false;
			//}

			//// 予定時間を割る
			//var rate = diff / endPositionTime;
			//var curvePos = curve.Evaluate(rate);

			//if (diff >= 0.0f)
			//{
			//	transform.position = Vector3.Lerp(startPosition, endPosition[setEndPositionId_1, 0], curvePos);
			//}
			//else if (diff >= 1.0f)
			//{
			//	// カーブの位置を照らし合わせる
			//	transform.position = Vector3.Lerp(endPosition[setEndPositionId_1, 0], endPosition[setEndPositionId_1, 1], curvePos);
			//}
			//else if (diff >= 2.0f)
			//{
			//	// カーブの位置を照らし合わせる
			//	transform.position = Vector3.Lerp(endPosition[setEndPositionId_1, 1], endPosition[setEndPositionId_1, 2], curvePos);
			//}

			//// カーブの位置を照らし合わせる
			//transform.position = Vector3.Lerp(startPosition, endPosition[setEndPositionId_1, setEndPositionId_2], curvePos);

			//Position = transform.position;

			//Position.x += 0.1f * Mathf.Cos(rad);
			//Position.y += 0.1f * Mathf.Sin(rad);

			//transform.position = Position;

			//if (endPosition[setEndPositionId_1, 0] == transform.position)
			//{
			//	enabled = false;
			//}


			//transform.position.x += speed.x * Mathf.Cos(rad);
			//transform.position.y += speed.y * Mathf.Sin(rad);

			//			float dist = Vector3.Distance(endPosition[setEndPositionId_1, 0], transform.position);
			//			Debug.Log(dist);
			//			if (dist <= 0.0f)
			//			{

			//			}

			//			if(dist >= 1.0f)
			//			{
			//				rad = Mathf.Atan2(
			//endPosition[setEndPositionId_1, 0].y - transform.position.y,
			//endPosition[setEndPositionId_1, 0].x - transform.position.x);

			//				transform.position += new Vector3(0.1f * Mathf.Cos(rad), 0.1f * Mathf.Sin(rad), 0.0f);
			//			}

			// 時間更新
			timeAdd[setEndPositionId_2] += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd[setEndPositionId_2] / endPositionTime[setEndPositionId_2];

			// カウンター席に着席する管理
			switch (setEndPositionId_2)
			{
				case 0:
					// 一つ目の終点座標に到着
					if (timeAdd[0] > endPositionTime[0]) { setEndPositionId_2 = 1; }
					transform.position = Vector3.Lerp(new Vector3(0.0f, 5.0f, 0.0f), endPosition[setEndPositionId_1, 0], rate);
					break;
				case 1:
					// 二つ目の終点座標に到着
					if (timeAdd[1] > endPositionTime[1]) { setEndPositionId_2 = 2; }
					transform.position = Vector3.Lerp(endPosition[setEndPositionId_1, 0], endPosition[setEndPositionId_1, 1], rate);
					break;
				case 2:
					// 三つ目の終点座標に到着
					if (timeAdd[2] > endPositionTime[2])
					{
						// 入店時の移動状態「OFF」
						AlienStatus.SetStatusFlag(false, setEndPositionId_1, (int)AlienStatus.EStatus.WALK);

						// 着席状態「ON」
						AlienStatus.SetStatusFlag(true, setEndPositionId_1, (int)AlienStatus.EStatus.GETON);

						// 移動状態終了する
						isMove = true;

						// スクリプトを切る
						enabled = false;
					}
					transform.position = Vector3.Lerp(endPosition[setEndPositionId_1, 1], endPosition[setEndPositionId_1, 2], rate);
					break;
			}
		}

		// 移動終了時、BoxCollider2Dを「ON」にする
		if (GetMoveStatus()) { GetComponent<BoxCollider2D>().enabled = true; }
	}

	/// <summary>
	/// 移動時の終点座標初期化関数
	/// </summary>
	void EndPositionInit()
	{
		// コンポーネント取得
		alienCall = GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>();

		// 一つ目の終点座標の設定
		for (int i = 0; i < alienCall.GetSeatMax(); i++) { endPosition[i, 0] = new Vector3(0.0f, 4.5f, -0.1f); }

		// 二つ目の終点座標の設定
		endPosition[0, 1] = new Vector3(-6.0f, 4.5f, -0.1f);
		endPosition[1, 1] = new Vector3(-3.0f, 4.5f, -0.1f);
		endPosition[2, 1] = new Vector3(0.0f, 4.5f, -0.1f);
		endPosition[3, 1] = new Vector3(3.0f, 4.5f, -0.1f);
		endPosition[4, 1] = new Vector3(6.0f, 4.5f, -0.1f);

		// 三つ目の終点座標の設定
		endPosition[0, 2] = new Vector3(-6.0f, 3.5f, -0.2f);
		endPosition[1, 2] = new Vector3(-3.0f, 3.5f, -0.2f);
		endPosition[2, 2] = new Vector3(0.0f, 3.5f, -0.2f);
		endPosition[3, 2] = new Vector3(3.0f, 3.5f, -0.2f);
		endPosition[4, 2] = new Vector3(6.0f, 3.5f, -0.2f);

		// 移動状態の初期化
		isMove = false;
	}

	/// <summary>
	/// オブジェクトが選択されている時
	/// </summary>
	void OnDrawGizmosSelected()
	{
//#if UNITY_EDITOR

//		if (!UnityEditor.EditorApplication.isPlaying || !enabled)
//		{
//			startPosition = transform.position;
//		}

//		UnityEditor.Handles.Label(endPosition[setEndPositionId_1, setEndPositionId_2], endPosition.ToString());
//		UnityEditor.Handles.Label(startPosition, startPosition.ToString());
//#endif
//		// 開始座標と終点座標のポイントを描画
//		Gizmos.DrawSphere(endPosition[setEndPositionId_1, setEndPositionId_2], 0.1f);
//		Gizmos.DrawSphere(startPosition, 0.1f);

//		// 開始座標と終点座標の間の線を描画
//		Gizmos.DrawLine(startPosition, endPosition[setEndPositionId_1, setEndPositionId_2]);
	}

	/// <summary>
	/// 店を出ていくかの状態を格納
	/// </summary>
	/// <param name="_withdrawal"></param>
	/// <returns></returns>
	public bool SetWithdrawal(bool _withdrawal) => withdrawal = _withdrawal;

	/// <summary>
	/// 店を出ていくかの状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetWithdrawal() => withdrawal;

	/// <summary>
	/// 移動状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetMoveStatus() => isMove;
}