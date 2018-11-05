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
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Grilled>().StartCooking(player_cs.GetPlayerControllerNumber(),ScoreManager.GetInstance().GetPlayerRank(player_cs.GetPlayerID()));
        player_cs.SetPlayerStatus(Player.PlayerStatus.GrilledTable);
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(true);
    }

    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Grilled>().InterruptionCooking();
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);

    }
}
