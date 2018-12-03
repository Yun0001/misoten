using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーがミキサーにアクセス
/// </summary>
public class MixerAccess : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 終点座標設定
	[SerializeField]
	private Vector3 endPod;

	// ---------------------------------------------

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// ラインの終点座標
		GetComponent<LineRenderer>().SetPosition(1, endPod);

        LineRenderer line = GetComponent<LineRenderer>();
        // プレイヤーがミキサーにアクセスをしている場合
        if (transform.parent.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.MixerWait
			|| transform.parent.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.MixerAccess)
		{
            //Line
            // ラインの描画
            line.startWidth = 0.03f;
            line.endWidth = 0.03f;
		}
		// ミキサーにアクセスしていない時は、ラインを見えなくする
		else
        {
            line.startWidth = 0.0f;
            line.endWidth = 0.0f;
        }

		// ラインの始点座標を更新
		GetComponent<LineRenderer>().SetPosition(0, transform.parent.position);
	}
}
