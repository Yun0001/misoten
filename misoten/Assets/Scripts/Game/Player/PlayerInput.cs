using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerInput : MonoBehaviour
{

    private Player player_cs;
    private PlayerMove playerMove_cs;
    private int playerID;
    private GamePad.Index playerNumber;
    private string inputXAxisName;
    private string inputYAxisName;

    // Use this for initialization
    public void Init(string xaxisname,string yaxisdname)
    {
        player_cs = GetComponent<Player>();
        playerMove_cs = GetComponent<PlayerMove>();
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
                InputKeyBoard_Player2();
                break;

            case 2:
                InputKeyBoard_Player3();
                    break;

            case 3:
                InputGamepad();
                break;
        }
    }

    private void InputGamepad()
    {
        // 補充マシーンとの当たり判定を取得
        if (player_cs.GetHitObj((int)Player.hitObjName.TasteMachine) != null)
        {
            // 当たっていればホールドで取得
            // ボタンを離すと初期化
            if (GamePad.GetButton(GamePad.Button.LeftShoulder, playerNumber)) player_cs.GetPowderSetScript().PowderSet();
            else if (GamePad.GetButtonUp(GamePad.Button.LeftShoulder, playerNumber)) player_cs.GetPowderSetScript().InitSet();
        }
        // Aボタン入力（電子レンジ）
        if (GamePad.GetButtonDown(GamePad.Button.A, playerNumber)) player_cs.ActionMicrowave();
        // Xボタン入力（鍋）
        if (GamePad.GetButtonDown(GamePad.Button.X, playerNumber)) player_cs.ActionPot();
        // Bボタン入力（フライパン）
        if (GamePad.GetButtonDown(GamePad.Button.B, playerNumber)) player_cs.ActionGrilled();
        // Yボタン入力（邪魔）
        if (GamePad.GetButtonDown(GamePad.Button.Y, playerNumber)) player_cs.ActionHindrance();
        // Rトリガー入力（配膳）
        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, playerNumber)) player_cs.OfferCuisine();

        playerMove_cs.SetMove(new Vector2(Input.GetAxis(inputXAxisName) * 5, -(Input.GetAxis(inputYAxisName) * 5)));

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
        if (Input.GetKeyDown(KeyCode.X)) player_cs.ActionHindrance();
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
        if (Input.GetKeyDown(KeyCode.L)) player_cs.ActionHindrance();
    }
}



