using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミキサーアクセス時のエフェクトスクリプト
/// </summary>
public class MixerAccessEffect : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// オブジェクト描画開始時間
	[SerializeField]
	private float time;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エフェクト再生・停止フラグ
	private bool flag = false;

	// 時間更新
	private float timeAdd = 0.0f;

	// 予定時間を割る用
	private float rate = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 更新関数
	/// </summary>
	void Start ()
	{
		// エフェクト再生・停止フラグの初期化
		flag = false;

		// 時間更新の初期化
		timeAdd = 0.0f;

		// 予定時間を割る用の初期化
		rate = 0.0f;

		// 位置の初期化
		transform.position = transform.parent.GetComponent<LineRenderer>().GetPosition(0);
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// プレイヤーがミキサーにアクセスをしている場合
		if (transform.parent.parent.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.MixerWait
			|| transform.parent.parent.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.MixerAccess)
		{
			// エフェクト再生
			if (flag) { GetComponent<ParticleSystem>().Play(); flag = false; }

			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / time;

			// 再び、指定始点から終点に向かって移動を行うようにする
			if (timeAdd > time) { timeAdd = 0.0f; }

			// ラインの始点から終点向かってエフェクトが移動
			transform.position = Vector3.Lerp(transform.parent.GetComponent<LineRenderer>().GetPosition(0),
			transform.parent.GetComponent<LineRenderer>().GetPosition(1), rate);
		}
		else
		{
			// エフェクト停止
			GetComponent<ParticleSystem>().Stop();

			// ミキサーアクセス時にエフェクト再生を行う為
			flag = true;
		}
	}
}