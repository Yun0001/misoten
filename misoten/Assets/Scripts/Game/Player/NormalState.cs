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
            // アクセス可能オブジェクトを検索
            var accessObjName = IsAccessPossible();

            // Noneならreturn
            if (accessObjName == PlayerAccessController.AccessObjectName.None) return;

            // アナウンスUI非表示
            player_cs.GetAccessPossibleAnnounce_cs().HiddenSprite();
            // 移動量を０に
            player_cs.StopMove();

            // エイリアンにアクセスしようとしているなら配膳関数を呼ぶ
            if (accessObjName == PlayerAccessController.AccessObjectName.Alien)
            {
                OfferCuisine();
            }
            else
            {
                // アタッチしているスクリプトを変更
                player_cs.ChangeAttachComponent((int)accessObjName);
            }
        }
    }

    /// <summary>
    /// アクセス可能オブジェクトを検索
    /// </summary>
    /// <returns></returns>
    private PlayerAccessController.AccessObjectName IsAccessPossible()
    {
        foreach (var Name in player_cs.GetAccessController().GetAccessObjectNameArray())
        {
            // アクセス可能なオブジェクトが見つかった！！
            if (player_cs.GetAccessController().IsAccessPossible(Name)) return Name;
        }

        // アクセス可能オブジェクトが見つからなければNoneを返す
        return PlayerAccessController.AccessObjectName.None;
    }


    /// <summary>
    /// 料理を渡す
    /// </summary>
    public void OfferCuisine()
    {
        if (!IsOffer())
        {
            // エイリアンのスクリプトを取得して料理を渡す
            var alien = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien);
            // エイリアン側の処理
            alien.GetComponent<AlienOrder>().EatCuisine(player_cs.GetHaveInEatoy_cs().GetHaveInEatoy());
            // プレイヤーの処理
            player_cs.GetHaveInEatoy_cs().OfferEatoy(alien.transform.position);

            // 状態変更
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            player_cs.SetPlayerStatus(Player.PlayerStatus.Normal);
        }
    }

    /// <summary>
    /// イートイを渡そうとしているエイリアンが食べられる状態か判定
    /// </summary>
    /// <returns></returns>
    public bool IsOffer()
    {
        var setId = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien).GetComponent<AlienOrder>().GetSetId();
        return
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.RETURN_BAD) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.RETURN_GOOD) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.EAT) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.SATISFACTION) ||
            AlienStatus.GetCounterStatusChangeFlag(setId, (int)AlienStatus.EStatus.CLAIM);
    }


    public override void AccessAction()
    {

    }
    public override void UpdateState()
    {

    }

}
