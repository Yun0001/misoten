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

    private int[] baseChip = { 100, 200, 300 };

	private static int richDegreeId = 0;

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start ()
	{
		// コンポーネント取得
		alienOrder = GetComponent<AlienOrder>();
        // プロトでは手渡し
        chipPattern = EChipPattern.HANDOVER;
        // ベースチップをセット
        SetChipValue(baseChip[GameObject.Find("Aliens").gameObject.GetComponent<AlienCall>().GetRichDegree(richDegreeId)]);

		// エイリアンの種類IDを更新
		if (richDegreeId < 4) { richDegreeId++; }
		else { richDegreeId = 0; };
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
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

						Debug.Log("入った");
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

    public void SetOpponentID(int ID) => opponentID = ID;
}