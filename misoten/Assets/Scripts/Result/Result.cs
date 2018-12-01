using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

/// <summary>
/// リザルトエイリアンの描画用スクリプト
/// </summary>
public class Result : MonoBehaviour
{
	// オブジェクトの種類列挙型
	// ---------------------------------------------

	/// <summary>
	/// オブジェクト種類
	/// </summary>
	private enum EObjectType
	{
		USERSCOREBOARD = 0,			// ユーザースコアボード
		DEVELOPMENTTEAMSCOREBOARD,	// 開発陣スコアボード
		ALIEN,						// リザルト用エイリアン
		HAPPYEND,					// ユーザースコアが開発陣スコアを上回った時用
		BADEND,						// ユーザースコアが開発陣スコアを下回った時用
		MAX							// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// オブジェクト取得用
	[SerializeField]
	private GameObject[] obj = new GameObject[(int)EObjectType.MAX];

	// オブジェクト描画開始時間
	[SerializeField]
	private float[] time = new float[(int)EObjectType.MAX];

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	private bool seFlag = true;
	private bool seFlag2 = true;

	// オブジェクト描画用フラグ
	private bool[] objDrawflag = new bool[(int)EObjectType.MAX];

	// 時間更新
	private float[] timeAdd = new float[(int)EObjectType.MAX];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 時間更新、オブジェクト描画用フラグの初期化
		for (int i = 0; i < (int)EObjectType.MAX; i++) { obj[0].SetActive(false); objDrawflag[i] = false; timeAdd[i] = 0.0f; }
		objDrawflag[0] = true;

		// リザルトから始めた時に呼ばれる
		if (!SoundController.Loadflg) { SoundController.SoundLoad(); }

		Sound.SetLoopFlgSe(SoundController.GetResultSEName(SoundController.ResultSE.register), false, 4);
		Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Result));
		seFlag = seFlag2 = true;
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// メニューの移動が終了すると、オブジェクトをアクティブ化
		if (MenuMove.GetResultAlienFlag())
		{
			if (seFlag) { seFlag = false; Sound.PlaySe(SoundController.GetResultSEName(SoundController.ResultSE.drumroll), 5); }
			for (int i = 0; i < (int)EObjectType.MAX; i++)
			{
				if (objDrawflag[i])
				{
					if(i < (int)EObjectType.MAX - 2)
					{
						if (timeAdd[i] >= time[i])
						{
							if (i <= 1) { Sound.PlaySe(SoundController.GetResultSEName(SoundController.ResultSE.register), 4); }
							if (i == 2) { Sound.PlaySe(SoundController.GetResultSEName(SoundController.ResultSE.applause), 0); }

							obj[i].SetActive(true);
							objDrawflag[i] = !objDrawflag[i];
							objDrawflag[i + 1] = !objDrawflag[i + 1];
						}
						else { timeAdd[i] += Time.deltaTime; }
					}
					else
					{
						if (ScoreCount.GetScore() >= 67650380)
						{
							if (obj[2].activeSelf)
							{
								if (timeAdd[3] >= time[3])
								{
									if (seFlag2) { seFlag2 = false; Sound.PlaySe(SoundController.GetResultSEName(SoundController.ResultSE.cracker_repeated), 0); }
									obj[3].SetActive(true); objDrawflag[3] = !objDrawflag[3]; objDrawflag[4] = !objDrawflag[4];
								}
								else { timeAdd[3] += Time.deltaTime; }
							}
						}
						else
						{
							if (obj[2].activeSelf)
							{
								if (timeAdd[3] >= time[3]) { obj[4].SetActive(true); objDrawflag[3] = !objDrawflag[3]; objDrawflag[4] = !objDrawflag[4]; }
								else { timeAdd[3] += Time.deltaTime; }
							}
						}

						if (i == 4)
						{
							if (obj[i - 1].activeSelf || obj[i].activeSelf)
							{
								if(GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any)
									|| GamePad.GetButtonDown(GamePad.Button.Y, GamePad.Index.Any) || GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
								{
									Sound.StopBgm(); SoundController.StopAllSE(); SceneManager.LoadScene("Title_heita", LoadSceneMode.Single);
								}

								if (timeAdd[i] >= time[i]) { Sound.StopBgm(); SoundController.StopAllSE(); SceneManager.LoadScene("Title_heita", LoadSceneMode.Single); }
								else { timeAdd[i] += Time.deltaTime; }
							}
						}
					}
				}
			}
		}
	}
}
