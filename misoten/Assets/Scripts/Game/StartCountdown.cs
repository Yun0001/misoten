using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GamepadInput;

/// <summary>
/// ゲーム開始時のカウントダウンスクリプト
/// </summary>
public class StartCountdown : MonoBehaviour
{
	[SerializeField] private Text textCountdown;
	[SerializeField] private Image imageMask;

	private bool one = true;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 文字列の初期化
		textCountdown.text = "";
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// スタートカウントダウン開始
		if (one)
		{
			StartCoroutine(CountdownCoroutine());
			one = false;
		}
	}

	IEnumerator CountdownCoroutine()
	{
		imageMask.gameObject.SetActive(true);
		textCountdown.gameObject.SetActive(true);

		textCountdown.text = "3";
		yield return new WaitForSeconds(1.0f);

		textCountdown.text = "2";
		yield return new WaitForSeconds(1.0f);

		textCountdown.text = "1";
		yield return new WaitForSeconds(1.0f);

		textCountdown.text = "GO!";
		yield return new WaitForSeconds(1.0f);

		textCountdown.text = "";
		textCountdown.gameObject.SetActive(false);
		imageMask.gameObject.SetActive(false);
	}
}