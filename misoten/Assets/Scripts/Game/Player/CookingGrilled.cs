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

    /// <summary>
    /// 調理開始
    /// </summary>
    public void CookingStart()
    {
        if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy()))
        {
            return;
        }
        player_cs.SetPlayerStatus(Player.PlayerStatus.GrilledTable);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(true);

        //着火SE
        CookingStratSE();
    }

    /// <summary>
    /// Playerが呼び出す更新関数
    /// </summary>
    public void UpdateCookingGrilled()
    {
        GameObject eatoy = UpdateGrilled();
        if (eatoy == null) return;

        // 焼く調理終了の処理 
        player_cs.SetHaveInEatoy(eatoy);
        player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    /// <returns></returns>
    private GameObject UpdateGrilled()
    {
        GameObject obj = player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().UpdateMiniGame();
        if (obj == null)
        {
            return null;
        }
        SuccessSE();
        return obj;
    }

    /// <summary>
    /// 調理キャンセル
    /// </summary>
    public void CancelCooking()
    {
        GameObject flyingpan = player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable);
        flyingpan.GetComponent<Flyingpan>().CookingInterruption();
        flyingpan.transform.Find("pan").GetComponent<CookWareAnimCtrl>().SetBool(false);
        CancelSE();
        player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.GrilledTable);
    }

    /// <summary>
    /// 調理開始時SE処理
    /// </summary>
    private void CookingStratSE()
    {
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), true, 12);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
    }

    /// <summary>
    /// 調理終了時SE処理
    /// </summary>
    private void SuccessSE()
    {
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), false, 12);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Success_share), 11);
    }

    /// <summary>
    /// キャンセル時SE処理
    /// </summary>
    private void CancelSE()
    {
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 14);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), false, 12);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Stirfry), 12);
    }
}
