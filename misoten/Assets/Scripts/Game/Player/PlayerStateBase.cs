using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態クラスベース
/// </summary>
public abstract class PlayerStateBase : MonoBehaviour
{
    protected Player player_cs;
	
    /// <summary>
    /// 生成時処理
    /// </summary>
	void Awake ()
    {
        player_cs = GetComponent<Player>();
	}

    /// <summary>
    /// 入力処理
    /// </summary>
    public abstract void InputState();

    /// <summary>
    /// 更新処理
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// アクセス時の処理
    /// </summary>
    public abstract void AccessAction();

    public void DeleteComponent() => Destroy(this);
}
