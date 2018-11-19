using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンのクレームスクリプト
/// </summary>
public class AlienClaim : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// クレーム描画用
	[SerializeField]
	GameObject[] claimBalloon;

	// クレームを行う時間
	[SerializeField, Range(1.0f, 10.0f)]
	float claimTime;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// エイリアンが一体でもクレームをすると、他のエイリアンの注文内容が見えなくなる用
	private static bool claimFlag = false;

	// クレーム時間
	private static float claimTimeAdd = 0.0f;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// クレーム中かの判定
	private bool isClaim = false;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 吹き出しを出さない
		claimBalloon[0].SetActive(false);
		claimBalloon[1].SetActive(false);

		// クレーム時間の初期化
		claimTimeAdd = 0.0f;

		// クレーム中かの判定の初期化
		isClaim = false;

		// エイリアンが一体でもクレームをすると、他のエイリアンの注文内容が見えなくなる用の初期化
		claimFlag = false;
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
                    Sound.PlaySe(GameSceneManager.seKey[2]);

					// 怒りアニメーションになる
					GetComponent<AlienAnimation>().SetIsCatering((int)AlienAnimation.EAlienAnimation.ANGER);

					// クレーム吹き出しを出す
					claimBalloon[0].SetActive(true);

					// エイリアンの注文内容が見えなくなる
					claimFlag = true;
				}

				// クレーム時間が指定時間を超えた場合
				if (GetClaimTimeAdd() >= claimTime)
				{
					// クレーム終了
					claimTimeAdd = 0.0f;
					claimFlag = false;
					SetIsClaim(false);

					// ビックリマーク吹き出しを出さない
					claimBalloon[1].SetActive(false);

					// 帰る(悪)状態「ON」
					AlienStatus.SetCounterStatusChangeFlag(true, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.RETURN_BAD);


                    for (int i = 0; i < GetComponent<AlienMove>().alienCall.GetCounterSeatsMax(); i++)
                    {
                        if (AlienStatus.GetCounterStatusChangeFlag(i, (int)AlienStatus.EStatus.RETURN_BAD))
                        {
                            Sound.SetLoopFlgSe(GameSceneManager.seKey[5], true, 8);
                            Sound.PlaySe(GameSceneManager.seKey[5], 8);
                            break;
                        }
                    }
                    // 退店時の移動開始
                    GetComponent<AlienMove>().SetWhenLeavingStoreFlag(true);
				}

				// 毎フレームの時間を加算
				claimTimeAdd += Time.deltaTime;
			}
		}
		else
		{
			// 状態移行フラグが「ON」の時
			if (GetComponent<AlienOrder>().GetStatusMoveFlag())
			{
				// エイリアンの注文内容が見えている状態の時
				if (!claimFlag)
				{
					// クレームを終えた場合は、クレーム用の吹き出しを消す
					claimBalloon[0].SetActive(false);
					claimBalloon[1].SetActive(false);
				}
				// エイリアンの注文内容が見えていない状態の時
				else
				{
					if (!GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag())
					{
						// ビックリマークをアクティブにする(吹き出し)
						claimBalloon[1].SetActive(true);
					}
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