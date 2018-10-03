using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

/// <summary>
/// 電子レンジのゲージスクリプト
/// </summary>
public class MicroWaveGage : MonoBehaviour
{
	// ゲージのGUI
	private Slider slider;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		slider = GetComponent<Slider>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		slider.value += 0.01f;
	}

	public void Hoge()
	{
		Debug.Log(slider.value);
	}
}
