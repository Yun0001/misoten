using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerInput : MonoBehaviour
{
    private int playerID;
    private GamePad.Index playerNumber;
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
        playerNumber = player_cs.GetPlayerNumber();
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
        // Aボタン入力（電子レンジ）
        if (GamePad.GetButtonDown(GamePad.Button.A, playerNumber)) player_cs.ActionMicrowave();
        // Xボタン入力（鍋）
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNumber)) player_cs.ActionPot();
        // Bボタン入力（フライパン）
        if (GamePad.GetButtonDown(GamePad.Button.B, playerNumber)) player_cs.ActionGrilled();
        // Yボタン入力（キャンセル）
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNumber)) player_cs.CookingCancel();
        // Lトリガー入力（配膳）
        if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, playerNumber)) player_cs.OfferCuisine();
        //Rトリガー入力（味の素）
        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, playerNumber)) hindrance_cs.DisplayTasteGage();
        else if (GamePad.GetButton(GamePad.Button.RightShoulder, playerNumber)) hindrance_cs.UpgateTasteGage();
        else if (GamePad.GetButtonUp(GamePad.Button.RightShoulder, playerNumber)) hindrance_cs.SprinkleTaste();
        
        playerMove_cs.SetMove(new Vector2(Input.GetAxis(inputXAxisName), -(Input.GetAxis(inputYAxisName))));
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
}



