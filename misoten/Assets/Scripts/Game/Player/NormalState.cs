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
                    player_cs.GetAccessPossibleAnnounce().HiddenSprite();
                    player_cs.StopMove(); // 移動値をリセット

                    // アタッチしているスクリプトを変更
                    player_cs.ChangeAttachComponent((int)Name);
                    break;
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
}
