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
	private float enterShop;

	// 生成するPrefab設定用
	[SerializeField]
	private GameObject[] prefab;

	// カウンター席の最大数指定
	[SerializeField, Range(1, 9)]
	private int counterSeatsMax;

	// エイリアン最大数指定
	[SerializeField, Range(1, 50)]
	private int alienMax;

	// 周期の設定(例外処理を発生させる為の物)
	[SerializeField]
	private float period;

	// 客の残存時間設定
	[SerializeField]
	private float[] theRemainingTime = new float[3];

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
	private static int seatAddId = 0;

	// エイリアンIDの保存用
	private static int idSave = 0;

	// オーダー待ち時間
	private static float[] orderLatencyAdd = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

	// 座っているかの判定(カウンター用)
	private static bool[] orSitting = { false, false, false, false, false, false, false, false, false };

	// ドアのアニメーションフラグ
	private static bool doorAnimationFlag = true;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 金持ち度
	private ERichDegree[] richDegree = new ERichDegree[51];

	// エイリアンの種類設定
	private EAlienPattern[] alienPattern = new EAlienPattern[9];

	// カウンター専用オブジェ
	private GameObject[] counterDesignatedObj = new GameObject[9];

	// スコアカウント
	private ScoreCount scoreCount;

	// タイムマネージャー
	private GameTimeManager gameTimeManager;

	// 最初にエイリアンが入ったかのフラグ
	private bool inAlienFlag = false;

	// エイリアン数
	private int alienNumber = 0;

	// 待ち時間の加算
	private float latencyAdd = 0.0f;

	// 例外処理用の時間
	private float exceptionTime = 0.0f;

	// 入店に遷移する秒数設定
	private float inAlienTime;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// コンポーネント取得
		scoreCount = GameObject.Find("Score/Canvas/Score_1").gameObject.GetComponent<ScoreCount>();
		gameTimeManager = GameObject.Find("TimeManager").gameObject.GetComponent<GameTimeManager>();

		// ゲームが開始してエイリアンが出てくる時間
		inAlienTime = 1.0f;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update()
	{
		// カウンター席専用処理
		CounterSeatsDesignated();

		// 例外処理関数
		ExceptionHandling();

		// 時間設定処理
		InTimeConfiguration();

		// 各エイリアンが帰る(削除)処理
		Return();
	}

	/// <summary>
	/// カウンター席専用処理
	/// </summary>
	void CounterSeatsDesignated()
	{
		// カウンター席が空いている場合
		if (!GetOrSitting(GetSeatAddId()))
		{
			// 毎フレームの時間を加算
			latencyAdd += Time.deltaTime;

			// チップが増える毎にエイリアンが入ってくる頻度が高くなる
			switch(scoreCount.GetScore())
			{
				case 300: inAlienTime -= 0.5f; break;
				case 600: inAlienTime -= 1.0f; break;
				case 900: inAlienTime -= 1.5f; break;
				case 1200:inAlienTime -= 2.0f;  break;
				case 1500:inAlienTime -= 2.5f;  break;
				case 1800: inAlienTime -= 3.0f; break;
			}

			// エイリアン数が指定最大数体以下及び、呼び出し時間を超えた場合、エイリアンが出現する
			if (alienNumber < alienMax && latencyAdd > inAlienTime)
			{
				// ドアのアニメーションを行う
				SetdoorAnimationFlag(true);

				// エイリアンの秒数設定処理
				TheNumberOfSecondsSet();

				// エイリアンの種類設定
				alienPattern[GetSeatAddId()] = (EAlienPattern)Random.Range((int)EAlienPattern.MARTIAN, (int)EAlienPattern.MAX);

				// エイリアン生成
				counterDesignatedObj[GetSeatAddId()] = Instantiate(prefab[(int)alienPattern[GetSeatAddId()]], new Vector3(0.0f, 0.8f, 7.0f), Quaternion.identity) as GameObject;
				counterDesignatedObj[GetSeatAddId()].transform.SetParent(transform);

				// 待機状態終了
				AlienStatus.SetCounterStatusChangeFlag(false, GetSeatAddId(), (int)AlienStatus.EStatus.STAND);

				// 空いている席へ
				AlienStatus.SetCounterStatusChangeFlag(true, GetSeatAddId(), (int)AlienStatus.EStatus.WALK);

				// その席が座られている状態にする
				orSitting[GetSeatAddId()] = true;

				// 時間初期化
				latencyAdd = 0.0f;

				// エイリアン数の更新
				alienNumber++;

				// エイリアンIDの保存
				idSave = GetSeatAddId();
			}
		}
		// 空いている席のIDになるまでこの処理を続ける
		else { SetSeatAddId(Random.Range(0, GetCounterSeatsMax())); }
	}

	/// <summary>
	/// 各エイリアンの秒数設定関数
	/// </summary>
	void TheNumberOfSecondsSet()
	{
		// 金持ち度IDの更新
		richDegreeId = GetSeatAddId();

		// エイリアンの金持ち度をランダムで設定及び、客の秒数設定
		switch (Random.Range((int)ERichDegree.POVERTY, (int)ERichDegree.RAND))
		{
			case (int)ERichDegree.POVERTY: orderLatencyAdd[GetRichDegreeId()] = theRemainingTime[0]; break;
			case (int)ERichDegree.NORMAL: orderLatencyAdd[GetRichDegreeId()] = theRemainingTime[1]; break;
			case (int)ERichDegree.RICHMAN: orderLatencyAdd[GetRichDegreeId()] = theRemainingTime[2]; break;
			default: Debug.Log("Error:カウンター客の秒数設定がされていません"); break;
		}
	}

	/// <summary>
	/// 例外処理関数
	/// </summary>
	void ExceptionHandling()
	{
		// 例外フラグがOFFの時
		if(!GetExceptionFlag())
		{
			// 例外処理用のフレーム更新
			exceptionTime += Time.deltaTime;

			// 指定の一定間隔で例外処理発生
			if (exceptionTime >= period)
			{
				// フレームの初期化(一定間隔で例外処理を発生させる為に)
				exceptionTime = 0.0f;

				// 例外フラグON
				exceptionFlag = true;
			}
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
				SetSeatAddId(i);
				orSitting[GetSeatAddId()] = false;

				// 次のエイリアンが退店出来るようにする
				AlienMove.SetCounterClosedCompletion(false, i);

				// 次のエイリアンがチップをプレイヤーに渡せるように初期化
				AlienChip.SetChipOnFlag(false, i);

				// 時間初期化
				latencyAdd = 0.0f;
			}
		}
	}

	/// <summary>
	/// 時間設定関数
	/// </summary>
	void InTimeConfiguration()
	{
		// 現在の時間を管理
		// 残り時間が減っていく毎にエイリアンの入店インターバルが減っていく
		switch ((int)gameTimeManager.GetCountTime())
		{
			case 200: inAlienTime -= 0.01f; break;
			case 180: inAlienTime -= 0.03f; break;
			case 150: inAlienTime -= 0.05f; break;
			case 120: inAlienTime -= 0.07f; break;
			case 90: inAlienTime -= 0.09f; break;
			case 60: inAlienTime -= 0.1f; break;
			case 30: inAlienTime -= 0.12f; break;
		}

		// 指定した数分エイリアンが入店すると、入店時間が再設定される
		if (alienNumber >= 1 && !inAlienFlag)
		{
			inAlienFlag = !inAlienFlag;
			inAlienTime = enterShop;
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
	/// 例外フラグの格納
	/// </summary>
	/// <param name="_exceptionFlag"></param>
	/// <returns></returns>
	public static bool SetExceptionFlag(bool _exceptionFlag) => exceptionFlag = _exceptionFlag;

	/// <summary>
	/// 例外フラグの取得
	/// </summary>
	/// <returns></returns>
	public static bool GetExceptionFlag() => exceptionFlag;

	/// <summary>
	/// 席管理用IDの格納
	/// </summary>
	/// <param name="_seatAddId"></param>
	/// <returns></returns>
	public static int SetSeatAddId(int _seatAddId) => seatAddId = _seatAddId;

	/// <summary>
	/// 席管理用IDの取得
	/// </summary>
	/// <returns></returns>
	public static int GetSeatAddId() => seatAddId;

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
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static float GetOrderLatencyAdd(int seatId) => orderLatencyAdd[seatId];

	/// <summary>
	/// 席の状態を取得
	/// </summary>
	/// <param name="seatId"></param>
	/// <returns></returns>
	public static bool GetOrSitting(int seatId) => orSitting[seatId];

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