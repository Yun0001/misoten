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
        switch (playerID)
        {
            case 0:
                InputGamepad();
                break;

            case 1:
                InputGamepad();
                break;

            case 2:
                InputGamepad();
                InputKeyBoard_Player3();
                break;

            case 3:
                InputGamepad();
                InputKeyBoard_Player4();
                break;
        }
    }

    private void InputGamepad()
    {
        AllStatus();

        playerMove_cs.SetMove(new Vector3(Input.GetAxis(inputXAxisName), 0, -(Input.GetAxis(inputYAxisName))));
    }


    private void InputKeyBoard_Player4()
    {
        if (Input.GetKey(KeyCode.A)) playerMove_cs.SetMove(PlayerMove.EDirection.Left);
        if (Input.GetKey(KeyCode.D)) playerMove_cs.SetMove(PlayerMove.EDirection.Right);
        if (Input.GetKey(KeyCode.W)) playerMove_cs.SetMove(PlayerMove.EDirection.Up);
        if (Input.GetKey(KeyCode.S)) playerMove_cs.SetMove(PlayerMove.EDirection.Down);
        if (Input.GetKeyDown(KeyCode.Z)) player_cs.ActionBranch();
        if (Input.GetKeyDown(KeyCode.X)) player_cs.CookingCancel();
    }

    private void InputKeyBoard_Player3()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Left);
        if (Input.GetKey(KeyCode.RightArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Right);
        if (Input.GetKey(KeyCode.UpArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Up);
        if (Input.GetKey(KeyCode.DownArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Down);
        if (Input.GetKeyDown(KeyCode.K)) player_cs.ActionBranch();
        if (Input.GetKeyDown(KeyCode.L)) player_cs.CookingCancel();
    }

    private void AllStatus()
    {
        // アクション
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber)) player_cs.ActionBranch();


        //if (GamePad.GetButtonDown(GamePad.Button.X, PlayerControllerNumber)) player_cs.GetHitObj((int)Player.hitObjName.IceBox).GetComponent<IceBox>().DecisionPutEatoyElement();

        // Yボタン入力（キャンセル）
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber)) player_cs.CookingCancel();
    }

    public void InputMicrowave()
    {
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().DecisionCheckClockCollision();
        }

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().DecisionCheckClockCollision();
                }
                break;

            case GamePad.Index.Four:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Microwave).GetComponent<Microwave>().DecisionCheckClockCollision();
                }
                break;
        }
    }

    public void InputGrilled()
    {
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().DecisionTimingPointCollision();
        }

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().DecisionTimingPointCollision();
                }
                break;

            case GamePad.Index.Four:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.GrilledTable).GetComponent<Flyingpan>().DecisionTimingPointCollision();
                }
                break;
        }
    }

    public void InputMixerAccess()
    {
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
        }


        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber))
        {
            player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().DecisionAccessPoint(transform.position);
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().ReturnStatus();
        }

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
                    player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().DecisionAccessPoint(transform.position);
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().ReturnStatus();
                }
                break;

            case GamePad.Index.Four:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
                    player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().DecisionAccessPoint(transform.position);
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().ReturnStatus();
                }
                break;
        }
    }

    public void InputMixerWait()
    {
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber))
        {
            player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        }

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKeyDown(KeyCode.L))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
                    player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
                }
                break;

            case GamePad.Index.Four:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
                    player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
                }

                break;
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
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetHitObj((int)Player.hitObjName.IceBox).GetComponent<IceBox>().ActionMiniGame();
        }

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.IceBox).GetComponent<IceBox>().ActionMiniGame();
                }
                break;

            case GamePad.Index.Four:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    player_cs.GetHitObj((int)Player.hitObjName.IceBox).GetComponent<IceBox>().ActionMiniGame();
                }

                break;
        }
    }

    public void InputDastBox()
    {
        if (GamePad.GetButton(GamePad.Button.B, PlayerControllerNumber))
        {
            player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
        }
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

        switch (PlayerControllerNumber)
        {
            case GamePad.Index.Three:
                if (Input.GetKey(KeyCode.K))
                {
                    player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
                }
                else if (Input.GetKeyUp(KeyCode.K))
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
                break;

            case GamePad.Index.Four:
                if (Input.GetKey(KeyCode.Z))
                {
                    player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
                }
                else if (Input.GetKeyUp(KeyCode.Z))
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
                break;
        }
    }
}



