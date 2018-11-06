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
        if (!player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().StartCookingPot(player_cs.GetPlayerID())) return;
        player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);
        player_cs.GetHitObj((int)Player.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(true);

    }

    /// <summary>
    /// 調理
    /// </summary>
    /// <param name="stickVec"></param>
    public GameObject UpdatePot()
    {
        if (player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().UpdateCooking())
        {
            player_cs.GetHitObj((int)Player.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);
            return player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().GetPotCuisine();
        }
        return null;
    }

    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().InterruptionCooking();
        player_cs.GetHitObj((int)Player.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);

    }
}
