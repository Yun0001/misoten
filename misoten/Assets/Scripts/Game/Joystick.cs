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
	//実際に動くスティック部分
	[SerializeField]
	private GameObject stickObj;

	//スティックが動く範囲の半径
	[SerializeField, Range(0.0f, 100.0f)]
	private float radius;

	// プレイヤーID
	[SerializeField]
	private int playerId;

	// プレイヤー
	private Player player;

	// プレイヤーへの参照の為
	private string[] nameObj = { "char/Player1", "char/Player2", "char/Player3", "char/Player4" };

	// 画像の位置
	private Vector2 imagePos = Vector2.zero;

	// ラジアン
	private float radian;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		//スティックを生成する必要があれば生成し、位置を中心に設定
		stickObj.transform.localPosition = Vector3.zero;

		// コンポーネント取得
		player = GameObject.Find(nameObj[playerId]).gameObject.GetComponent<Player>();

		//スティックのImage取得(なければ追加)、タッチ判定を取られないようにraycastTargetをfalseに
		//Image stickImage = stickObj.GetComponent<Image>();
		//if (stickImage == null)
		//{
		//	stickImage = stickObj.AddComponent<Image>();
		//}
		//stickImage.raycastTarget = false;

		////タッチ判定を受け取れるようにRaycastTargetをTrueに
		//raycastTarget = true;

		////タッチ判定をとる範囲は表示されないように透明に
		//color = new Color(0, 0, 0, 0);
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// タップ位置を画面内の座標に変換し、スティックを移動
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
			new Vector2(Input.GetAxis(player.GetInputXAxisName()), -Input.GetAxis(player.GetInputYAxisName())), null, out imagePos);

		// 位置の更新
		stickObj.transform.localPosition = imagePos;

		// 移動場所が設定した半径を超えてる場合は制限内に抑える
		if (Vector3.Distance(Vector3.zero, stickObj.transform.localPosition) > radius)
		{
			// 角度計算
			float radian = Mathf.Atan2(stickObj.transform.localPosition.y, stickObj.transform.localPosition.x);

			// 円上にXとYを設定
			Vector3 limitedPosition = Vector3.zero;
			limitedPosition.x = radius * Mathf.Cos(radian);
			limitedPosition.y = radius * Mathf.Sin(radian);

			stickObj.transform.localPosition = limitedPosition;
		}

		//// 角度計算
		//float radian = Mathf.Atan2(Input.GetAxis(player.GetInputYAxisName()), Input.GetAxis(player.GetInputXAxisName()));
		//Vector3 limitedPosition = Vector3.zero;

		//if (Vector3.Distance(Vector3.zero, new Vector3(Input.GetAxis(player.GetInputYAxisName()) + 45.0f, Input.GetAxis(player.GetInputXAxisName()), 0.0f)) < radius)
		//{
		//	// 円上にXとYを設定
		//	limitedPosition.x = radius * Mathf.Sin(radian);
		//	limitedPosition.y = radius * Mathf.Cos(radian);

		//	stickObj.transform.localPosition = limitedPosition;
		//}

		//// 角度計算
		//radian = Mathf.Atan2(-Input.GetAxis(player.GetInputYAxisName()), Input.GetAxis(player.GetInputXAxisName()));
		//Debug.Log(radian);

		//if()
		//{

		//}
		//// 円上にXとYを設定
		//Vector3 limitedPosition = Vector3.zero;
		//limitedPosition.x = radius * Mathf.Sin(radian);
		//limitedPosition.y = radius * Mathf.Cos(radian);

		//stickObj.transform.localPosition = limitedPosition;
	}

		//オブジェクトと衝突している間に呼ばれ続けます
	void OnCollisionStay(Collision collision)
	{
		Debug.Log("Stay" + collision.collider.name);
	}
}