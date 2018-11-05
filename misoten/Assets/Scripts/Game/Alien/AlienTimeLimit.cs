using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エイリアン時間制限の管理スクリプト
/// </summary>
public class AlienTimeLimit : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	// ローカル変数
	// ---------------------------------------------

	// オブジェクト保存用
	private GameObject textObj;
	private GameObject setObj;

	// ---------------------------------------------

	/// <summary>
	/// テキスト生成
	/// </summary>
	public void TextParent()
	{
		// テキスト生成
		textObj = Instantiate(prefab, new Vector3(), Quaternion.identity) as GameObject;
		textObj.transform.SetParent(transform);
	}
}