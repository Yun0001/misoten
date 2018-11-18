using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ライン用のパーティクル移動スクリプト
/// </summary>
public class LineParticleMove : MonoBehaviour
{
	// ローカル変数
	// ---------------------------------------------

	// ライン移動スクリプト変数
	private LineMove lineMove;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		lineMove = GameObject.Find("Result/BadEnd/Line/" + transform.root.name).gameObject.GetComponent<LineMove>();
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		transform.localPosition = lineMove.GetSetPos();
	}
}
