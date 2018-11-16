using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンが厨房に向けて邪魔な行為をするスクリプト
/// </summary>
public class AlienDisturbance : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンの機嫌
	/// </summary>
	private enum EAlienMood
	{
		NORMAL = 0, // 通常状態
		ANGER,		// 怒り状態
		FAVORABLE,	// 良好状態
		MAX			// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// Prefabを指定して、そのPrefabを生成する為の物
	[SerializeField]
	GameObject prefab;

	// エイリアンの機嫌が変わるまでの時間を設定
	[SerializeField]
	private float moodChangeTime;

	// テキストメッシュ描画用
	[SerializeField]
	GameObject TextMeshBalloon;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エイリアンのオーダー
	private AlienOrder alienOrder;

	// エイリアンのチップ
	private AlienChip alienChip;

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// エイリアンのタイムリミット用
	private GameObject[] timeLimitDraw = new GameObject[7];

	// エイリアンの機嫌
	private EAlienMood[] mood = new EAlienMood[7];

	// テキストメッシュフラグ
	private bool textMeshFlag = true;

	// エイリアン毎のID
	private int setId = 0;

	// エイリアンの残り時間
	private int timeFont = 0;

	// 待ち時間の加算
	private float[] latencyAdd = new float[7];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();
		alienChip = GetComponent<AlienChip>();
		alienCall = GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>();

		setId = AlienCall.GetIdSave();
		timeLimitDraw[setId] = gameObject.transform.Find("TextMesh").gameObject;
		mood[setId] = EAlienMood.NORMAL;
		latencyAdd[setId] = AlienCall.GetOrderLatencyAdd(AlienCall.GetRichDegreeId());

		// エイリアンの残り時間
		timeFont = 0;
}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// エイリアンが注文している時
		if (alienOrder.GetIsOrder()) { Mood(); }
	}

	/// <summary>
	/// エイリアンのモード関数
	/// </summary>
	void Mood()
	{
		// エイリアンの機嫌管理
		switch (mood[setId])
		{
			case EAlienMood.NORMAL: // 通常状態

				// 毎フレームの時間を加算
				latencyAdd[setId] -= Time.deltaTime;

				// 残存時間の可視化準備完了
				if (textMeshFlag) { TextMeshBalloon.SetActive(true); textMeshFlag = !textMeshFlag; }

				// 残存時間更新
				timeFont = (int)latencyAdd[setId];

				// 残存時間が「-1」以下になると入る
				if(timeFont <= -1)
				{
					// テキストメッシュが消える
					TextMeshBalloon.SetActive(false);

					// 怒り状態になる
					mood[setId] = EAlienMood.ANGER;
				}

				// クレームor満足状態になった時
				if(GetComponent<AlienClaim>().GetIsClaim() || GetComponent<AlienSatisfaction>().GetSatisfactionFlag())
				{
					// テキストメッシュが消える
					TextMeshBalloon.SetActive(false);
				}
				break;
			case EAlienMood.ANGER:  // 怒り状態
				// エイリアンの種類管理
				switch (alienCall.GetAlienPattern(setId))
				{
					case 0: // 火星人の場合

						// 火星人特有の邪魔行為
						//MartianDisturbance();
						break;
					case 1: // 水星人の場合

						// 水星人特有の邪魔行為
						//MercuryDisturbance();
						break;
					case 2:// 金星人の場合

						// 金星人特有の邪魔行為
						//VenusianDisturbance();
						break;
					default:
						// 例外処理
						break;
				}

				// 退店時の移動開始
				GetComponent<AlienMove>().SetWhenLeavingStoreFlag(true);
				break;
			case EAlienMood.FAVORABLE:  // 良好状態

				break;
			default:
				// 例外処理
				break;
		}
	}

	/// <summary>
	/// 火星人の邪魔行為
	/// </summary>
	void MartianDisturbance()
	{
		// Debug用
		Debug.Log("火星人の邪魔行動");

		// 5個ランダムな場所にcubeを生成する
		//for (int i = 0; i < 5; i++)
		//{
		//	// Instantiateの引数にPrefabを渡すことでインスタンスを生成する
		//	GameObject ball = Instantiate(prefab) as GameObject;

		//	// ランダムな場所に配置する
		//	ball.transform.position = new Vector3(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f), 0.0f);
		//}
	}

	/// <summary>
	/// 水星人の邪魔行為
	/// </summary>
	void MercuryDisturbance()
	{
		// Debug用
		Debug.Log("水星人の邪魔行動");
	}

	/// <summary>
	/// 金星人の邪魔行為
	/// </summary>
	void VenusianDisturbance()
	{
		// Debug用
		Debug.Log("金星人の邪魔行動");
	}

	/// <summary>
	/// 残存時間の取得
	/// </summary>
	/// <returns></returns>
	public int GetTimeFont() => timeFont;
}