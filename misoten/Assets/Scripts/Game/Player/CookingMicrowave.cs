using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class CookingMicrowave : MonoBehaviour {

    private Player player_cs;

    [SerializeField]
    private ParticleSystem microwaveEffect;
	// Use this for initialization
	void Awake ()
    {
        player_cs = GetComponent<Player>();
	}

    public GameObject UpdateMicrowave()
    {
        GameObject eatoy = player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().UpdateMiniGame();
        if (eatoy != null)
        {
            microwaveEffect.Stop();
        }
        return eatoy;
    }


    /// <summary>
    /// 電子レンジを動かす
    /// </summary>
    public void CookingStart()
    {
        // 電子レンジ調理開始
        if (!player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
        // プレイヤーのステータスを電子レンジ調理状態に変更
        player_cs.SetPlayerStatus(Player.PlayerStatus.Microwave);
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(true);

        // レンジOpenSE
        Sound.PlaySe(GameSceneManager.seKey[16], 4);
        microwaveEffect.Play();
    }

    /// <summary>
    /// 調理中断
    /// </summary>
    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().CookingInterruption();
        player_cs.GetHitObj((int)Player.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(false);
        microwaveEffect.Stop();
    }
}
