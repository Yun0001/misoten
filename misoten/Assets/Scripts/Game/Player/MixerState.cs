using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class MixerState : PlayerStateBase
{
    [SerializeField]
    private float Angle;

    [SerializeField]
    private float AngleSum = 0;

    [SerializeField]
    private int rotationNum = 0;

    Vector2 oldStickVec = new Vector2(0, -1);

    private int stickframe = 0;

    public override void InputState()
    {
        switch (player_cs.GetPlayerStatus())
        {
            case Player.PlayerStatus.MixerAccess:
                //　決定ボタン
                if (player_cs.InputDownButton(GamePad.Button.B))
                {
                    // ミキサーのアクセス数を加算
                    player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
                    // プレイヤーの状態をミキサー待機状態に変更
                    player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
                    player_cs.DisplaySandbySprite();
                }

                // キャンセルボタン
                if (player_cs.InputDownButton(GamePad.Button.A))
                {
                    // ミキサーへのアクセスを切断
                    if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<MixerAccessPoint>().AccessDiscconnection(transform.position))
                    {
                        // プレイヤーの状態を配膳状態に戻す
                        player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                        player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
                        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().DeleteStackPlayerID(gameObject);
                    }
                }
                break;

            case Player.PlayerStatus.MixerWait:

                // ミキサーがオープン状態以降は入力不可
                if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).
                    GetComponent<Mixer>().GetStatus() < Mixer.Status.Open)
                {
                    // キャンセルボタン入力
                    if (player_cs.InputDownButton(GamePad.Button.A))
                    {
                        // ミキサーのアクセス数を減算
                        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
                        // プレイヤーの状態をミキサーアクセスに戻す
                        player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
                        player_cs.HiddenStandbySprite();
                        GetComponent<PlayerAccessPossiblAnnounce>().SetMixerAccessSprite();
                    }
                }

                break;

                case Player.PlayerStatus.Mixer:
                Mixer mixer_cs = player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>();
                stickframe++;
                if (stickframe >= 2)
                {
                    stickframe = 0;
                    //Vector2 stickVec = GamePad.GetAxis(GamePad.Axis.LeftStick, GetComponent<PlayerInput>().GetPlayerControllerNumber());
                    Vector2 stickVec = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)player_cs.GetPlayerID() + 1);

                    Angle = AngleWithSign(oldStickVec, stickVec);
                    if (Angle != 90)
                    {
                        AngleSum += Angle;
                        if (!mixer_cs.GetMiniGameUI().GetComponent<MixerMiniGame>().GetRotation())
                        {
                            if ((int)AngleSum <= -360 * (rotationNum + 1))
                            {
                                rotationNum++;
                                if (mixer_cs.OneRotation())
                                {
                                    rotationNum = 0;
                                    AngleSum = 0;
                                }
                            }
                        }
                        else
                        {
                            if ((int)AngleSum >= 360 * (rotationNum + 1))
                            {
                                rotationNum++;
                                mixer_cs.OneRotation();
                            }
                        }

                    }
                    oldStickVec = stickVec;
                }
                break;
        }
    }

    public override void AccessAction()
    {
        if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<MixerAccessPoint>().Access(gameObject))
        {
            //Debug.LogError("アクセス失敗");
            return;
        }
        player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        transform.Find("Line").gameObject.SetActive(true);
        GetComponent<PlayerAccessPossiblAnnounce>().SetMixerAccessSprite();
    }

    public override void UpdateState()
    {
        switch (player_cs.GetPlayerStatus())
        {
            case Player.PlayerStatus.MixerWait:
                // 二人でミキサーを利用する時
                if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.Play)
                {
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Mixer);
                    GetComponent<PlayerAnimCtrl>().SetServing(false);
                    transform.Find("Line").gameObject.SetActive(false);
                    player_cs.HiddenAnnounceSprite();
                }
                break;

            case Player.PlayerStatus.Mixer:
                if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                {
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Normal);
                    player_cs.ChangeAttachComponent((int)Player.PlayerStatus.Normal);
                    InitMixerVariable();
                }
                break;
        }

    }



    private float Cross2D(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }


    private float AngleWithSign(Vector2 oldVec, Vector2 Vec)
    {
        float angle = Vector2.Angle(oldVec, Vec);
        float cross = Cross2D(oldVec, Vec);
        return (cross != 0) ? angle * Mathf.Sign(cross) : angle;
    }

    public void InitMixerVariable()
    {
        rotationNum = 0;
        AngleSum = 0;
        stickframe = 0;
    }
}
