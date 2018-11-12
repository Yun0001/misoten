using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアのアニメーションスクリプト
/// </summary>
public class DoorAnimCtrl : MonoBehaviour
{
	// ローカル変数
	// ---------------------------------------------

	// 左右のドアのアニメーション
	private Animator doorRAnimator, doorLAnimator;

	// ドアのアニメーション管理用
	//private bool isOpen = false;
	//private bool openAnim = false;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		doorRAnimator = GameObject.Find("doorr").GetComponent<Animator>();
		doorLAnimator = GameObject.Find("doorl").GetComponent<Animator>();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// エイリアンが生成された時、ドアのアニメーションを行う
		if (AlienCall.GetdoorAnimationFlag()) { DoorToDrink(); }
		else { DoorToClose(); }
	}

	/// <summary>
	/// ドアを開ける関数
	/// </summary>
	public void DoorToDrink()
	{
		doorRAnimator.Play("doorr|ropen");
		doorLAnimator.Play("doorl|lopen");
	}

	/// <summary>
	/// ドアを閉める関数
	/// </summary>
	public void DoorToClose()
	{
		doorRAnimator.Play("doorr|rclose");
		doorLAnimator.Play("doorl|lclose");
	}
}
