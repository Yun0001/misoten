using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerInput : MonoBehaviour
{
    private int playerID;
    private GamePad.Index PlayerControllerNumber;
    private string inputXAxisName;
    private string inputYAxisName;
    private Player player_cs;
    private PlayerMove playerMove_cs;
    private HindranceItem hindrance_cs;

    Vector2 oldStickVec = new Vector2(0, -1);

    private int stickframe = 0;

    [SerializeField]
    private float Angle;

    [SerializeField]
    private float AngleSum = 0;

    [SerializeField]
    private int rotationNum = 0;

    // Use this for initialization
    public void Init(string xaxisname,string yaxisdname)
    {
        player_cs = GetComponent<Player>();
        playerMove_cs = GetComponent<PlayerMove>();
        hindrance_cs = GetComponent<HindranceItem>();
        playerID = player_cs.GetPlayerID();
        PlayerControllerNumber = player_cs.GetPlayerControllerNumber();
        inputXAxisName = xaxisname;
        inputYAxisName = yaxisdname;
    }

    // Update is called once per frame
   public void UpdateInput()
    {
        // アクション
        if (InputDownButton(GamePad.Button.B)) player_cs.ActionBranch();

        // Yボタン入力（キャンセル）
        if (InputDownButton(GamePad.Button.A)) player_cs.CookingCancel();

        // 移動量セット
        playerMove_cs.SetMove(new Vector3(Input.GetAxis(inputXAxisName), 0, -(Input.GetAxis(inputYAxisName))));
    }

    public void InputMicrowave()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().DecisionCheckClockCollision();
        }
    }

    public void InputGrilled()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().DecisionTimingPointCollision();
        }
    }

    public void InputMixerAccess()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
        }


        if (InputDownButton(GamePad.Button.A))
        {
            player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().DecisionAccessPoint(transform.position);
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().ReturnStatus();
        }
    }

    public void InputMixerWait()
    {
        if (InputDownButton(GamePad.Button.A))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        }
    }

    public void InputMixer()
    {
        stickframe++;
        if (stickframe >= 2)
        {
            stickframe = 0;
            Vector2 stickVec = GamePad.GetAxis(GamePad.Axis.LeftStick, PlayerControllerNumber);
            Angle = AngleWithSign(oldStickVec, stickVec);
            if (Angle != 90)
            {
                AngleSum += Angle;
                if (!player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().GetMiniGameUI().GetComponent<MixerMiniGame>().GetRotation())
                {
                    if ((int)AngleSum <= -360 * (rotationNum + 1))
                    {
                        rotationNum++;
                        if (player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().OneRotation())
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
                        player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().OneRotation();
                    }
                }

            }
            oldStickVec = stickVec;
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

    public void InputIceBox()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.GetHitObj((int)Player.hitObjName.IceBox).GetComponent<IceBox>().ActionMiniGame();
        }
    }

    public void InputDastBox()
    {
        if (GamePad.GetButton(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
        }
        // 途中でボタンを離した時
        else if (GamePad.GetButtonUp(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetDastBoxUI().SetActive(false);
            switch (player_cs.GetDastBoxUI().GetComponent<DastBox>().GetPlayerStatus())
            {
                case 9:
                    player_cs.SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
                    break;
                case 10:
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                    break;
            }
        }
    }

    private bool InputDownButton(GamePad.Button button) => GamePad.GetButtonDown(button, PlayerControllerNumber);
}



