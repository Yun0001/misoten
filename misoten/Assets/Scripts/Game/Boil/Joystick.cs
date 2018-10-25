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
	private string[] nameObj = { "char/Player1", "char/Player2", "char/Player3", "char/Player4" };

	// 画像の位置
	private Vector2 imagePos = Vector2.zero;

	// プレイヤー
	private Player player;

    // ---------------------------------------------

    /// <summary>
    /// 開始関数
    /// </summary>
    void Start()
	{
		//スティックを生成する必要があれば生成し、位置を中心に設定
		stickObj.transform.localPosition = Vector3.zero;

		// コンポーネント取得
		//player = GameObject.Find(nameObj[playerId]).gameObject.GetComponent<Player>();
    }


    public void Init(int playerID)
    {
        player = GameObject.Find(nameObj[playerID]).gameObject.GetComponent<Player>();
    }

    /// <summary>
    /// 更新関数
    /// </summary>
    void Update()
	{
        //プレイヤーの上に表示
        Vector3 pos = player.transform.position;
        pos.y += 2;
        gameObject.transform.position = pos;

        // 現在のワールド座標をスクリーン座標に変換
        Vector3 Position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), gameObject.transform.position, null, out Position);

        //スティックのベクトルに現在のスクリーン座標を加算して計算
        // スティックを動かすと座標変換される
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
			new Vector2(Input.GetAxis(player.GetInputXAxisName()) + Position.x, -Input.GetAxis(player.GetInputYAxisName())+ Position.y), null, out imagePos);


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
}