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

    /// <summary>
    /// 電子レンジを動かす
    /// </summary>
    public void CookingStart()
    {
        // 電子レンジ調理開始
        if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).GetComponent<CookWareMw>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
        // プレイヤーのステータスを電子レンジ調理状態に変更
        player_cs.SetPlayerStatus(Player.PlayerStatus.Microwave);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(true);

        // レンジOpenSE
        Sound.PlaySe(GameSceneManager.seKey[16], 4);
        microwaveEffect.Play();
    }

    /// <summary>
    /// Player.csが呼ぶ更新関数
    /// </summary>
    public void UpdateCookingMicrowave()
    {
        GameObject eatoy = UpdateMicrowave();
        if (eatoy == null) return;

        // 料理を持つ
        player_cs.SetHaveInEatoy(eatoy);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(true);
        // レンジOpenSE
        //Sound.PlaySe(GameSceneManager.seKey[16], 4);
    }

    private GameObject UpdateMicrowave()
    {
        GameObject eatoy = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).GetComponent<CookWareMw>().UpdateMiniGame();
        if (eatoy != null)
        {
            microwaveEffect.Stop();
        }
        return eatoy;
    }


    /// <summary>
    /// 調理中断
    /// </summary>
    public void CancelCooking()
    {
        GameObject microwave = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave);
        microwave.GetComponent<CookWareMw>().CookingInterruption();
        microwave.transform.Find("microwave").GetComponent<mwAnimCtrl>().SetIsOpen(false);
        microwaveEffect.Stop();
        player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.Microwave);
    }
}
