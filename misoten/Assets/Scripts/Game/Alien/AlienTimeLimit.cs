using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアン時間制限の管理スクリプト
/// </summary>
public class AlienTimeLimit : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	// ローカル変数
	// ---------------------------------------------

	private GameObject obj;

	// ---------------------------------------------

	/// <summary>
	/// テキスト生成
	/// </summary>
	public void TextParent()
	{
		// テキスト生成
		obj = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.0f), Quaternion.identity) as GameObject;
		obj.transform.SetParent(transform);
	}
}