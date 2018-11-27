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

    // 調理開始
    public void CookingStart()
    {

        if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
              
        player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(true);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire),17);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), true, 13);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
    }

    /// <summary>
    /// 調理
    /// </summary>
    /// <param name="stickVec"></param>
    public GameObject UpdatePot()
    {
        GameObject obj = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>().UpdateMiniGame();
        if (obj == null)
        {
            return null;
        }
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Success_share), 11);
        return obj;
    }

    public void CancelCooking()
    {
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>().CookingInterruption();
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).transform.Find("nabe").GetComponent<CookWareAnimCtrl>().SetBool(false);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), false, 13);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 16);
    }
}
