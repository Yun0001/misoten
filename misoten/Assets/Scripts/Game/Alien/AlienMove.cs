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

	// アニメーションのカーブ進行率設定
	[SerializeField]
	AnimationCurve curve;

	// 終点座標に行くまでの時間指定
	[SerializeField]
	private float endPositionTime;

	// 終点座標の種類
	[SerializeField]
	private EEndPositionPattern endPositionPattern;

	private Vector3[,] endPosition2 = new Vector3[5, 5];

	// 移動状態の判定用
	private static bool isMove = false;

	// 終点座標ID
	private static int setEndPositionID;

	// 開始時間設定用
	private float[] startTime = new float[2];

	// 開始座標設定用
	private Vector3 startPosition;

	// 店を出ていくかの判定用
	private bool withdrawal = false;

	// 入店時状態取得用
	private bool walk = false;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		//endPosition2[0, 0] = new Vector3(0.0f, 4.0f, 0.0f);
		//endPosition2[0, 1] = new Vector3(6.0f, 4.0f, 0.0f);

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
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		if (!withdrawal)
		{
			// 実際の経過時間を求める
			var diff = Time.timeSinceLevelLoad - startTime[0];
			if (diff > endPositionTime)
			{
				//transform.position = endPosition[GetEndPositionsID()];
				//withdrawal = true;

				// AlienMoveのスクリプトを切る
				enabled = false;

				// 入店時の移動状態「OFF」
				walk = false;

				isMove = true;
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

		// 移動終了
		if(GetMoveStatus())
		{
			GetComponent<BoxCollider2D>().enabled = true;
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

	/// <summary>
	/// 入店時状態の取得
	/// </summary>
	/// <returns></returns>
	public bool GetWalk() => walk;
}