using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがカウンター席に向かって移動を行うスクリプト
/// </summary>
public class AlienMove : MonoBehaviour
{
	// 指定時間
	[SerializeField, Range(0.0f, 10.0f)]
	float time;

	// 指定終点座標
	[SerializeField]
	Vector3 endPosition;

	// アニメーションのカーブ進行率設定
	[SerializeField]
	AnimationCurve curve;

	// 開始時間設定用
	private float startTime;

	// 開始座標設定用
	private Vector3 startPosition;

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 実際の経過時間を求める
		var diff = Time.timeSinceLevelLoad - startTime;
		if (diff > time)
		{
			transform.position = endPosition;
			enabled = false;
		}

		// 予定時間を割る
		var rate = diff / time;
		var curvePos = curve.Evaluate(rate);

		// カーブの位置を照らし合わせる
		transform.position = Vector3.Lerp(startPosition, endPosition, curvePos);
	}

	/// <summary>
	/// アクティブな状態の時に呼び出される
	/// </summary>
	void OnEnable()
	{
		if (time <= 0.0f)
		{
			transform.position = endPosition;
			enabled = false;
			return;
		}

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

		UnityEditor.Handles.Label(endPosition, endPosition.ToString());
		UnityEditor.Handles.Label(startPosition, startPosition.ToString());
#endif
		// 開始座標と終点座標のポイントを描画
		Gizmos.DrawSphere(endPosition, 0.1f);
		Gizmos.DrawSphere(startPosition, 0.1f);

		// 開始座標と終点座標の間の線を描画
		Gizmos.DrawLine(startPosition, endPosition);
	}
}