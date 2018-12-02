using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class NormalState : PlayerStateBase {

    
    public override void InputState()
    {
        if (player_cs.InputDownButton(GamePad.Button.B))
        {
            foreach (var Name in player_cs.GetAccessController().GetAccessObjectNameArray())
            {
                // アクセス可能なオブジェクトが見つかった！！
                if (player_cs.GetAccessController().IsAccessPossible(Name))
                {
                    player_cs.GetAccessPossibleAnnounce_cs().HiddenSprite();
                    player_cs.StopMove(); // 移動値をリセット

                    if (Name == PlayerAccessController.AccessObjectName.Alien)
                    {
                        OfferCuisine();
                    }
                    else
                    {
                        // アタッチしているスクリプトを変更
                        player_cs.ChangeAttachComponent((int)Name);
                        break;
                    }
                }
            }
        }
    }

    public override void AccessAction()
    {

    }
    public override void UpdateState()
    {

    }

    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
        if (!IsOffer())
        {
            // エイリアンのスクリプトを取得して料理を渡す
            player_cs.GetHaveInEatoy_cs().SetHaveInEatoyPosition(player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien).transform.position);
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().EatCuisine(player_cs.GetHaveInEatoy_cs().GetHaveInEatoy());
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            // イートイを表示
            player_cs.GetHaveInEatoy_cs().DisplayEatoy();


            player_cs.SetPlayerStatus(Player.PlayerStatus.Normal);
        }
    }

    public bool IsOffer()
    {
        int setId = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().GetSetId();
        return
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.CLAIM) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.SATISFACTION) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.RETURN_BAD) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.RETURN_GOOD);
    }
}
