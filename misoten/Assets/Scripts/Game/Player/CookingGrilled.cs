using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingGrilled : MonoBehaviour {

    private Player player_cs;
    // Use this for initialization
    void Awake()
    {
        player_cs = GetComponent<Player>();
    }

    public void OnFire()
    {
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Grilled>().StartCooking();
        player_cs.SetPlayerStatus(Player.PlayerStatus.GrilledTable);
    }

    public void ShaketheFryingpan()
    {
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Grilled>().EndCooking();
        player_cs.WithaCuisine(player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Grilled>().GetGrilledCuisine());
    }
}
