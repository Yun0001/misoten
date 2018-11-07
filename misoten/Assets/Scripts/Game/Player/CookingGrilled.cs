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

    public void CookingStart()
    {
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().CookingStart();
        player_cs.SetPlayerStatus(Player.PlayerStatus.GrilledTable);
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(true);
    }

    public GameObject UpdateGrilled()
    {
        return player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().UpdateMiniGame();
    }

    public void CancelCooking()
    {
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().CookingInterruption();
        player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }
}
