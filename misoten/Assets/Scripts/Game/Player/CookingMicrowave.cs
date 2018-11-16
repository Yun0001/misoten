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
        return player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().UpdateMiniGame();
    }

    /// <summary>
    /// 電子レンジを動かす
    /// </summary>
    public void CookingStart()
    {
        // 電子レンジ調理開始
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().CookingStart(player_cs.GetHaveInHandCuisine());
        // プレイヤーのステータスを電子レンジ調理状態に変更
        player_cs.SetPlayerStatus(Player.PlayerStatus.Microwave);
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(true);
    }

    /// <summary>
    /// 調理中断
    /// </summary>
    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().CookingInterruption();
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetBool(false);

    }
}
