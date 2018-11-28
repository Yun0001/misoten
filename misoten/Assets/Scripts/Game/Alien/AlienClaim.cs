using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンのクレームスクリプト
/// </summary>
public class AlienClaim : MonoBehaviour
{
	// クレームパターン列挙型
	// ---------------------------------------------

	/// <summary>
	/// クレームパターン
	/// </summary>
	private enum EClaimPattern
	{
		ANGER1 = 0,
		ANGER2,
		SURPRISE,
		MAX
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// クレーム描画用
	[SerializeField]
	GameObject[] claimBalloon = new GameObject[(int)EClaimPattern.MAX];

	// クレームを行う時間
	[SerializeField, Range(1.0f, 10.0f)]
	float claimTime;

	// オブジェクト描画開始時間
	[SerializeField]
	private float time;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// エイリアンが一体でもクレームをすると
	// 他のエイリアンの注文内容が見えなくなる用
	private static bool claimFlag = false;

	// クレーム時間
	private static float claimTimeAdd = 0.0f;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// クレーム中かの判定
	private bool isClaim = false;

	// アニメーションフラグ
	private bool AnimationFlag = false;

	// 切り替えフラグ
	private bool changeFlag = false;

	// 時間更新
	private float timeAdd = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 吹き出しを出さない
		for(int i = 0; i < (int)EClaimPattern.MAX; i++) { claimBalloon[i].SetActive(false); }

		// クレーム中かの判定の初期化
		isClaim = false;

		// アニメーションフラグの初期化
		AnimationFlag = false;

		// 切り替えフラグの初期化
		changeFlag = false;

		// 時間更新の初期化
		timeAdd = 0.0f;

		// エイリアンが一体でもクレームをすると
		// 他のエイリアンの注文内容が見えなくなる用の初期化
		claimFlag = false;

		// クレーム時間
		claimTimeAdd = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// クレーム状態の時
		if (GetIsClaim())
		{
			// 状態移行フラグが「ON」の時
			if(GetComponent<AlienOrder>().GetStatusMoveFlag())
			{
				// エイリアンの注文内容が見えている状態の時
				if (!claimFlag)
				{
					// イートイオブジェクトを削除
					Destroy(GetComponent<AlienOrder>().GetEatoyObj());

					// 怒りアニメーションになる
					GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.ANGER);

					// クレーム「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.CLAIM);

					// クレーム吹き出しを出す
					//claimBalloon[0].SetActive(true);
					AnimationFlag = true;

					// エイリアンの注文内容が見えなくなる
					claimFlag = true;
				}

				// クレームアニメーション
				if(AnimationFlag)
				{
					if (timeAdd >= time)
					{
						if (!changeFlag) { claimBalloon[0].SetActive(true); claimBalloon[1].SetActive(false); changeFlag = true; }
						else { claimBalloon[1].SetActive(true); claimBalloon[0].SetActive(false); changeFlag = false; }
						timeAdd = 0.0f;
					}
					else { timeAdd += Time.deltaTime; }
				}

				// クレーム時間が指定時間を超えた場合
				if (GetClaimTimeAdd() >= claimTime)
				{
					// クレーム終了
					claimTimeAdd = 0.0f;
					claimFlag = false;
					SetIsClaim(false);

					// ビックリマーク吹き出しを出さない
					claimBalloon[2].SetActive(false);

					// 帰る(悪)状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_BAD);

					//for (int i = 0; i < AlienCall.alienCall.GetCounterSeatsMax(); i++)
					//{
					//	if (AlienStatus.GetCounterStatusChangeFlag(i, (int)AlienStatus.EStatus.RETURN_BAD))
					//	{
					//		Sound.SetLoopFlgSe(GameSceneManager.seKey[5], true, 8);
					//		Sound.PlaySe(GameSceneManager.seKey[5], 8);
					//		break;
					//	}
					//}

					// 退店時の移動開始
					GetComponent<AlienMove>().SetWhenLeavingStoreFlag(true);
				}

				// 毎フレームの時間を加算
				claimTimeAdd += Time.deltaTime;
			}
		}
		else
		{
			// エイリアンの注文内容が見えている状態の時
			if (!claimFlag)
			{
				// 吹き出しを出さない
				if (!GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
				{
					for (int i = 0; i < (int)EClaimPattern.MAX; i++) { claimBalloon[i].SetActive(false); }
				}
			}
			else
			{
				if (!GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag())
				{
					// ビックリマークをアクティブにする(吹き出し)
					claimBalloon[2].SetActive(true);
				}
			}

			if(GetComponent<AlienMove>().GetWhenLeavingStoreFlag() && AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_BAD))
			{
				// クレームアニメーション
				if (AnimationFlag)
				{
					if (timeAdd >= time)
					{
						if (!changeFlag) { claimBalloon[0].SetActive(true); claimBalloon[1].SetActive(false); changeFlag = true; }
						else { claimBalloon[1].SetActive(true); claimBalloon[0].SetActive(false); changeFlag = false; }
						timeAdd = 0.0f;
					}
					else { timeAdd += Time.deltaTime; }
				}
			}
		}
	}

	/// <summary>
	/// クレーム状態を格納
	/// </summary>
	/// <param name="_isClaim"></param>
	/// <returns></returns>
	public bool SetIsClaim(bool _isClaim) => isClaim = _isClaim;

	/// <summary>
	/// クレーム状態を取得
	/// </summary>
	/// <returns></returns>
	public bool GetIsClaim() => isClaim;

	/// <summary>
	/// クレームフラグの取得
	/// </summary>
	/// <returns></returns>
	public static bool GetClaimFlag() => claimFlag;

	/// <summary>
	/// クレーム時間
	/// </summary>
	/// <returns></returns>
	public static float GetClaimTimeAdd() => claimTimeAdd;
}