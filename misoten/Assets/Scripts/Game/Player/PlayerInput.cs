using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerInput : MonoBehaviour
{
    private GamePad.Index playerControllerNumber;
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
	public void Init()
    {
        player_cs = GetComponent<Player>();
        playerMove_cs = GetComponent<PlayerMove>();
        switch (LayerMask.LayerToName(gameObject.layer))
        {
            case "Player1":
                SetInputAxisName("L_XAxis_1", "L_YAxis_1");
                SetPlayerControllerNumber(GamePad.Index.One);
                break;
            case "Player2":
                SetInputAxisName("L_XAxis_2", "L_YAxis_2");
                SetPlayerControllerNumber(GamePad.Index.Two);
                break;
            case "Player3":
                SetInputAxisName("L_XAxis_3", "L_YAxis_3");
                SetPlayerControllerNumber(GamePad.Index.Three);
                break;
            case "Player4":
                SetInputAxisName("L_XAxis_4", "L_YAxis_4");
                SetPlayerControllerNumber(GamePad.Index.Four);
                break;
        }
    }

    // Update is called once per frame
   public void UpdateInput()
    {

        // アクション
        if (InputDownButton(GamePad.Button.B)) player_cs.ActionBranch();

        // test
      //  if (InputDownButton(GamePad.Button.X)) Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Fire));
      

        // キャンセル
        if (InputDownButton(GamePad.Button.A)) player_cs.CookingCancel();

        // 移動量セット
        playerMove_cs.SetMove(new Vector3(Input.GetAxis(inputXAxisName), 0, -(Input.GetAxis(inputYAxisName))));
    }

    public void InputMicrowave()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave).GetComponent<CookWareMw>().DecisionCheckClockCollision();
        }
    }

    public void InputGrilled()
    {
        if (InputDownButton(GamePad.Button.B))
        {
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable).GetComponent<Flyingpan>().DecisionTimingPointCollision();
        }
    }

    /// <summary>
    /// ミキサーアクセス状態
    /// </summary>
    public void InputMixerAccess()
    {
        //　決定ボタン
        if (InputDownButton(GamePad.Button.B))
        {
            // ミキサーのアクセス数を加算
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().AddAccessNum();
            // プレイヤーの状態をミキサー待機状態に変更
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerWait);
        }

        // キャンセルボタン
        if (InputDownButton(GamePad.Button.A))
        {
            // ミキサーへのアクセスを切断
            if(player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<MixerAccessPoint>().AccessDiscconnection(transform.position))
            {
                // プレイヤーの状態を配膳状態に戻す
                player_cs.SetPlayerStatus(Player.PlayerStatus.Catering);
                //transform.Find("Line").gameObject.SetActive(false);
            }

        }
    }

    /// <summary>
    /// ミキサー待機状態
    /// </summary>
    public void InputMixerWait()
    {
        // キャンセルボタン入力
        if (InputDownButton(GamePad.Button.A))
        {
            // ミキサーのアクセス数を減算
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().SubAccessNum();
            // プレイヤーの状態をミキサーアクセスに戻す
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        }
    }

    public void InputMixer()
    {
        stickframe++;
        if (stickframe >= 2)
        {
            stickframe = 0;
            Vector2 stickVec = GamePad.GetAxis(GamePad.Axis.LeftStick, playerControllerNumber);
            Angle = AngleWithSign(oldStickVec, stickVec);
            if (Angle != 90)
            {
                AngleSum += Angle;
                if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetMiniGameUI().GetComponent<MixerMiniGame>().GetRotation())
                {
                    if ((int)AngleSum <= -360 * (rotationNum + 1))
                    {
                        rotationNum++;
                        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().OneRotation())
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
                        player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().OneRotation();
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
            if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>().GetIceBoxID() == 0)
            {
                    Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Icebreak),18);
            }
            else
            {
                    Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Icebreak),19);
            }
            
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox).GetComponent<IceBox>().ActionMiniGame();
        }
    }

    public void InputDastBox()
    {
        if (GamePad.GetButton(GamePad.Button.B, playerControllerNumber))
        {
            player_cs.GetDastBoxUI().GetComponent<DastBox>().Action();
        }
        // 途中でボタンを離した時
        else if (GamePad.GetButtonUp(GamePad.Button.B, playerControllerNumber))
        {
            player_cs.GetDastBoxUI().SetActive(false);
            //アナウンスUIを再表示
            player_cs.SetAnnounceSprite((int)PlayerCollision.hitObjName.DastBox);
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

    private bool InputDownButton(GamePad.Button button) => GamePad.GetButtonDown(button, playerControllerNumber);


	public GamePad.Index GetPlayerControllerNumber() => playerControllerNumber;

	private void SetInputAxisName(string x, string y)
    {
        inputXAxisName = x;
        inputYAxisName = y;
    }

    public string GetInputXAxisName() => inputXAxisName;

    public string GetInputYAxisName() => inputYAxisName;

    private void SetPlayerControllerNumber(GamePad.Index index)
    {
        if (index < GamePad.Index.One || index > GamePad.Index.Four)
        {
            Debug.LogError("不正なコントローラindex");
            return;
        }
        playerControllerNumber = index;
    }

    public void InitMixerVariable()
    {
        rotationNum = 0;
        AngleSum = 0;
        AngleSum = 0;
    }
}



