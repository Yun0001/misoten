using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class DastBoxState : PlayerStateBase
{

    public override void InputState()
    {
        if (player_cs.InputButton(GamePad.Button.B))
        {
            player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
        }
        // 途中でボタンを離した時
        else if (player_cs.InputUpButton(GamePad.Button.B))
        {
            player_cs.GetDastBoxUI().SetActive(false);
            //アナウンスUIを再表示
            player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.DastBox);
            player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
            switch (player_cs.GetDastBoxUI().GetComponent<DastBox>().GetPlayerStatus())
            {
                case (int)Player.PlayerStatus.CateringIceEatoy:
                    player_cs.SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
                    break;
                case (int)Player.PlayerStatus.Catering:
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                    break;
            }
        }
    }
    public override void AccessAction()
    {
        player_cs.GetDastBoxUI().SetActive(true);
        player_cs.GetDastBoxUI().GetComponent<DastBox>().
            Access((int)player_cs.GetPlayerStatus(), transform.position);
        player_cs.SetPlayerStatus(Player.PlayerStatus.DastBox);
    }

    public override void UpdateState()
    {
        //　ゴミ箱ゲージがMaxの時
        if (player_cs.GetDastBoxUI().GetComponent<DastBox>().GetGageAmount() >= 1.0f)
        {
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.DastBox).transform.Find("box").GetComponent<mwAnimCtrl>().SetIsOpen(true);
            player_cs.SetPlayerStatus(Player.PlayerStatus.Normal);
            player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
            player_cs.GetHaveInEatoy_cs().RevocationHaveInEatoy(true);
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            player_cs.GetDastBoxUI().SetActive(false);
            Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Dustshoot), 0.3f, 8);
            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Dustshoot), 8);
        }
    }
}
