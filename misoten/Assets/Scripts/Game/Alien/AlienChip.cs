using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがプレイヤーに対して、チップを渡すスクリプト
/// </summary>
public class AlienChip : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンがチップをプレイヤーに渡す種類
	/// </summary>
	private enum EChipPattern
	{
		PUT = 0,	// チップを置いていく
		HANDOVER,	// チップを直接渡す
		MAX			// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// チップの渡し方
	[SerializeField]
	private EChipPattern chipPattern;

	// チップの値
	[SerializeField]
	private float chipVal = 0.0f;

	// ---------------------------------------------

	// 他のスクリプトから関数越しで参照可能。一つしか存在しない
	// ---------------------------------------------

	// チップをプレイヤーに渡したかのフラグ
	private static bool[] chipOnFlag = { false, false, false, false, false, false, false };

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// 料理が来たかの判定用
	private bool isCuisineCame = false;

	// 料理を持ってきた相手のID
	private int opponentID = 0;

	// チップID(各エイリアン事に持っている)
	private int chipId = 0;

	// 料理の旨味係数
	private float cisineTasteCoefficient = 0.0f;

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// チップIDへの受け渡し
		chipId = AlienCall.GetIdSave();

		// プロトでは手渡し
		chipPattern = EChipPattern.HANDOVER;

		// ベースチップをセット
		SetChipValue(AlienCall.GetRichDegree(chipId));

		// 料理が来たかの判定用の初期化
		isCuisineCame = false;

		// 料理を持ってきた相手のIDの初期化
		opponentID = 0;

		// 料理の旨味係数の初期化
		cisineTasteCoefficient = 0.0f;

		// カウンター席の最大数指定分ループ
		for (int i = 0; i < AlienCall.alienCall.GetCounterSeatsMax(); i++)
		{
			// チップをプレイヤーに渡したかのフラグ
			chipOnFlag[i] = false;
		}

		// Debug用
		//Debug.Log("金持ち度"+ AlienCall.GetRichDegree(chipId));
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エイリアンが注文している時
		if (GetComponent<AlienOrder>().GetIsOrder())
		{
			if (isCuisineCame)
			{
				// チップの渡し方の管理
				switch (chipPattern)
				{
					case EChipPattern.PUT:		// チップを置いていく
						// 多分もう使わない
						break;
					case EChipPattern.HANDOVER:	// チップを直接渡す
						// 満足状態の時
						if (GetComponent<AlienSatisfaction>().GetSatisfactionFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
						{
                            //ScoreManager.GetInstance().GetComponent<ScoreManager>().AddScore(opponentID, CalcChipValue());
                            SetCuisineCame(false);
							GetComponent<AlienOrder>().SetIsOrder(false);

							// チップをプレイヤーに渡した
							chipOnFlag[chipId] = true;
						}
						break;
					default:
						// 例外処理
						break;
				}
			}
		}
	}

	/// <summary>
	/// 料理が来たかの判定を格納
	/// </summary>
	/// <param name="_isCuisineCame"></param>
	/// <returns></returns>
	public bool SetCuisineCame(bool _isCuisineCame) => isCuisineCame = _isCuisineCame;

	/// <summary>
	/// 料理が来たかの判定を取得
	/// </summary>
	/// <returns></returns>
	public bool GetCuisineCame() => isCuisineCame;

	/// <summary>
	/// チップの値を格納
	/// </summary>
	/// <param name="_chipVal"></param>
	/// <returns></returns>
	public float SetChipValue(float _chipVal) => chipVal = _chipVal;

	/// <summary>
	/// チップの値を取得
	/// </summary>
	/// <returns></returns>
	public float GetChipValue() => chipVal;

	/// <summary>
	/// ベースチップに掛ける旨味係数セット
	/// </summary>
	/// <param name="coefficient">料理の旨味係数</param>
	public void SetCuisineCoefficient(float coefficient) => cisineTasteCoefficient = coefficient;

	/// <summary>
	/// ベースチップに掛ける旨味係数取得
	/// </summary>
	/// <returns></returns>
	public float GetCuisineCoefficient() => cisineTasteCoefficient;

	/// <summary>
	/// チップをプレイヤーに渡したかのフラグの格納
	/// </summary>
	/// <param name="_chipOnFlag"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool SetChipOnFlag(bool _chipOnFlag, int id) => chipOnFlag[id] = _chipOnFlag;

	/// <summary>
	/// チップをプレイヤーに渡したかのフラグの取得
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static bool GetChipOnFlag(int id) => chipOnFlag[id];

    /// <summary>
    /// 料理を持ってきた相手のIDの取得
    /// </summary>
    public int GetOpponentID() => opponentID;

    /// <summary>
    /// 料理を持ってきた相手のIDの格納
    /// </summary>
    /// <param name="ID"></param>
    public void SetOpponentID(int ID) => opponentID = ID;

	/// <summary>
	/// 渡すor置くチップの計算
	/// </summary>
	/// <returns>渡すor置くチップの値</returns>
	public int CalcChipValue()
	{
		return (int)(chipVal * cisineTasteCoefficient);
	}
}