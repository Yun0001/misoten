using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : MonoBehaviour {

    private Player player_cs;
	static public bool inFlag = true;
    // Use this for initialization
    void Awake()
    {
		inFlag = true;
		player_cs = GetComponent<Player>();
    }

    // 調理開始
    public void CookingStart()
    {
		if (!GetCollisionPot_cs().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
              
        player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);
        GetCollisionPot_cs().GetAnimationModel().GetComponent<CookingAnimCtrl>().SetIsCooking(true);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire),17);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), true, 13);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
    }

    public void UpdateCookingPot()
    {
        // スティック一周ができればcuisineはnullでない
        GameObject eatoy = UpdatePot();
        if (eatoy == null) return;

        player_cs.SetHaveInEatoy(eatoy);
        GetCollisionPot_cs().GetAnimationModel().GetComponent<CookingAnimCtrl>().SetIsCooking(false);
    }

    /// <summary>
    /// 調理
    /// </summary>
    /// <param name="stickVec"></param>
    public GameObject UpdatePot()
    {
        GameObject obj = GetCollisionPot_cs().UpdateMiniGame();
        if (obj == null) return null;

        SuccessCookSE();
        return obj;
    }

    /// <summary>
    /// 調理キャンセル
    /// </summary>
    public void CancelCooking()
    {
		inFlag = false;
		Pot pot = GetCollisionPot_cs();
        pot.CookingInterruption();
        pot.GetAnimationModel().GetComponent<CookingAnimCtrl>().SetIsCooking(false);
        CancelSE();
        player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.Pot);
    }

    private void SuccessCookSE()
    {
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Success_share), 11);
    }

    /// <summary>
    /// キャンセル時SEストップ
    /// </summary>
    private void CancelSE()
    {
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), false, 13);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 16);
    }

    private Pot GetCollisionPot_cs() => player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>();
}
