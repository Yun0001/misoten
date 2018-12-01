using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

/// <summary>
/// ポーズ画面スクリプト
/// </summary>
public class PauseScreen : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// プレイヤーの設定
	[SerializeField]
	private GameObject[] playerObj = new GameObject[4];

	// セレクトカーソルの設定
	[SerializeField]
	private GameObject selectCursorObj;

	// ポーズマネージャーの設定
	[SerializeField]
	private GameObject pauseObj;

	// Fadeが開始された時の状態を取得する為に必要
	[SerializeField]
	private GameObject coinFoObj;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// オブジェクト取得用
	private GameObject childObj1, childObj2, childObj3;

	// ポーズを開いたプレイヤーのみ操作可能
	private bool[] id = new bool[4];

	// 選択時のカーソルフラグ
	private bool selectCursorFlag = false;

	// 選択時のカーソル
	private int selectCursor = 0;

	// ---------------------------------------------

	/// <summary>
	/// インスタンス化の時に呼ばれる
	/// </summary>
	void Start()
	{
		// 指定オブジェクト取得
		childObj1 = transform.Find("Canvas1").gameObject;
		childObj2 = transform.Find("Canvas2").gameObject;
		childObj3 = transform.Find("UIButton").gameObject;


		// 初期化処理
		Init();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// Fadeが開始されていない時
		if (!coinFoObj.GetComponent<CoinFO>().GetIsStartingCoinFO())
		{
			// プレイヤーの数分ループ
			for (int i = 0; i < playerObj.Length; i++)
			{
				// スタートボタンが押されると、ポーズ画面になる
				if (!pauseObj.GetComponent<Pause>().pausing
					&& GamePad.GetButtonDown(GamePad.Button.Start, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()))
				{
					// ポーズ「ON」、ポーズを開いたプレイヤーのみ操作可能
					pauseObj.GetComponent<Pause>().pausing = id[i] = true;

					// ポーズオブジェクトを非アクティブ化に設定
					childObj1.SetActive(true);
					childObj2.SetActive(true);
					childObj3.SetActive(true);
				}

				// ポーズ中の処理
				else if (pauseObj.GetComponent<Pause>().pausing && id[i])
				{
					// セレクトカーソルの更新
					if (GamePad.GetAxis(GamePad.Axis.LeftStick, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()).y == 1.0f && !selectCursorFlag && selectCursor == 1)
					{
						selectCursor = 0;
						selectCursorFlag = true;
						selectCursorObj.transform.localPosition = new Vector3(0.0f, 169.0f, 0.1f);
						Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.cursor), 22);
					}
					if (GamePad.GetAxis(GamePad.Axis.LeftStick, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()).y == -1.0f && !selectCursorFlag && selectCursor == 0)
					{
						selectCursor = 1;
						selectCursorFlag = true;
						selectCursorObj.transform.localPosition = new Vector3(0.0f, 98.0f, 0.1f);
						Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.cursor), 22);
					}

					// ゲーム画面へ戻る
					if (GamePad.GetButtonDown(GamePad.Button.Start, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber())
						|| GamePad.GetButtonDown(GamePad.Button.A, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()))
					{
						Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.Cancelkey_share), 22);
						pauseObj.GetComponent<Pause>().pausing = false;

						// 初期化処理
						Init();
					}

					// 決定
					if (GamePad.GetButtonDown(GamePad.Button.B, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()))
					{
						// ポーズ中の項目
						switch (selectCursor)
						{
							case 0:	// ゲーム画面へ戻る
								pauseObj.GetComponent<Pause>().pausing = false;
								Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.Cancelkey_share), 22);
								break;
							case 1: // タイトルへ戻る
								SoundController.StopAllSE();
								Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.decisionkey_share), 22);
								Sound.StopBgm();
								SceneManager.LoadScene("Title_heita", LoadSceneMode.Single);
								break;
							default: Debug.LogError("だから言ったじゃないか！"); break;
						}

						// 初期化処理
						Init();
					}
				}
			}
		}
	}

	/// <summary>
	/// 初期化関数
	/// </summary>
	void Init()
	{
		// ポーズオブジェクトを非アクティブ化に設定
		childObj1.SetActive(false);
		childObj2.SetActive(false);
		childObj3.SetActive(false);

		// 選択時のカーソルフラグの初期化
		selectCursorFlag = false;

		// 選択時のカーソルの初期化
		selectCursor = 0;

		// 初期位置保存
		selectCursorObj.transform.localPosition = new Vector3(0.0f, 169.0f, 0.1f);

		// ポーズを開いたプレイヤーのみ操作可能の初期化
		for (int i = 0; i < playerObj.Length; i++) { id[i] = false; }
	}

	/// <summary>
	/// 選択時のカーソルフラグの格納
	/// </summary>
	/// <param name="_selectCursorFlag"></param>
	public void SetSelectCursorFlag(bool _selectCursorFlag) => selectCursorFlag = _selectCursorFlag;

	/// <summary>
	/// 選択時のカーソルフラグの取得
	/// </summary>
	/// <returns></returns>
	public bool GetSelectCursorFlag() => selectCursorFlag;
}