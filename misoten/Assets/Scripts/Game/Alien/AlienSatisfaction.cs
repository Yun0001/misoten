using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの満足スクリプト
/// </summary>
public class AlienSatisfaction : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンの向き
	/// </summary>
	public enum EHappyPoint
	{
		LOW = 0,
		MID,
		HIGH,
		OVER
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 満足描画用
	[SerializeField]
	GameObject[] satisfactionBalloon = new GameObject[4];

	// 満足を行う時間
	[SerializeField, Range(1.0f, 10.0f)]
	private float judgeCount;

	// 各満足時のポイント設定
	[SerializeField]
	private float[] happyPoint = new float[4];

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

	// イベントマネージャー
	private GameObject eventManager;

	// 満足フラグ
	private bool satisfactionFlag = false;

	// チップ取得フラグ
	private bool chipGetFlag = true;

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

		// チップ取得フラグの初期化
		chipGetFlag = true;

		// 満足時間の初期化
		satisfactionTimeAdd = 0.0f;
		SetEventManager();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 満足した場合
		if (GetSatisfactionFlag())
		{
			// 当たり判定が消える
			GetComponent<BoxCollider>().enabled = false;

			// 状態移行フラグが「ON」の時
			if (GetComponent<AlienOrder>().GetStatusMoveFlag())
			{
				// 満足を指定ない時
				if (!AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_GOOD))
				{
					// イートイオブジェクトを削除
					Destroy(GetComponent<AlienOrder>().GetEatoyObj());

					// 満足「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.SATISFACTION);

					// スコア取得
					if (chipGetFlag)
					{
						// 何度もスコアが加算されるのを防ぐ為
						chipGetFlag = false;

						// イベントエイリアンに正しい料理を渡した時の処理
						if (AlienCall.GetEventAlienCallFlag(GetComponent<AlienOrder>().GetSetId()))
						{
							Debug.Log("イベントエイリアンに正しい料理を渡した");
							SetEventManager();
							eventManager.GetComponent<EventManager>().StartEvent();
						}

						// 通常のエイリアンに正しい料理を渡した時の処理
						else
						{
							// 該当のエイリアンがフィーバー中かそうでないかの判定を行い、フィーバー中なら通常よりも多くのスコア、そうでないなら通常のスコアが貰える
							switch ((AlienCall.EAlienPattern)AlienCall.alienCall.GetAlienPattern(GetComponent<AlienOrder>().GetSetId()))
							{
								case AlienCall.EAlienPattern.MARTIAN:   // 火星人(赤)
									if (eventManager.GetComponent<EventManager>().GetNowPattern() == EventManager.FeverPattern.RedAlien) { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), true); }
									else { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), false); }
									break;
								case AlienCall.EAlienPattern.MERCURY:   // 水星人(青)
									if (eventManager.GetComponent<EventManager>().GetNowPattern() == EventManager.FeverPattern.BuleAlien) { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), true); }
									else { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), false); }
									break;
								case AlienCall.EAlienPattern.VENUSIAN:  // 金星人(黄)
									if (eventManager.GetComponent<EventManager>().GetNowPattern() == EventManager.FeverPattern.YellowAlien) { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), true); }
									else { ScoreManager.Instance.GetComponent<ScoreManager>().AddScore(GetComponent<AlienChip>().GetOpponentID(), GetComponent<AlienChip>().CalcChipValue(), false); }
									break;
								default: Debug.LogError("何かおかしいぞ?"); break;
							}
						}
					}

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

		// Fadeが開始された時
		if (AlienCall.alienCall.GetCoinFoObj().GetComponent<CoinFO>().GetIsStartingCoinFO())
		{
			for(int i = 0; i < 4; i++) { satisfactionBalloon[i].SetActive(false); }
		}
	}

	/// <summary>
	/// 満足吹き出しの描画
	/// </summary>
	void BalloonDraw()
	{
		// 満足した吹き出しを出す
		if ((int)GetComponent<AlienChip>().GetCuisineCoefficient() <= happyPoint[(int)EHappyPoint.LOW]) { satisfactionBalloon[0].SetActive(true); }
		if ((happyPoint[(int)EHappyPoint.LOW] + 1) <= (int)GetComponent<AlienChip>().GetCuisineCoefficient() && (int)GetComponent<AlienChip>().GetCuisineCoefficient() <= happyPoint[(int)EHappyPoint.MID]) { satisfactionBalloon[1].SetActive(true); }
		if ((happyPoint[(int)EHappyPoint.MID] + 1) <= (int)GetComponent<AlienChip>().GetCuisineCoefficient() && (int)GetComponent<AlienChip>().GetCuisineCoefficient() <= happyPoint[(int)EHappyPoint.HIGH]) { satisfactionBalloon[2].SetActive(true); }
		if ((int)GetComponent<AlienChip>().GetCuisineCoefficient() >= happyPoint[(int)EHappyPoint.OVER]) { satisfactionBalloon[3].SetActive(true); }
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

    private void SetEventManager() => eventManager = GameObject.Find("EventManager");
}
