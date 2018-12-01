using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// イライラフレームの制御スクリプト
/// </summary>
public class IraIraFrame : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 指定されたオブジェクトの位置を取得
	[SerializeField]
	private GameObject targetObj;

	// 移動速度
	[SerializeField, Range(0, 100.0f)]
	private float speed;

	// 確認用
	[SerializeField]
	private GameObject pot;

    [SerializeField]
    private GameObject pointText;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 拡縮設定
	private Vector3 scale;

	// 判回転したかのフラグ
	private bool halfRotationFlag = false;

	// 一回転したかのフラグ
	private bool oneRotationFlag = false;

	// エフェクトフラグ
	private bool effectFlag = false;

	// ヒットフラグ
	private bool hitFlag = false;

	// ---------------------------------------------

	private void Awake()
	{
		//pot = GameObject.Find("nabe1");
		effectFlag = hitFlag = false;
	}

	/// <summary>
	/// 衝突していない時
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerExit2D(Collider2D collision)
	{
		// Tagが「SecondHand」に設定されているオブジェクトのみ
		if (collision.tag == "SecondHand")
		{
			// 初期位置に戻してやる
			transform.localPosition = new Vector3(0.0f, 48.0f, 0.0f);

			// 初期角度に戻してやる
			transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

			// 判回転したかのフラグ初期化
			halfRotationFlag = false;

			// 一回転したかのフラグ初期化
			oneRotationFlag = false;

			pot.GetComponent<Pot>().AddMissCount();
			if (hitFlag) { effectFlag = true; }
		}
	}

	/// <summary>
	/// 衝突している間
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerStay2D(Collider2D collision)
	{
		// Tagが「SecondHand」に設定されているオブジェクトのみ
		if (collision.tag == "SecondHand")
		{
			// 回転中(一回転をしていない場合)
			if (!oneRotationFlag)
			{
				hitFlag = true;
				// イライラフレームの回転
				transform.RotateAround(targetObj.transform.position, new Vector3(0.0f, 0.0f, -1.0f), speed * Time.deltaTime);

				// 半回転判定
				if (!halfRotationFlag && transform.rotation.z >= 0.9f) { halfRotationFlag = true; Debug.Log("半回転した!"); }

				// 一回転判定
				if (halfRotationFlag && transform.rotation.z <= 0.0f) { oneRotationFlag = true; Debug.Log("一回転した!"); }
			}
		}
	}

	/// <summary>
	/// 拡縮の格納
	/// </summary>
	/// <param name="_scale"></param>
	public void SetScale(Vector3 _scale) => scale = _scale;

	/// <summary>
	/// 拡縮の取得
	/// </summary>
	/// <returns></returns>
	public Vector3 GetScale() => scale;

    public bool GetOneRotationFlag() => oneRotationFlag;
	public bool SetHitFlag(bool _hitFlag) => hitFlag = _hitFlag;
	public bool SetEffectFlag(bool _effectFlag) => effectFlag = _effectFlag;
	public bool GetEffectFlag() => effectFlag;
}