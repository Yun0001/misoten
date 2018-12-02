using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PotState : PlayerStateBase {

    /// <summary>
    /// 入力処理
    /// </summary>
    public override void InputState()
    {
        // キャンセルキー入力
        if (player_cs.InputDownButton(GamePad.Button.A))
        {
            Pot pot = GetCollisionPot_cs();
            pot.SetIrairaFrameInFlag(false);
            pot.CookingInterruption();
            pot.GetAnimationModel().GetComponent<CookingAnimCtrl>().SetIsCooking(false);
            CancelSE();
            player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.Pot);
            player_cs.SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
            player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public override void UpdateState()
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
    /// アクセス時の処理
    /// </summary>
    public override void AccessAction()
    {
        if (GetCollisionPot_cs().IsCooking()) return;
        GetCollisionPot_cs().JoystickInit(player_cs.GetPlayerID());
        if (!GetCollisionPot_cs().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;

        player_cs.SetPlayerStatus(Player.PlayerStatus.Pot);

        GetCollisionPot_cs().GetAnimationModel().GetComponent<CookingAnimCtrl>().SetIsCooking(true);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 17);
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Boil), true, 13);
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Boil), 13);
    }

    private Pot GetCollisionPot_cs() => player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot).GetComponent<Pot>();

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
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Fire), 17);
    }
}
