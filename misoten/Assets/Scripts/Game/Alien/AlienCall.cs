using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンの呼び出しスクリプト
/// </summary>
public class AlienCall : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

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

	/// <summary>
	/// 処理の種類
	/// </summary>
	private enum EProcessingPattern
	{
		NORMAL = 0,	// 通常
		EXCEPTION,	// 例外
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 入店に遷移する秒数設定
	[SerializeField]
	private float inTime;

	// 生成するPrefab設定用
	[SerializeField]
	private GameObject[] prefab;

	// 生成されたPrefabの確認用
	[SerializeField]
	private GameObject[] obj;

	// 席の最大数指定
	[SerializeField, Range(1, 5)]
	private int seatMax;

	// エイリアン最大数指定
	[SerializeField, Range(1, 50)]
	private int alienMax;

	// 周期の設定(例外処理を発生させる為の物)
	[SerializeField]
	private float period;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// 例外フラグ
	private static bool exceptionFlag = false;

	// クレーム用ID
	private static int claimId = 0;

	// 金持ち度ID
	private static int richDegreeId = 0;

	// 席管理用ID
	private static int addId = 0;

	// エイリアンIDの保存用
	private static int idSave = 0;

	// オーダー待ち時間
	private static float[] orderLatencyAdd = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	// 座っているかの判定
	private static bool[] orSitting = { false, false, false, false, false };

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 金持ち度
	private ERichDegree[] richDegree = new ERichDegree[51];

	// エイリアンの種類設定
	private EAlienPattern[] alienPattern = new EAlienPattern[5];

	// エイリアン数
	private int alienNumber = 0;

	// 待ち時間の加算
	private float[] latencyAdd = { 0.0f, 0.0f };

	// ---------------------------------------------

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// 席が空いている場合
		if (!GetOrSitting(GetAddId()))
		{
			// 毎フレームの時間を加算
			latencyAdd[(int)EProcessingPattern.NORMAL] += Time.deltaTime;

			// エイリアン数が指定最大数体以下及び、呼び出し時間を超えた場合、エイリアンが出現する
			if (alienNumber < alienMax && latencyAdd[(int)EProcessingPattern.NORMAL] > inTime)
			{
				// エイリアン事の秒数設定処理
				TheNumberOfSecondsSet();

				// エイリアンの種類設定
				alienPattern[GetAddId()] = (EAlienPattern)Random.Range((int)EAlienPattern.MARTIAN, (int)EAlienPattern.MAX);

				// エイリアン生成
				obj[GetAddId()] = Instantiate(prefab[(int)alienPattern[GetAddId()]], new Vector3(0.0f, 5.0f, 0.0f), Quaternion.identity) as GameObject;
				obj[GetAddId()].transform.SetParent(transform);

				// 待機状態終了
				AlienStatus.SetStatusFlag(false, GetAddId(), (int)AlienStatus.EStatus.STAND);

				// 空いている席へ
				AlienStatus.SetStatusFlag(true, GetAddId(), (int)AlienStatus.EStatus.WALK);

				// その席が座られている状態にする
				SetOrSitting(true, GetAddId());

				// 時間初期化
				latencyAdd[(int)EProcessingPattern.NORMAL] = 0.0f;

				// エイリアン数の更新
				alienNumber++;

				// エイリアンIDの保存
				idSave = GetAddId();
			}
		}
		// 空いている席のIDになるまでこの処理を続ける
		else { SetAddId(Random.Range(0, 5)); }

		// 例外処理関数
		//ExceptionHandling();

		// クレーム終了時
		if(AlienClaim.GetClaimEndFlag() || AlienSatisfaction.GetClaimEndFlag())
		{
			// 各エイリアンが帰る処理
			Return();
		}
	}

	/// <summary>
	/// 各エイリアンの秒数設定関数
	/// </summary>
	void TheNumberOfSecondsSet()
	{
		// クレーム用IDと金持ち度IDの更新
		richDegreeId = GetAddId();

		// エイリアンの金持ち度をランダムで設定及び、客の秒数設定
		switch (Random.Range((int)ERichDegree.POVERTY, (int)ERichDegree.RAND))
		{
			case (int)ERichDegree.POVERTY: orderLatencyAdd[GetRichDegreeId()] = 15.0f; break;
			case (int)ERichDegree.NORMAL: orderLatencyAdd[GetRichDegreeId()] = 10.0f; break;
			case (int)ERichDegree.RICHMAN: orderLatencyAdd[GetRichDegreeId()] = 7.0f; break;
			default: Debug.Log("Error:客の秒数設定がされていません"); break;
		}
	}

	/// <summary>
	/// 各エイリアンが帰る関数
	/// </summary>
	void Return()
	{
		AlienClaim.SetClaimEndFlag(false);
		AlienSatisfaction.SetClaimEndFlag(false);

		// 席の数分ループする
		for (int i = 0; i < GetSeatMax(); i++)
		{
			//条件：座っている、チップをプレイヤーに渡したエイリアン
			if (AlienStatus.GetStatusFlag(i, (int)AlienStatus.EStatus.GETON) && AlienChip.GetChipOnFlag(i))
			{

				SetAddId(i);
				Destroy(obj[i]);
				SetOrSitting(false, i);
				AlienChip.SetChipOnFlag(false, i);

				// 時間初期化
				latencyAdd[(int)EProcessingPattern.NORMAL] = 0.0f;
			}
		}
	}

	/// <summary>
	/// 例外処理関数
	/// </summary>
	void ExceptionHandling()
	{
		// 例外処理用のフレーム更新
		latencyAdd[(int)EProcessingPattern.EXCEPTION] += Time.deltaTime;

		// 指定の一定間隔で例外処理発生
		if (latencyAdd[(int)EProcessingPattern.EXCEPTION] >= period)
		{
			// フレームの初期化(一定間隔で例外処理を発生させる為に)
			latencyAdd[(int)EProcessingPattern.EXCEPTION] = 0.0f;

			// Debug用
			Debug.Log("例外処理に入りました");
		}
	}

	/// <summary>
	/// 生成されたオブジェクトの取得
	/// </summary>
	/// <returns></returns>
	public GameObject GetObject(int id) => obj[id];

	/// <summary>
	/// 席の最大数の取得
	/// </summary>
	/// <returns></returns>
	public int GetSeatMax() => seatMax;

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
	/// 金持ち度IDの取得
	/// </summary>
	/// <returns></returns>
	public static int GetRichDegreeId() => richDegreeId;

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
	/// クレーム用IDの格納
	/// </summary>
	/// <param name="_claimId"></param>
	/// <returns></returns>
	public static int SetClaimId(int _claimId) => claimId = _claimId;

	/// <summary>
	/// クレーム用IDの取得
	/// </summary>
	/// <returns></returns>
	public static int GetClaimId() => claimId;

	/// <summary>
	/// エイリアンIDの保存用の取得
	/// </summary>
	/// <returns></returns>
	public static int GetIdSave() => idSave;

	/// <summary>
	/// オーダー待ち時間を取得
	/// </summary>
	/// <returns></returns>
	public static float GetOrderLatencyAdd(int id) => orderLatencyAdd[id];

	/// <summary>
	/// 席の状態を格納
	/// </summary>
	/// <param name="_is"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetOrSitting(bool _is, int id) => orSitting[id] = _is;

	/// <summary>
	/// 席の状態を取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetOrSitting(int id) => orSitting[id];
}
