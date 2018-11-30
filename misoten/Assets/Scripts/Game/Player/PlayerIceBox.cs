using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceBox : MonoBehaviour {

    private Player player_cs;
    private IceBox iceBox_cs;
    private void Awake()
    {
        player_cs = GetComponent<Player>();
    }

    public void AccessIceBox()
    {
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>().Access(player_cs.GetPlayerID()))
        {
            player_cs.SetPlayerStatus(Player.PlayerStatus.IceBox);
            iceBox_cs = player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>();
        }
    }

    public void UpdateIceBox()
    {
        player_cs.GetPlayerInput().InputIceBox();
        if (iceBox_cs.IsPutEatoy() && iceBox_cs.IsAccessOnePlayer(player_cs.GetPlayerID()))
        {
            // イートイを持つ
            player_cs.SetHaveInEatoy(iceBox_cs.PassEatoy());
            // 冷蔵庫の後処理
            iceBox_cs.ResetEatoy();
            GetComponent<PlayerAnimCtrl>().SetFront(true);

            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.RefrigeratorSuccess),
               iceBox_cs.GetIceBoxID() + 5);
            ResetIceBox_cs();
        }
    }

    public void ResetIceBox_cs() => iceBox_cs = null;
}
