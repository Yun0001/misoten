using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポーズ中のセレクトカーソル制御スクリプト
/// </summary>
public class SelectCursor : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// ポーズオブジェクト設定用
	[SerializeField]
	private GameObject pauseObj;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 拡縮(Z軸)
	private float scaleZ = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 拡縮(Z軸)の設定
		scaleZ = 0.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if(pauseObj.GetComponent<PauseScreen>().GetSelectCursorFlag())
		{
			if(scaleZ < 6.0f) { transform.localScale = new Vector3(27.0f, 1.0f, scaleZ += 1.0f); }
			else { pauseObj.GetComponent<PauseScreen>().SetSelectCursorFlag(false); }
		}
		else { scaleZ = 0.0f; }
	}
}
