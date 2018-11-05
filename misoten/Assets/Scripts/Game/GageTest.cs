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
	/// <summary>
	/// ゲージの種類
	/// </summary>
	private enum EGageType
	{
		SideGage = 0,
		CircleGage,
	};

	// ゲージの種類の列挙型変数
	[SerializeField] EGageType gageType;

	// ゲージ処理切り替え
	bool change = true;

	// ゲージのGUI
	private Slider slider;
	private Image image;

	// ゲージの進み具合
	[SerializeField] [Range(0.0f, 100.0f)]
	private float gageSpd;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		slider = GetComponent<Slider>();
		image = GetComponent<Image>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// ゲージ処理の切り替え管理
		switch (change)
		{
			case true:
				// ゲージの種類管理
				// ゲージが増えていく
				switch(gageType)
				{
					case EGageType.SideGage:
						if (slider.value < 100.0f) { slider.value += gageSpd; }
						else { change = false; }
						break;
					case EGageType.CircleGage:
						if (image.fillAmount < 1.0f) { image.fillAmount += gageSpd; }
						else { change = false; }
						break;
					default:
						break;
				}
				break;
			case false:
				// ゲージの種類管理
				// ゲージが減っていく
				switch (gageType)
				{
					case EGageType.SideGage:
						if (slider.value > 0.0f) { slider.value += -gageSpd; }
						else { change = true; }
						break;
					case EGageType.CircleGage:
						if (image.fillAmount > 0.0f) { image.fillAmount += -gageSpd; }
						else { change = true; }
						break;
					default:
						break;
				}
				break;
			default:
				break;
		}
	}

    public int GetSliderVal()
    {
        return (int)slider.value;
    }
}
