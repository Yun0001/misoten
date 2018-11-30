using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

/// <summary>
/// ジョイスティック
/// </summary>
public class Joystick : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	//実際に動くスティック部分
	[SerializeField]
	private GameObject stickObj;

	//スティックが動く範囲の半径
	[SerializeField, Range(0.0f, 100.0f)]
	private float radius;

	// プレイヤーID
	[SerializeField]
	private int playerId;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// プレイヤーへの参照の為
	private string[] nameObj = { "Players/Player1", "Players/Player2", "Players/Player3", "Players/Player4" };

	// 画像の位置
	private Vector2 imagePos = Vector2.zero;

	// プレイヤー
	private PlayerInput playerInput;

    private Vector3 pos;

    // ---------------------------------------------

    public void Init(int playerID)
    {
        playerInput = GameObject.Find(nameObj[playerID]).gameObject.GetComponent<PlayerInput>();
		PositionInit();
	}

    /// <summary>
    /// 更新関数
    /// </summary>
    void Update()
	{
        
        // 現在のワールド座標をスクリーン座標に変換
        Vector3 Position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), gameObject.transform.position, null, out Position);

        //スティックのベクトルに現在のスクリーン座標を加算して計算
        // スティックを動かすと座標変換される
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
			new Vector2(Input.GetAxis(playerInput.GetInputXAxisName())*100 + Position.x, -Input.GetAxis(playerInput.GetInputYAxisName())*100+ Position.y), null, out imagePos);


        // 位置の更新
        stickObj.transform.localPosition = imagePos;

        // 移動場所が設定した半径を超えてる場合は制限内に抑える
        if (Vector3.Distance(Vector3.zero, stickObj.transform.localPosition) > radius)
		{
			// 角度計算
			float radian = Mathf.Atan2(stickObj.transform.localPosition.y, stickObj.transform.localPosition.x);

			// 円上にXとYを設定
			stickObj.transform.localPosition = new Vector3(radius * Mathf.Cos(radian), radius * Mathf.Sin(radian), 0.0f);
		}
        
	}

	/// <summary>
	/// 位置初期化関数
	/// </summary>
	private void PositionInit()
	{
		stickObj.transform.localPosition = Vector3.zero;
	}
}