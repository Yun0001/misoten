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
    void Update()
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
                InputKeyBoard_Player2();
                break;
        }
    }

    private void InputGamepad()
    {
        AllStatus();

        /*
        switch (player_cs.GetPlayerStatus())
        {
            case Player.PlayerStatus.Normal:
                NormalStatus();
                break;

            case Player.PlayerStatus.Catering:
                CateringStatus();
                break;

            case Player.PlayerStatus.TasteCharge:
                TasteChargeStatus();
                break;

            default:
                return;
        }
        */
        playerMove_cs.SetMove(new Vector3(Input.GetAxis(inputXAxisName), 0, -(Input.GetAxis(inputYAxisName))));

    }


    private void InputKeyBoard_Player2()
    {
        if (Input.GetKey(KeyCode.A)) playerMove_cs.SetMove(PlayerMove.EDirection.Left);
        if (Input.GetKey(KeyCode.D)) playerMove_cs.SetMove(PlayerMove.EDirection.Right);
        if (Input.GetKey(KeyCode.W)) playerMove_cs.SetMove(PlayerMove.EDirection.Up);
        if (Input.GetKey(KeyCode.S)) playerMove_cs.SetMove(PlayerMove.EDirection.Down);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            player_cs.ActionMicrowave();
            player_cs.ActionPot();
            player_cs.ActionGrilled();
            player_cs.OfferCuisine();
        }
        if (Input.GetKeyDown(KeyCode.X)) hindrance_cs.DisplayTasteGage();
        else if (Input.GetKey(KeyCode.X)) hindrance_cs.UpgateTasteGage();
        else if (Input.GetKeyUp(KeyCode.X)) hindrance_cs.SprinkleTaste();
    }

    private void InputKeyBoard_Player3()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Left);
        if (Input.GetKey(KeyCode.RightArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Right);
        if (Input.GetKey(KeyCode.UpArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Up);
        if (Input.GetKey(KeyCode.DownArrow)) playerMove_cs.SetMove(PlayerMove.EDirection.Down);
        if (Input.GetKeyDown(KeyCode.K))
        {
            player_cs.ActionMicrowave();
            player_cs.ActionPot();
            player_cs.ActionGrilled();
            player_cs.OfferCuisine();
        }
        if (Input.GetKeyDown(KeyCode.L)) hindrance_cs.DisplayTasteGage();
        else if (Input.GetKey(KeyCode.L)) hindrance_cs.UpgateTasteGage();
        else if (Input.GetKeyUp(KeyCode.L)) hindrance_cs.SprinkleTaste();
    }

    private void AllStatus()
    {
        /*
        // Aボタン入力（電子レンジ）
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber)) player_cs.ActionMicrowave();
        // Xボタン入力（鍋）
        if (GamePad.GetButtonDown(GamePad.Button.X, PlayerControllerNumber)) player_cs.ActionPot();
        // Bボタン入力（フライパン）
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber)) player_cs.ActionGrilled();
        // Yボタン入力（キャンセル）
        if (GamePad.GetButtonDown(GamePad.Button.Y, PlayerControllerNumber)) player_cs.CookingCancel();
        */

        // アクション
        if (GamePad.GetButtonDown(GamePad.Button.B, PlayerControllerNumber)) player_cs.ActionBranch();

        // Yボタン入力（キャンセル）
        if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber)) player_cs.CookingCancel();
    }

    private void NormalStatus()
    {
        //Rトリガー入力（味の素）
        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, PlayerControllerNumber)) hindrance_cs.DisplayTasteGage();
    }

    private void TasteChargeStatus()
    {
        if (GamePad.GetButton(GamePad.Button.RightShoulder, PlayerControllerNumber)) hindrance_cs.UpgateTasteGage();
        else if (GamePad.GetButtonUp(GamePad.Button.RightShoulder, PlayerControllerNumber)) hindrance_cs.SprinkleTaste();
    }

    private void CateringStatus()
    {
        // Lトリガー入力（配膳）
        if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, PlayerControllerNumber)) player_cs.OfferCuisine();
    }
}



