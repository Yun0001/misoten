using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMove : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定終点座標
	[SerializeField]
	private Vector3 endPos;

	// 移動時間
	[SerializeField]
	private float moveTime;

	// ---------------------------------------------

	// 終点座標ID
	private bool setEndPositionFlag = true;

	// 時間更新用
	private float timeAdd = 0.0f;

	// 予定時間を割る用
	private float rate = 0.0f;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// 終点座標IDの初期化
		setEndPositionFlag = true;

		// 時間更新用、予定時間を割る用の初期化
		timeAdd = rate = 0.0f;
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if(setEndPositionFlag)
		{
			// 時間更新
			timeAdd += Time.deltaTime;

			// 予定時間を割る
			rate = timeAdd / moveTime;

			// 終点座標に到着
			if (timeAdd > moveTime) { setEndPositionFlag = false; timeAdd = 0.0f; }
			transform.position = Vector3.Lerp(new Vector3(-6.5f, 19.5f, 0.0f), endPos, rate);
		}
	}
}
