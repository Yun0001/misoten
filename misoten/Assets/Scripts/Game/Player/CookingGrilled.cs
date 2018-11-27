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
        if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy()))
        {
            return;
        }
        player_cs.SetPlayerStatus(Player.PlayerStatus.GrilledTable);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(true);

        //着火SE
       // Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
      //  Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 0.1f, 14);
     // //  Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
//Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 0.3f, 12);
    }

    public GameObject UpdateGrilled()
    {
        GameObject obj = player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().UpdateMiniGame();
        if (obj == null)
        {
            return null;
        }
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Success_share), 11);
        return obj;
    }

    public void CancelCooking()
    {
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().CookingInterruption();
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
    }
}
