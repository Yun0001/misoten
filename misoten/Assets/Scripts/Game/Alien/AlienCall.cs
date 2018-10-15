using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの呼び出しスクリプト
/// </summary>
public class AlienCall : MonoBehaviour
{
	/// <summary>
	/// エイリアンがお金を多く持っているかの種類
	/// </summary>
	 enum ERichDegree
	{
		POVERTY = 0,	// 貧乏
		NORMAL,			// 普通
		RICHMAN,		// 金持ち
		RAND			// ランダム
	}

	/// <summary>
	/// エイリアンの種類設定
	/// </summary>
	private enum EAlienPattern
	{
		MARTIAN,	// 火星人
		MERCURY,	// 水星人
		VENUSIAN,	// 金星人
		MAX			// 最大
	}

	// 入店に遷移する秒数設定
	[SerializeField]
	private float inTime;

	// 生成するPrefab設定用
	[SerializeField]
	private GameObject[] prefab;

	// 生成されたPrefabの確認用
	[SerializeField]
	private GameObject[] obj;

	// 金持ち度
	private ERichDegree[] richDegree = new ERichDegree[5];

	// エイリアンの種類設定
	private EAlienPattern[] alienPattern = new EAlienPattern[5];

	// 種類のID
	private int patternId = 0;

	// 待ち時間の加算
	private float latencyAdd = 0.0f;

	// オーダー待ち時間
	private float[] orderLatencyAdd = new float[5];

	// 座っているかの判定用
	private static bool[] seatID = { false, false, false, false, false };
	private static int[] seatID2 = { 0, 1, 2, 3, 4 };

	// 席管理用ID
	private static int addId = 0;
	private int addId2 = 0;

	// エイリアンの種類ID
	private static int richDegreeId = 0;

	/// <summary>
	/// エイリアン生成関数
	/// </summary>
	public void AlienForm()
	{
		// 席が空いている場合
		if (!GetSeat(GetAddId()))
		{
			// 毎フレームの時間を加算
			latencyAdd += Time.deltaTime;

			// 呼び出し時間を超えた場合、エイリアンが出現する
			if (latencyAdd > inTime)
			{
				// エイリアンの金持ち度をランダムで設定
				richDegree[richDegreeId] = (ERichDegree)Random.Range((int)ERichDegree.POVERTY, (int)ERichDegree.RAND);
				Debug.Log("入った");
				// エイリアンの種類設定
				alienPattern[GetAddId()] = (EAlienPattern)Random.Range((int)EAlienPattern.MARTIAN, (int)EAlienPattern.MAX);
				//alienPattern[patternId] = (EAlienPattern)richDegree;

				// エイリアン生成
				obj[GetAddId()] = Instantiate(prefab[(int)alienPattern[GetAddId()]], transform.position, Quaternion.identity) as GameObject;
				obj[GetAddId()].transform.SetParent(transform);

				// 待機状態終了
				//GetComponent<AlienStatus>().SetStatusFlag(false, (int)AlienStatus.EStatus.STAND);

				//// 客の秒数設定
				//switch ((int)richDegree)
				//{
				//	case (int)ERichDegree.POVERTY:
				//		orderLatencyAdd[patternId] = 15.0f;
				//		Debug.Log("貧乏");
				//		break;
				//	case (int)ERichDegree.NORMAL:
				//		orderLatencyAdd[patternId] = 10.0f;
				//		Debug.Log("普通");
				//		break;
				//	case (int)ERichDegree.RICHMAN:
				//		orderLatencyAdd[patternId] = 7.0f;
				//		Debug.Log("リッチマン");
				//		break;
				//	default:
				//		Debug.Log("客の秒数設定がされていません");
				//		break;
				//}

				// エイリアンの種類IDをチェック
				patternId++;

				// エイリアンの種類IDを更新
				richDegreeId++;

				// 時間初期化
				latencyAdd = 0.0f;

				// 空いている席へ
				SetSeat(true, GetAddId());
			}
		}

		for (int i = 0; i < 5; i++)
		{
			//条件：座っている、チップをプレイヤーに渡したエイリアン
			if (GetSeat(i) && AlienChip.GetChipOnFlag(i))
			{
				switch (i)
				{
					case 0: Destroy(obj[4]); SetSeat(false, 4); AlienChip.SetChipOnFlag(false, 0); addId = 4; break;
					case 1: Destroy(obj[0]); SetSeat(false, 0); AlienChip.SetChipOnFlag(false, 1); addId = 0; break;
					case 2: Destroy(obj[1]); SetSeat(false, 1); AlienChip.SetChipOnFlag(false, 2); addId = 1; break;
					case 3: Destroy(obj[2]); SetSeat(false, 2); AlienChip.SetChipOnFlag(false, 3); addId = 2; break;
					case 4: Destroy(obj[3]); SetSeat(false, 3); AlienChip.SetChipOnFlag(false, 4); addId = 3; break;
					default:break;
				}

				richDegreeId = 0;
				latencyAdd = 0.0f;
			}
		}
	}

	/// <summary>
	/// 生成されたオブジェクトの取得
	/// </summary>
	/// <returns></returns>
	public GameObject GetObject(int id) => obj[id];

	/// <summary>
	/// エイリアンの金持ち度を取得
	/// </summary>
	/// <returns></returns>
	public int GetRichDegree(int id) => (int)richDegree[id];

	/// <summary>
	/// エイリアンの種類設定の取得
	/// </summary>
	/// <returns></returns>
	public int GetAlienPattern(int id) => (int)alienPattern[id];

	/// <summary>
	/// 座っているかの判定用の格納
	/// </summary>
	/// <param name="_seatID"></param>
	/// <returns></returns>
	public static bool SetSeat(bool _seatID, int id) => seatID[id] = _seatID;

	/// <summary>
	/// 座っているかの判定用の取得
	/// </summary>
	/// <returns></returns>
	public static bool GetSeat(int id) => seatID[id];
	public static int GetSeat2(int id) => seatID2[id];

	/// <summary>
	/// 席管理用IDの格納
	/// </summary>
	/// <returns></returns>
	public static int SetAddId(int _addId) => addId = _addId;

	/// <summary>
	/// 席管理用IDの取得
	/// </summary>
	/// <returns></returns>
	public static int GetAddId() => addId;

	/// <summary>
	/// オーダー待ち時間を取得
	/// </summary>
	/// <returns></returns>
	public float GetOrderLatencyAdd(int id) => orderLatencyAdd[id];

	/// <summary>
	/// エイリアンの種類IDを取得
	/// </summary>
	/// <returns></returns>
	public static int GetRichDegreeId() => richDegreeId;
}
