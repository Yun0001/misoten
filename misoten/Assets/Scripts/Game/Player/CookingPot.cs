using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : MonoBehaviour {

    private Player player_cs;
    // Use this for initialization
    void Awake()
    {
        player_cs = GetComponent<Player>();
    }

    public void CookingStart()
    {
        if (!player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
        player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);
        player_cs.GetHitObj((int)Player.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(true);
        Sound.PlaySe(GameSceneManager.seKey[27]);
        Sound.SetLoopFlgSe(GameSceneManager.seKey[22], true, 4);
        Sound.PlaySe(GameSceneManager.seKey[22], 4);
    }

    /// <summary>
    /// 調理
    /// </summary>
    /// <param name="stickVec"></param>
    public GameObject UpdatePot()
    {
        return player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().UpdateMiniGame();
    }

    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().CookingInterruption();
        player_cs.GetHitObj((int)Player.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);
        Sound.SetLoopFlgSe(GameSceneManager.seKey[22], false, 4);
        Sound.StopSe(GameSceneManager.seKey[22], 4);
    }
}
