using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIceBox : MonoBehaviour {

    private Player player_cs;
    private void Awake()
    {
        
    }

    public void AccessIceBox()
    {
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>().Access(player_cs.GetPlayerID()))
        {
            player_cs.SetPlayerStatus(Player.PlayerStatus.IceBox);
        }
    }

    public void UpdateIceBox()
    {
        player_cs.GetPlayerInput().InputIceBox();
        IceBox iceBox = player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>();
        if (iceBox.IsPutEatoy() && iceBox.IsAccessOnePlayer(player_cs.GetPlayerID()))
        {
            // イートイを持つ
            player_cs.SetHaveInEatoy(iceBox.PassEatoy());
            // 冷蔵庫の後処理
            iceBox.ResetEatoy();

            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.RefrigeratorSuccess),
               iceBox.GetIceBoxID() + 5);
        }    }
}
