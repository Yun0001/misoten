using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクト同士が衝突した場合、オブジェクトが削除されるスクリプト
/// </summary>
public class WhenDeletingCollision : MonoBehaviour
{
	/// <summary>
	/// 衝突時の削除されるオブジェクトの種類
	/// </summary>
	[SerializeField] private enum EDeleteType
	{
		OTHER = 0,
		OWN,
		BOTH
	}

	// 衝突時の削除されるオブジェクト選択用
	[SerializeField] private EDeleteType m_type;

	/// <summary>
	/// 開始関数
	/// </summary>
	private void Start ()
	{
		// 処理内容は特に無し
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	private void Update ()
	{
		// 処理内容は特に無し
	}

	/// <summary>
	/// 他のオブジェクトと衝突した時
	/// </summary>
	/// <param name="other">他のオブジェクト</param>
	void OnCollisionEnter(Collision other)
	{
		// オブジェクト削除方法の管理
		switch (m_type)
		{
			case EDeleteType.OTHER:
				// オブジェクト衝突時、衝突された側が削除される
				GameObject.Destroy(other.gameObject);
				break;
			case EDeleteType.OWN:
				// オブジェクト衝突時、衝突した側が削除される
				Destroy(gameObject);
				break;
			case EDeleteType.BOTH:
				// オブジェクト衝突時、どちらも削除される
				GameObject.Destroy(other.gameObject);
				Destroy(gameObject);
				break;
			default:
				break;
		}
	}
}
