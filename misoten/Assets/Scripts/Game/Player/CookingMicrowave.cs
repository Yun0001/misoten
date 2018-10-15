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
	
    /// <summary>
    /// 電子レンジを動かす
    /// </summary>
    public void PresstheMicrowaveStartButton()
    {
        // 電子レンジ調理開始
        if (!player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<MicroWave>().StartCooking()) return;
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
        return player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<MicroWave>().EndCooking();
    }
}
