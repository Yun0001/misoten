﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキスト描画スクリプト
/// </summary>
public class TimeLimitDraw : MonoBehaviour
{
	/// <summary>
	/// 時間制限可視化関数
	/// </summary>
	public void TimeLimit(int timeFont, Vector3 pos)
	{
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
		GetComponent<Text>().text = timeFont.ToString("00");
	}
}