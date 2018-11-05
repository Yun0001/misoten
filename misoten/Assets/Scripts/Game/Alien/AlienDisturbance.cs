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

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エイリアンのオーダー
	private AlienOrder alienOrder;

	// エイリアンのチップ
	private AlienChip alienChip;

	// エイリアン時間制限
	private AlienTimeLimit[,] alienTimeLimit = new AlienTimeLimit[7, (int)AlienCall.ESeatPattern.MAX];

	private TimeLimitDraw[,] timeLimitDraw = new TimeLimitDraw[7, (int)AlienCall.ESeatPattern.MAX];

	// エイリアンの呼び出し
	private AlienCall alienCall;

	// エイリアンの機嫌
	private EAlienMood[,] mood = new EAlienMood[7, (int)AlienCall.ESeatPattern.MAX];

	// 席の種類保存用
	private int seatPatternSave = 0;

	// エイリアン毎のID
	private int setId = 0;

	// 待ち時間の加算
	private float[,] latencyAdd = new float[7, (int)AlienCall.ESeatPattern.MAX];

	private bool parent = true;

	private Vector3 setPos;

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

		// 座る席の種類保存
		seatPatternSave = alienCall.GetSeatPattern();

		// エイリアンが座る席のパターン管理
		switch (seatPatternSave)
		{
			// チップIDへの受け渡し
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				setId = AlienCall.GetIdSave(seatPatternSave);
				alienTimeLimit[setId, seatPatternSave] = GetComponent<AlienTimeLimit>();
				mood[setId, (int)AlienCall.ESeatPattern.COUNTERSEATS] = EAlienMood.NORMAL;
				latencyAdd[setId, (int)AlienCall.ESeatPattern.COUNTERSEATS] = AlienCall.GetOrderLatencyAdd1(AlienCall.GetRichDegreeId((int)AlienCall.ESeatPattern.COUNTERSEATS));
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				setId = AlienCall.GetIdSave(seatPatternSave);
				alienTimeLimit[setId, seatPatternSave] = GetComponent<AlienTimeLimit>();
				mood[setId, (int)AlienCall.ESeatPattern.TAKEAWAYSEAT] = EAlienMood.NORMAL;
				latencyAdd[setId, (int)AlienCall.ESeatPattern.TAKEAWAYSEAT] = AlienCall.GetOrderLatencyAdd2(AlienCall.GetRichDegreeId((int)AlienCall.ESeatPattern.TAKEAWAYSEAT));
				break;
			default: break;
		}
		parent = true;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// エイリアンが座る席のパターン管理
		switch (seatPatternSave)
		{
			case (int)AlienCall.ESeatPattern.COUNTERSEATS:
				// エイリアンが注文している時
				if (alienOrder.GetIsOrder(seatPatternSave))
				{
					Mood((int)AlienCall.ESeatPattern.COUNTERSEATS);
				}
				break;
			case (int)AlienCall.ESeatPattern.TAKEAWAYSEAT:
				// エイリアンが注文している時
				if (alienOrder.GetIsOrder(seatPatternSave))
				{
					Mood((int)AlienCall.ESeatPattern.TAKEAWAYSEAT);
				}
				break;
			default: break;
		}
	}

	/// <summary>
	/// エイリアンのモード
	/// </summary>
	/// <param name="pattern"></param>
	void Mood(int pattern)
	{
		// エイリアンの機嫌管理
		switch (mood[setId, pattern])
		{
			case EAlienMood.NORMAL: // 通常状態

				// 毎フレームの時間を加算
				latencyAdd[setId, pattern] -= Time.deltaTime;

				if (latencyAdd[setId, pattern] <= 0.0f)
				{
					mood[setId, pattern] = EAlienMood.ANGER;
					break;
				}

				//if (parent)
				//{
				//	alienTimeLimit[setId, pattern].TextParent();
				//	parent = !parent;
				//	timeLimitDraw[setId, pattern] = GameObject.Find(transform.name + "TimeLimitTextObj(Clone)/Canvas/TimeLimitText").gameObject.GetComponent<TimeLimitDraw>();
				//}

				//timeLimitDraw[setId, pattern].TimeLimit((int)latencyAdd[setId, pattern], transform.position);

				// 料理が来ていない時
				if (!alienChip.GetCuisineCame())
				{

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
}
