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
        if (player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().GetStatus() == Pot.PotState.unused)
        {
            player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().StartCookingPot(player_cs.GetPlayerID());
            player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);
        }
    }

    /// <summary>
    /// 調理
    /// </summary>
    /// <param name="stickVec"></param>
    public GameObject Mix()
    {
        if (player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().UpdateCooking())
        {
            return player_cs.GetHitObj((int)Player.hitObjName.Pot).GetComponent<Pot>().GetPotCuisine();
        }
        return null;
    }
}
