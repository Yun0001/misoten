using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameFinish : MonoBehaviour
{
	// オブジェクト描画開始時間
	[SerializeField]
	private float time;

	// 時間更新
	private float timeAdd = 0.0f;

	static public bool gameEndFlag = false;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		timeAdd = 0.0f;
		gameEndFlag = false;
		GetComponent<SpriteRenderer>().enabled = false;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if(GameTimeManager.uiGameFinishFlag)
		{
			GetComponent<SpriteRenderer>().enabled = true;
			if (timeAdd >= time)
			{
				gameEndFlag = true;
			}
			else { timeAdd += Time.deltaTime; }
		}
	}
}
