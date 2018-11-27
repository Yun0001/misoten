using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの満足スクリプト
/// </summary>
public class AlienSatisfaction : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 満足描画用
	[SerializeField]
	GameObject[] satisfactionBalloon = new GameObject[4];

	// 満足を行う時間
	[SerializeField, Range(1.0f, 10.0f)]
	private float judgeCount;

	// 満足度
	[SerializeField]
	private float[] satisfactionLevel = new float[4];

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 満足フラグ(チップ取得時用)
	private static bool satisfactionChipFlag = false;

	// 満足時間
	private static float satisfactionTimeAdd = 0.0f;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 満足フラグ
	private bool satisfactionFlag = false;

	// アニメーションフラグ
	private bool AnimationFlag = false;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 満足フラグの初期化
		satisfactionFlag = false;

		// 満足フラグ(チップ取得時用)の初期化
		satisfactionChipFlag = false;

		// アニメーションフラグの初期化
		AnimationFlag = false;

		// 満足時間の初期化
		satisfactionTimeAdd = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 満足した場合
		if(GetSatisfactionFlag())
		{
			// 状態移行フラグが「ON」の時
			if (GetComponent<AlienOrder>().GetStatusMoveFlag())
			{
				// 満足を指定ない時
				if (!AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD))
				{
					// イートイオブジェクトを削除
					Destroy(GetComponent<AlienOrder>().GetEatoyObj());

					// 満足時のSE
					Sound.PlaySe(GameSceneManager.seKey[4]);

					// 満足アニメーションになる
					GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.SATISFACTION);

					// 満足吹き出しの描画を行う
					BalloonDraw();

					// 満足フラグ(チップ取得時用)をON
					satisfactionChipFlag = true;
				}

				// 満足時間が指定時間を超えた場合
				if (satisfactionTimeAdd >= judgeCount)
				{
					// 時間の初期化
					satisfactionTimeAdd = 0.0f;

					// 帰る(良)状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD);

					// 退店時の移動開始
					GetComponent<AlienMove>().SetWhenLeavingStoreFlag(true);
				}

				// 毎フレームの時間を加算
				satisfactionTimeAdd += Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// 満足吹き出しの描画
	/// </summary>
	void BalloonDraw()
	{
		// 満足した吹き出しを出す
		if ((int)GetComponent<AlienChip>().GetCuisineCoefficient() <= 1) { satisfactionBalloon[0].SetActive(true); }
		if ((int)GetComponent<AlienChip>().GetCuisineCoefficient() <= 2) { satisfactionBalloon[1].SetActive(true); }
		if (3 <= (int)GetComponent<AlienChip>().GetCuisineCoefficient() && (int)GetComponent<AlienChip>().GetCuisineCoefficient() <= 15) { satisfactionBalloon[2].SetActive(true); }
		if (16 <= (int)GetComponent<AlienChip>().GetCuisineCoefficient() && (int)GetComponent<AlienChip>().GetCuisineCoefficient() <= 99999999) { satisfactionBalloon[3].SetActive(true); }
	}

	/// <summary>
	/// 満足フラグの格納
	/// </summary>
	/// <param name="_satisfactionFlag"></param>
	/// <returns></returns>
	public bool SetSatisfactionFlag(bool _satisfactionFlag) => satisfactionFlag = _satisfactionFlag;

	/// <summary>
	/// 満足フラグの取得
	/// </summary>
	/// <returns></returns>
	public bool GetSatisfactionFlag() => satisfactionFlag;

	/// <summary>
	/// 満足フラグ(チップ取得時用)の格納
	/// </summary>
	/// <param name="_satisfactionChipFlag"></param>
	/// <returns></returns>
	public static bool SetSatisfactionChipFlag(bool _satisfactionChipFlag) => satisfactionChipFlag = _satisfactionChipFlag;

	/// <summary>
	/// 満足フラグ(チップ取得時用)の取得
	/// </summary>
	/// <returns></returns>
	public static bool GetSatisfactionChipFlag() => satisfactionChipFlag;
}
