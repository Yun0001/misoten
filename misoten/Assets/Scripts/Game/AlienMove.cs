using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがカウンター席に向かって移動を行うスクリプト
/// </summary>
public class AlienMove : MonoBehaviour
{
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

	// 指定終点座標(席に座る)
	[SerializeField]
	Vector3[] endPosition;

	// 指定終点座標(店から出ていく)
	[SerializeField]
	Vector3[] endPosition2;

	// アニメーションのカーブ進行率設定
	[SerializeField]
	AnimationCurve curve;

	// 終点座標に行くまでの時間指定
	[SerializeField]
	private float endPositionTime;

	// 終点座標の種類
	[SerializeField]
	private EEndPositionPattern endPositionPattern;

	// 待機時間設定
	[SerializeField]
	private float[] theWaitingTime = new float[3];

	// 移動状態の判定用
	private static bool isMove;

	// 終点座標ID
	private static int setEndPositionID;

	// 開始時間設定用
	private float[] startTime = new float[2];

	// 開始座標設定用
	private Vector3 startPosition;

	//初期化成功か
	private bool initSuccess = false;

	// 店を出ていくかの判定用
	private bool withdrawal = false;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 移動状態の初期化
		SetMoveStatus(true);

		// エイリアンがどの席に向かうかの設定
		if (endPositionPattern != EEndPositionPattern.THEORDER)
		{
			// インスペクターで指定した席に向かう
			SetEndPositionsID((int)endPositionPattern);
		}
		else
		{
			// 左から順番に座る
			SetEndPositionsID(AlienCall.GetAddId());

			// 席のIDを更新
			if (AlienCall.GetAddId() < 4) { AlienCall.SetAddId(AlienCall.GetAddId() + 1); }
			else { AlienCall.SetAddId(0); }
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		if (!withdrawal)
		{
			// 入店時の移動状態
			//GetComponent<AlienStatus>().SetStatusFlag(true, (int)AlienStatus.EStatus.WALK);

			// 実際の経過時間を求める
			var diff = Time.timeSinceLevelLoad - startTime[0];
			if (diff > endPositionTime)
			{
				transform.position = endPosition[GetEndPositionsID()];
				withdrawal = true;
				enabled = false;
				SetMoveStatus(false);
			}

			// 予定時間を割る
			var rate = diff / endPositionTime;
			var curvePos = curve.Evaluate(rate);

			// カーブの位置を照らし合わせる
			transform.position = Vector3.Lerp(startPosition, endPosition[GetEndPositionsID()], curvePos);

			// 下に行けば行くほど手前になる
			if (transform.position.y >= 5.0f) { transform.position += new Vector3(0.0f, 0.0f, -0.1f); }
			else if (transform.position.y >= 4.0f) { transform.position += new Vector3(0.0f, 0.0f, -0.2f); }
			else if (transform.position.y >= 3.5f) { transform.position += new Vector3(0.0f, 0.0f, -0.3f); }
		}
	}

	/// <summary>
	/// アクティブな状態の時に呼び出される
	/// </summary>
	void OnEnable()
	{
		// 処理の開始時間を記録
		if (!withdrawal) { startTime[0] = Time.timeSinceLevelLoad;  }
		//startTime[1] = Time.timeSinceLevelLoad;
		startPosition = transform.position;
	}

	/// <summary>
	/// オブジェクトが選択されている時
	/// </summary>
	void OnDrawGizmosSelected()
	{
#if UNITY_EDITOR

		if (!UnityEditor.EditorApplication.isPlaying || !enabled)
		{
			startPosition = transform.position;
		}

		UnityEditor.Handles.Label(endPosition[GetEndPositionsID()], endPosition.ToString());
		UnityEditor.Handles.Label(startPosition, startPosition.ToString());
#endif
		// 開始座標と終点座標のポイントを描画
		Gizmos.DrawSphere(endPosition[GetEndPositionsID()], 0.1f);
		Gizmos.DrawSphere(startPosition, 0.1f);

		// 開始座標と終点座標の間の線を描画
		Gizmos.DrawLine(startPosition, endPosition[GetEndPositionsID()]);
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
	/// 移動状態を格納
	/// </summary>
	/// <param name="_isMove"></param>
	/// <returns></returns>
	public static bool SetMoveStatus(bool _isMove) => isMove = _isMove;

	/// <summary>
	/// 移動状態を取得
	/// </summary>
	/// <returns></returns>
	public static bool GetMoveStatus() => isMove;

	/// <summary>
	/// 終点座標IDの格納
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static int SetEndPositionsID(int id) => setEndPositionID = id;

	/// <summary>
	/// 終点座標IDの取得
	/// </summary>
	/// <returns></returns>
	public static int GetEndPositionsID() => setEndPositionID;
}