using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンがプレイヤーに対して、チップを渡すスクリプト
/// </summary>
public class AlienChip : MonoBehaviour
{
	/// <summary>
	/// エイリアンがチップをプレイヤーに渡す種類
	/// </summary>
	private enum EChipPattern
	{
		PUT = 0,	// チップを置いていく
		HANDOVER,	// チップを直接渡す
		MAX			// 最大
	}

	// チップの渡し方
	[SerializeField]
	private EChipPattern chipPattern;

	// エイリアンのオーダー
	private AlienOrder alienOrder;

	// 料理が来たかの判定用
	private bool isCuisineCame = false;

	// チップの値
    [SerializeField]
	private float chipVal = 0.0f;

    // 料理の旨味係数
    private float cisineTasteCoefficient = 0f;

    //料理を持ってきた相手のID
    private int opponentID;

	// 初期化フラグ
	private bool initFlag = false;

    private int[] baseChip = { 100, 200, 300 };

	// チップをプレイヤーに渡したかのフラグ
	private static bool[] chipOnFlag = { false, false, false, false, false };

	// チップID(各エイリアン事に持っている)
	private int chipId;

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 一度だけ初期化
		if (!initFlag)
		{
			Init();
			initFlag = true;
		}

		// エイリアンが注文している時
		if (alienOrder.GetIsOrder())
		{
			// if (Input.GetKey(KeyCode.Space))
			//  {
			// 料理が来た判定
			//SetCuisineCame(true);
			if (isCuisineCame)
            {

				//SetChipValue(CalcChipValue());
				// チップの渡し方の管理
				switch (chipPattern)
                {
                    case EChipPattern.PUT:      // チップを置いていく

                        break;
                    case EChipPattern.HANDOVER: // チップを直接渡す
                        ScoreManager.GetInstance().GetComponent<ScoreManager>().AddScore(opponentID, CalcChipValue());
                        SetCuisineCame(false);

						// チップをプレイヤーに渡した
						chipOnFlag[chipId] = true;

						break;
                    default:
                        // 例外処理
                        break;
                }
            }
            
          //  }
		
		}
	}

	/// <summary>
	/// 初期化関数
	/// </summary>
	void Init()
	{
		chipId = AlienCall.GetAddId();

		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();
		// プロトでは手渡し
		chipPattern = EChipPattern.HANDOVER;
		// ベースチップをセット
		SetChipValue(baseChip[GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>().GetRichDegree(chipId)]);

		//richDegreeId++;
		//if (richDegreeId > 5) { richDegreeId = 0; }
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
    public void SetCuisineCoefficient(float coefficient)
    {
        cisineTasteCoefficient = coefficient;
    }

    /// <summary>
    /// 渡すor置くチップの計算
    /// </summary>
    /// <returns>渡すor置くチップの値</returns>
    public int CalcChipValue()
    {
        return (int)(chipVal * cisineTasteCoefficient);
    }

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

	public void SetOpponentID(int ID) => opponentID = ID;
}