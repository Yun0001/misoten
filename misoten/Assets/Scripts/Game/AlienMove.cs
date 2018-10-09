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
		RAND			// ランダム
	}

	// 指定終点座標
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

	// エイリアンがその席に座っているかの判定用
	private AlienOrSitting alienOrSitting;

	// 移動状態の判定用
	private bool isMove;

	// 終点座標ID
	private int setEndPositionID;

	// 開始時間設定用
	private float startTime;

	// 開始座標設定用
	private Vector3 startPosition;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 移動状態の初期化
		SetMoveStatus(true);

		// コンポーネント取得
		alienOrSitting = GameObject.Find("AlienOrSitting").GetComponent<AlienOrSitting>();

		// エイリアンがどの席に向かうかの設定
		if (endPositionPattern != EEndPositionPattern.RAND)
		{
			// インスペクターで指定した席に向かう
			SetEndPositionsID((int)endPositionPattern);
		}
		else
		{
			// 座れる席になるまで、ループする
			for (; ; )
			{
				// ランダムで席を決める
				SetEndPositionsID(Random.Range((int)EEndPositionPattern.PATTERN_1, (int)EEndPositionPattern.RAND));

				// 席の状態が座られていない時、ループを抜ける
				if (!AlienOrSitting.GetOrSitting(GetEndPositionsID())) { break; }
			}
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 実際の経過時間を求める
		var diff = Time.timeSinceLevelLoad - startTime;
		if (diff > endPositionTime)
		{
			transform.position = endPosition[GetEndPositionsID()];
			enabled = false;
			SetMoveStatus(false);
		}

		// 予定時間を割る
		var rate = diff / endPositionTime;
		var curvePos = curve.Evaluate(rate);

		// カーブの位置を照らし合わせる
		transform.position = Vector3.Lerp(startPosition, endPosition[GetEndPositionsID()], curvePos);

		// 下に行けば行くほど手前になる
		if (transform.position.y >= 5.0f) { transform.position += new Vector3(0.0f, 0.0f, 0.1f); }
		else if (transform.position.y >= 4.0f) { transform.position += new Vector3(0.0f, 0.0f, 0.2f); }
		else if (transform.position.y >= 3.0f) { transform.position += new Vector3(0.0f, 0.0f, 0.3f); }
	}

	/// <summary>
	/// アクティブな状態の時に呼び出される
	/// </summary>
	void OnEnable()
	{
		// 処理の開始時間を記録
		startTime = Time.timeSinceLevelLoad;
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
	/// 移動状態を格納
	/// </summary>
	/// <param name="_isMove"></param>
	/// <returns></returns>
	public bool SetMoveStatus(bool _isMove) => isMove = _isMove;

	/// <summary>
	/// 移動状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetMoveStatus() => isMove;

	/// <summary>
	/// 終点座標IDの格納
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public int SetEndPositionsID(int id) => setEndPositionID = id;

	/// <summary>
	/// 終点座標IDの取得
	/// </summary>
	/// <returns></returns>
	public int GetEndPositionsID() => setEndPositionID;
}