using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;


/// <summary>
/// 電子レンジにアクセスしている状態
/// </summary>
public class MicrowaveState : PlayerStateBase
{
    /// <summary>
    /// 電子レンジアクセス時の入力処理
    /// </summary>
    public override void InputState()
    {
        // 決定キー入力
        if (player_cs.InputDownButton(GamePad.Button.B))
        {
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).
                GetComponent<CookWareMw>().DecisionCheckClockCollision();
        }

        // キャンセルキー入力
        if (player_cs.InputDownButton(GamePad.Button.A))
        {
            CookWareMw microwave_cs = GetMicrowave_cs();
            microwave_cs.CookingInterruption();
            microwave_cs.GetAnimationModel().GetComponent<mwAnimCtrl>().SetIsOpen(false);
            microwave_cs.StopMicrowaveEffect();
            player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.Microwave);
            player_cs.SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
            player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
        }
    }

    /// <summary>
    /// 電子レンジアクセス状態の更新処理
    /// </summary>
    public override void UpdateState()
    {
        GameObject eatoy = UpdateMicrowave();
        if (eatoy == null) return;

        // 料理を持つ
        player_cs.SetHaveInEatoy(eatoy);
        GetMicrowave_cs().GetAnimationModel().GetComponent<mwAnimCtrl>().SetIsOpen(true);
    }

    /// <summary>
    /// アクセス時の処理
    /// </summary>
    public override void AccessAction()
    {
        // 電子レンジ調理開始
        if (!GetMicrowave_cs().CookingStart(GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy())) return;
        // プレイヤーのステータスを電子レンジ調理状態に変更
        player_cs.SetPlayerStatus(Player.PlayerStatus.Microwave);
        GetMicrowave_cs().GetAnimationModel().GetComponent<mwAnimCtrl>().SetIsOpen(true);

        // レンジOpenSE
        Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.MicrowaveOpen), 4);
        GetMicrowave_cs().PlayMicrowaveEffect();
    }

    /// <summary>
    /// 電子レンジの更新
    /// </summary>
    /// <returns></returns>
    private GameObject UpdateMicrowave()
    {
        // 電子レンジミニゲームの更新
        GameObject eatoy = GetMicrowave_cs().UpdateMiniGame();

        // eatoyがnullでない → ミニゲームが終わっている
        if (eatoy != null)
        {
            GetMicrowave_cs().StopMicrowaveEffect();
        }
        return eatoy;
    }


    private CookWareMw GetMicrowave_cs() => player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).GetComponent<CookWareMw>();

}
