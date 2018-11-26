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

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// オブジェクト取得用
	private GameObject childObj;

	// ポーズを開いたプレイヤーのみ操作可能
	private bool[] id = new bool[4];

	// 選択時のカーソル
	private int selectCursor = 0;

	// ---------------------------------------------

	/// <summary>
	/// インスタンス化の時に呼ばれる
	/// </summary>
	void Start()
	{
		// 指定オブジェクト取得
		childObj = transform.Find("Canvas").gameObject;

		// 初期化処理
		Init();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// プレイヤーの数分ループ
		for(int i = 0; i < playerObj.Length; i++)
		{
			// スタートボタンが押されると、ポーズ画面になる
			if (!pauseObj.GetComponent<Pause>().pausing
				&& GamePad.GetButtonDown(GamePad.Button.Start, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()))
			{
				// ポーズ「ON」、ポーズを開いたプレイヤーのみ操作可能
				pauseObj.GetComponent<Pause>().pausing = id[i] = true;

				// ポーズオブジェクトを非アクティブ化に設定
				childObj.SetActive(true);
			}

			// ポーズ中の処理
			if (pauseObj.GetComponent<Pause>().pausing && id[i])
			{
				// セレクトカーソルの更新
				if (GamePad.GetAxis(GamePad.Axis.LeftStick, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()).y == 1.0f)
				{
					selectCursor = 0;
					selectCursorObj.transform.localPosition = new Vector3(0.0f, 30.0f, 0.1f);
				}
				if (GamePad.GetAxis(GamePad.Axis.LeftStick, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()).y == -1.0f)
				{
					selectCursor = 1;
					selectCursorObj.transform.localPosition = new Vector3(0.0f, -31.0f, 0.1f);
				}

				// 決定
				if (GamePad.GetButtonDown(GamePad.Button.B, playerObj[i].GetComponent<PlayerInput>().GetPlayerControllerNumber()))
				{
					// ポーズ中の項目
					switch(selectCursor)
					{
						case 0: pauseObj.GetComponent<Pause>().pausing = false; break;				// ゲーム画面へ戻る
						case 1: SceneManager.LoadScene("Title_heita", LoadSceneMode.Single); break;	// タイトルへ戻る
						default: Debug.LogError("だから言ったじゃないか！"); break;
					}

					// 初期化処理
					Init();
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
		childObj.SetActive(false);

		// 選択時のカーソルの初期化
		selectCursor = 0;

		// 初期位置保存
		selectCursorObj.transform.localPosition = new Vector3(0.0f, 30.0f, 0.1f);

		// ポーズを開いたプレイヤーのみ操作可能の初期化
		for (int i = 0; i < playerObj.Length; i++) { id[i] = false; }
	}
}
