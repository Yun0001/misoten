using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ライン移動スクリプト
/// </summary>
public class LineMove : MonoBehaviour
{
	// ラインのID列挙型
	// ---------------------------------------------

	/// <summary>
	/// オブジェクト種類
	/// </summary>
	private enum ELine
	{
		LINE1 = 0,	// ライン(1)
		LINE2,		// ライン(2)
		MAX			// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// ラインID
	[SerializeField]
	private ELine line;

	// ラインの終点座標
	[SerializeField]
	private Vector3 endPos;

	// 更新値
	[SerializeField]
	private float updateValue;

	// ライン描画開始時間
	[SerializeField]
	private float time;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 時間更新
	private float timeAdd = 0.0f;

	// 座標取得用
	private Vector3 setPos;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 時間更新の初期化
		timeAdd = 0.0f;
	}


	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if (timeAdd >= time)
		{
			switch (line)
			{
				case ELine.LINE1:
					if (GetComponent<LineRenderer>().GetPosition(1).x >= endPos.x)
					{
						GetComponent<LineRenderer>().SetPosition(1, new Vector3(GetComponent<LineRenderer>().GetPosition(1).x + updateValue, 0.0f, 0.0f));
					}
					break;
				case ELine.LINE2:
					if (GetComponent<LineRenderer>().GetPosition(1).x <= endPos.x)
					{
						GetComponent<LineRenderer>().SetPosition(1, new Vector3(GetComponent<LineRenderer>().GetPosition(1).x + updateValue, 0.0f, 0.0f));
					}
					break;
				default: break;
			}

			// 座標の保存
			setPos = GetComponent<LineRenderer>().GetPosition(1);

			// 時間更新の初期化
			timeAdd = 0.0f;
		}
		else { timeAdd += Time.deltaTime; }
	}
	
	/// <summary>
	/// 座標の取得
	/// </summary>
	/// <returns></returns>
	public Vector3 GetSetPos() => setPos;
}
