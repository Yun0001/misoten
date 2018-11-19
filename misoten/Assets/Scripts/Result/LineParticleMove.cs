using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ライン用のパーティクル移動スクリプト
/// </summary>
public class LineParticleMove : MonoBehaviour
{
	// ラインのID列挙型
	// ---------------------------------------------

	/// <summary>
	/// オブジェクト種類
	/// </summary>
	private enum ELine
	{
		LINE1 = 0,  // ライン(1)
		LINE2,      // ライン(2)
		MAX         // 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// ラインID
	[SerializeField]
	private ELine line;

	// ---------------------------------------------

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
		switch (line)
		{
			case ELine.LINE1:
				// コンポーネント取得
				lineMove = GameObject.Find("Result/BadEnd/Line/1").gameObject.GetComponent<LineMove>();
				break;
			case ELine.LINE2:
				// コンポーネント取得
				lineMove = GameObject.Find("Result/BadEnd/Line/2").gameObject.GetComponent<LineMove>();
				break;
			default: break;
		}
	}
	
	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		transform.localPosition = lineMove.GetSetPos();
	}
}
