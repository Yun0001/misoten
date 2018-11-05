using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class CookingMicrowave : MonoBehaviour {

    private Player player_cs;
	// Use this for initialization
	void Awake ()
    {
        player_cs = GetComponent<Player>();
	}

    public GameObject UpdateMicrowave()
    {
        return player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().UpdateMicrowaveGage();
    }

    /// <summary>
    /// 電子レンジを動かす
    /// </summary>
    public void PresstheMicrowaveStartButton()
    {
        // 電子レンジ調理開始
        if (!player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().StartCookingMicrowave(player_cs.GetPlayerID(), player_cs.GetPlayerControllerNumber())) return;
        // プレイヤーのステータスを電子レンジ調理状態に変更
        player_cs.SetPlayerStatus(Player.PlayerStatus.Microwave);
    }

    /// <summary>
    /// 電子レンジを停止させる
    /// </summary>
    /// <returns></returns>
    public GameObject PresstheMicrowaveStopButton()
    {
        // 電子レンジ調理終了
        return player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().EndCookingMicrowave();
    }

    /// <summary>
    /// 調理中断
    /// </summary>
    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().InterruptionCooking();
    }
}
