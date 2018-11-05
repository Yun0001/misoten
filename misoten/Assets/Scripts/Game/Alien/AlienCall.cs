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

	/// <summary>
	/// 席の種類
	/// 補足：ごめん、ここだけpublicにさせて...
	/// </summary>
	public enum ESeatPattern
	{
		COUNTERSEATS = 0,	// カウンター席
		TAKEAWAYSEAT,		// 持ち帰り用の席
		MAX					// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 入店に遷移する秒数設定
	[SerializeField]
	private float[] inAlienTime;

	// 生成するPrefab設定用
	[SerializeField]
	private GameObject[] prefab;

	// カウンター席の最大数指定
	[SerializeField, Range(1, 7)]
	private int counterSeatsMax;

	// 持ち帰り用の席の最大数指定
	[SerializeField, Range(1, 6)]
	private int takeAwaySeatMax;

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
	//private static bool exceptionFlag = false;

	// クレーム用ID(カウンター用&持ち帰り用)
	private static int[] claimId = new int[(int)ESeatPattern.MAX];

	// 金持ち度ID(カウンター用&持ち帰り用)
	private static int[] richDegreeId = new int[(int)ESeatPattern.MAX];

	// 席管理用ID(カウンター用&持ち帰り用)
	private static int[] seatAddId = new int[(int)ESeatPattern.MAX];

	// エイリアンIDの保存用(カウンター用&持ち帰り用)
	private static int[] idSave = new int[(int)ESeatPattern.MAX];

	// オーダー待ち時間(カウンター用)
	private static float[] orderLatencyAdd1 = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	// オーダー待ち時間(持ち帰り用)
	private static float[] orderLatencyAdd2 = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	// 座っているかの判定(カウンター用)
	private static bool[] orSitting1 = { false, false, false, false, false, false, false };

	// 座っているかの判定(持ち帰り用)
	private static bool[] orSitting2 = { false, false, false, false, false, false };

	// ドアのアニメーションフラグ
	private static bool doorAnimationFlag = true;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 金持ち度
	private ERichDegree[] richDegree = new ERichDegree[51];

	// エイリアンの種類設定
	private EAlienPattern[] alienPattern = new EAlienPattern[7];

	// 席の種類
	private ESeatPattern seatPattern = ESeatPattern.MAX;

	// カウンター専用オブジェ
	private GameObject[] counterDesignatedObj = new GameObject[7];

	// 持ち帰り用専用オブジェ
	private GameObject[] takeOutDesignatedObj = new GameObject[6];

	// スコアカウント
	private ScoreCount scoreCount;

	// タイムマネージャー
	private GameTimeManager gameTimeManager;

	// 最初にエイリアンが入ったかのフラグ
	private bool inAlienFlag = false;

	// エイリアン数
	private int alienNumber = 0;

	// 待ち時間の加算
	private float[] latencyAdd = { 0.0f, 0.0f };

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		scoreCount = GameObject.Find("Score/Canvas/Score_1").gameObject.GetComponent<ScoreCount>();
		gameTimeManager = GameObject.Find("Score/Canvas/GameTimeManager").gameObject.GetComponent<GameTimeManager>();

		inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 1.0f;
		inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 2.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// カウンター席専用処理
		CounterSeatsDesignated();

		// 持ち帰り用席専用処理
		TakeAwaySeatDesignated();

		// 時間設定処理
		InTimeConfiguration();

		// 例外処理関数
		//ExceptionHandling();

		// 各エイリアンが帰る(削除)処理
		Return();
	}

	/// <summary>
	/// カウンター席専用処理
	/// </summary>
	void CounterSeatsDesignated()
	{
		// カウンター席が空いている場合
		if (!GetOrSitting1(GetSeatAddId((int)ESeatPattern.COUNTERSEATS)))
		{
			// 毎フレームの時間を加算
			latencyAdd[(int)ESeatPattern.COUNTERSEATS] += Time.deltaTime;

			// チップが増える毎にエイリアンが入ってくる頻度が高くなる
			switch(scoreCount.GetScore())
			{
				case 300: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 4.5f; break;
				case 600: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 4.0f; break;
				case 900: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 3.5f; break;
				case 1200: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 3.0f; break;
				case 1500: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 2.5f; break;
				case 1800: inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 2.0f; break;
			}

			// エイリアン数が指定最大数体以下及び、呼び出し時間を超えた場合、エイリアンが出現する
			if (alienNumber < alienMax && latencyAdd[(int)ESeatPattern.COUNTERSEATS] > inAlienTime[(int)ESeatPattern.COUNTERSEATS])
			{
				// ドアのアニメーションを行う
				SetdoorAnimationFlag(true);

				// エイリアンの秒数設定処理
				TheNumberOfSecondsSet(ESeatPattern.COUNTERSEATS);

				// エイリアンの種類設定
				alienPattern[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)] = (EAlienPattern)Random.Range((int)EAlienPattern.MARTIAN, (int)EAlienPattern.MAX);

				// エイリアン生成
				counterDesignatedObj[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)] = Instantiate(prefab[(int)alienPattern[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)]], new Vector3(0.0f, 0.75f, 7.0f), Quaternion.identity) as GameObject;
				counterDesignatedObj[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)].transform.SetParent(transform);

				// 待機状態終了
				AlienStatus.SetCounterStatusChangeFlag(false, GetSeatAddId((int)ESeatPattern.COUNTERSEATS), (int)AlienStatus.EStatus.STAND);

				// 空いている席へ
				AlienStatus.SetCounterStatusChangeFlag(true, GetSeatAddId((int)ESeatPattern.COUNTERSEATS), (int)AlienStatus.EStatus.WALK);

				// その席が座られている状態にする
				//SetOrSitting1(true, GetCounterSeatsAddId());
				orSitting1[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)] = true;

				// 時間初期化
				latencyAdd[(int)ESeatPattern.COUNTERSEATS] = 0.0f;

				// エイリアン数の更新
				alienNumber++;

				// エイリアンIDの保存
				idSave[(int)ESeatPattern.COUNTERSEATS] = GetSeatAddId((int)ESeatPattern.COUNTERSEATS);
			}
		}
		// 空いている席のIDになるまでこの処理を続ける
		else { SetSeatAddId(Random.Range(0, GetCounterSeatsMax()), (int)ESeatPattern.COUNTERSEATS); }
	}

	/// <summary>
	/// 持ち帰り用席専用処理
	/// </summary>
	void TakeAwaySeatDesignated()
	{
		// 持ち帰り用の席が空いている場合
		if (!GetOrSitting2(GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)))
		{
			// 毎フレームの時間を加算
			latencyAdd[(int)ESeatPattern.TAKEAWAYSEAT] += Time.deltaTime;

			// チップが増える毎にエイリアンが入ってくる頻度が高くなる
			switch (scoreCount.GetScore())
			{
				case 300: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 5.5f; break;
				case 600: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 5.0f; break;
				case 900: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 4.5f; break;
				case 1200: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 4.0f; break;
				case 1500: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 3.5f; break;
				case 1800: inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 3.0f; break;
			}

			// エイリアン数が指定最大数体以下及び、呼び出し時間を超えた場合、エイリアンが出現する
			if (alienNumber < alienMax && latencyAdd[(int)ESeatPattern.TAKEAWAYSEAT] > inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT])
			{
				// ドアのアニメーションを行う
				SetdoorAnimationFlag(true);

				// エイリアンの秒数設定処理
				TheNumberOfSecondsSet(ESeatPattern.TAKEAWAYSEAT);

				// エイリアンの種類設定
				alienPattern[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)] = (EAlienPattern)Random.Range((int)EAlienPattern.MARTIAN, (int)EAlienPattern.MAX);

				// エイリアン生成
				takeOutDesignatedObj[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)] = Instantiate(prefab[(int)alienPattern[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)]], new Vector3(0.0f, 0.75f, 7.0f), Quaternion.identity) as GameObject;
				takeOutDesignatedObj[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)].transform.SetParent(transform);

				// 待機状態終了
				AlienStatus.SetTakeOutStatusChangeFlag(false, GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT), (int)AlienStatus.EStatus.STAND);

				// 空いている席へ
				AlienStatus.SetTakeOutStatusChangeFlag(true, GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT), (int)AlienStatus.EStatus.WALK);

				// その席が座られている状態にする
				//SetOrSitting2(true, GetTakeAwaySeatAddId());
				orSitting2[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)] = true;

				// 時間初期化
				latencyAdd[(int)ESeatPattern.TAKEAWAYSEAT] = 0.0f;

				// エイリアン数の更新
				alienNumber++;

				// エイリアンIDの保存
				idSave[(int)ESeatPattern.TAKEAWAYSEAT] = GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT);
			}
		}
		// 空いている席のIDになるまでこの処理を続ける
		else { SetSeatAddId(Random.Range(0, GetTakeAwaySeatMax()), (int)ESeatPattern.TAKEAWAYSEAT); }
	}

	/// <summary>
	/// 各エイリアンの秒数設定関数
	/// </summary>
	void TheNumberOfSecondsSet(ESeatPattern _seatPattern)
	{
		// カウンターor持ち帰りの設定
		seatPattern = _seatPattern;

		// カウンターor持ち帰りかの判断
		switch (seatPattern)
		{
			case ESeatPattern.COUNTERSEATS:
				// 金持ち度IDの更新
				richDegreeId[(int)ESeatPattern.COUNTERSEATS] = GetSeatAddId((int)ESeatPattern.COUNTERSEATS);

				// エイリアンの金持ち度をランダムで設定及び、客の秒数設定
				switch (Random.Range((int)ERichDegree.POVERTY, (int)ERichDegree.RAND))
				{
					case (int)ERichDegree.POVERTY: orderLatencyAdd1[GetRichDegreeId((int)ESeatPattern.COUNTERSEATS)] = 15.0f; break;
					case (int)ERichDegree.NORMAL: orderLatencyAdd1[GetRichDegreeId((int)ESeatPattern.COUNTERSEATS)] = 10.0f; break;
					case (int)ERichDegree.RICHMAN: orderLatencyAdd1[GetRichDegreeId((int)ESeatPattern.COUNTERSEATS)] = 7.0f; break;
					default: Debug.Log("Error:カウンター客の秒数設定がされていません"); break;
				}
				break;
			case ESeatPattern.TAKEAWAYSEAT:
				// 金持ち度IDの更新
				richDegreeId[(int)ESeatPattern.TAKEAWAYSEAT] = GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT);

				// エイリアンの金持ち度をランダムで設定及び、客の秒数設定
				switch (Random.Range((int)ERichDegree.POVERTY, (int)ERichDegree.RAND))
				{
					case (int)ERichDegree.POVERTY: orderLatencyAdd2[GetRichDegreeId((int)ESeatPattern.TAKEAWAYSEAT)] = 15.0f; break;
					case (int)ERichDegree.NORMAL: orderLatencyAdd2[GetRichDegreeId((int)ESeatPattern.TAKEAWAYSEAT)] = 10.0f; break;
					case (int)ERichDegree.RICHMAN: orderLatencyAdd2[GetRichDegreeId((int)ESeatPattern.TAKEAWAYSEAT)] = 7.0f; break;
					default: Debug.Log("Error:持ち帰り客の秒数設定がされていません"); break;
				}
				break;
			default: break;
		}
	}

	/// <summary>
	/// 各エイリアンが帰る関数
	/// </summary>
	void Return()
	{
		// カウンター席の数分ループする
		for (int i = 0; i < GetCounterSeatsMax(); i++)
		{
			// 退店が完了したかの判定をとる
			if (AlienMove.GetCounterClosedCompletion(i))
			{
				// エイリアン削除
				Destroy(counterDesignatedObj[i]);

				// 次のエイリアンが入店出来るようにする
				SetSeatAddId(i, (int)ESeatPattern.COUNTERSEATS);
				orSitting1[GetSeatAddId((int)ESeatPattern.COUNTERSEATS)] = false;

				// 次のエイリアンが退店出来るようにする
				AlienMove.SetCounterClosedCompletion(false, i);

				// 次のエイリアンがチップをプレイヤーに渡せるように初期化
				AlienChip.SetChipOnFlag(false, i);

				// 時間初期化
				latencyAdd[(int)ESeatPattern.COUNTERSEATS] = 0.0f;
			}
		}

		// 持ち帰り席の数分ループする
		for (int i = 0; i < GetTakeAwaySeatMax(); i++)
		{
			// 退店が完了したかの判定をとる
			if (AlienMove.GetTakeoutClosedCompletion(i))
			{
				// エイリアン削除
				Destroy(takeOutDesignatedObj[i]);

				// 次のエイリアンが入店出来るようにする
				SetSeatAddId(i, (int)ESeatPattern.TAKEAWAYSEAT);
				orSitting2[GetSeatAddId((int)ESeatPattern.TAKEAWAYSEAT)] = false;

				// 次のエイリアンが退店出来るようにする
				AlienMove.SetTakeoutClosedCompletion(false, i);

				// 次のエイリアンがチップをプレイヤーに渡せるように初期化
				AlienChip.SetChipOnFlag(false, i);

				// 時間初期化
				latencyAdd[(int)ESeatPattern.TAKEAWAYSEAT] = 0.0f;
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
	/// 時間設定関数
	/// </summary>
	void InTimeConfiguration()
	{
		// カウンターと持ち帰り用分ループ
		for (int i = 0; i < (int)ESeatPattern.MAX; i++)
		{
			// 現在の時間を管理
			// 残り時間が減っていく毎にエイリアンの入店インターバルが減っていく
			switch ((int)gameTimeManager.GetCountTime())
			{
				case 200: inAlienTime[i] -= 0.1f; break;
				case 180: inAlienTime[i] -= 0.2f; break;
				case 150: inAlienTime[i] -= 0.3f; break;
				case 120: inAlienTime[i] -= 0.4f; break;
				case 90: inAlienTime[i] -= 0.5f; break;
				case 60: inAlienTime[i] -= 0.6f; break;
				case 30: inAlienTime[i] -= 0.7f; break;
			}
		}

		// 指定した数分エイリアンが入店すると、入店時間が再設定される
		if (alienNumber >= 2 && !inAlienFlag)
		{
			inAlienFlag = !inAlienFlag;
			inAlienTime[(int)ESeatPattern.COUNTERSEATS] = 7.0f;
			inAlienTime[(int)ESeatPattern.TAKEAWAYSEAT] = 8.0f;
		}
	}

	/// <summary>
	/// 生成されたオブジェクトの取得
	/// </summary>
	/// <returns></returns>
	public GameObject GetCounterDesignatedObj(int id) => counterDesignatedObj[id];

	/// <summary>
	/// カウンター席の最大数の取得
	/// </summary>
	/// <returns></returns>
	public int GetCounterSeatsMax() => counterSeatsMax;

	/// <summary>
	/// 持ち帰り用の席の最大数の取得
	/// </summary>
	/// <returns></returns>
	public int GetTakeAwaySeatMax() => takeAwaySeatMax;

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
	/// 席の種類の取得
	/// </summary>
	/// <returns></returns>
	public int GetSeatPattern() => (int)seatPattern;

	/// <summary>
	/// 金持ち度ID(カウンター用&持ち帰り用)の取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int GetRichDegreeId(int seatId) => richDegreeId[seatId];

	/// <summary>
	/// 席管理用ID(カウンター用&持ち帰り用)の格納
	/// </summary>
	/// <param name="_seatAddId"></param>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int SetSeatAddId(int _seatAddId, int seatId) => seatAddId[seatId] = _seatAddId;

	/// <summary>
	/// 席管理用ID(カウンター用&持ち帰り用)の取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int GetSeatAddId(int seatId) => seatAddId[seatId];

	/// <summary>
	/// クレーム用ID(カウンター用&持ち帰り用)の格納
	/// </summary>
	/// <param name="_claimId"></param>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int SetClaimId(int _claimId, int seatId) => claimId[seatId] = _claimId;

	/// <summary>
	/// クレーム用ID(カウンター用&持ち帰り用)の取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int GetClaimId(int seatId) => claimId[seatId];

	/// <summary>
	/// エイリアンIDの保存用(カウンター用&持ち帰り用)の取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static int GetIdSave(int seatId) => idSave[seatId];

	/// <summary>
	/// オーダー待ち時間(カウンター用)を取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static float GetOrderLatencyAdd1(int seatId) => orderLatencyAdd1[seatId];

	/// <summary>
	/// 席の状態(カウンター用)を取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static bool GetOrSitting1(int seatId) => orSitting1[seatId];

	/// <summary>
	/// オーダー待ち時間(持ち帰り用)を取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static float GetOrderLatencyAdd2(int seatId) => orderLatencyAdd2[seatId];

	/// <summary>
	/// 席の状態(持ち帰り用)を取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static bool GetOrSitting2(int seatId) => orSitting2[seatId];

	/// <summary>
	/// ドアのアニメーションフラグを格納
	/// </summary>
	/// <param name="_doorAnimationFlag"></param>
	/// <returns></returns>
	public static bool SetdoorAnimationFlag(bool _doorAnimationFlag) => doorAnimationFlag = _doorAnimationFlag;

	/// <summary>
	/// ドアのアニメーションフラグを取得
	/// </summary>
	/// <returns></returns>
	public static bool GetdoorAnimationFlag() => doorAnimationFlag;
}