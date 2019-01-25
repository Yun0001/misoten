using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;


public class Player : MonoBehaviour
{

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    public enum PlayerStatus
    {
        Microwave,        //電子レンジ
        Pot,                   //鍋
        GrilledTable,       //焼き台
        Mixer,                 // ミキサー
        IceBox,             // 冷蔵庫
        DastBox,            // ゴミ箱
        Normal,             //通常
        CateringIceEatoy,      // 冷凍イートイ
        Catering,           //イートイorチェンジイートイ
        MixerAccess,        // ミキサーアクセス状態
        MixerWait,          // ミキサー他プレイヤー待ち状態
    }

    /// <summary>
    /// 入力を取得する時に使用する情報
    /// </summary>
    public class ControllerInformation
    {
        public string XAxis;
        public string YAxis;
        public GamePad.Index controllerNumber;

        public ControllerInformation(string x, string y, GamePad.Index num)
        {
            XAxis = x;
            YAxis = y;
            controllerNumber = num;
        }
    }

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    private PlayerStatus playerStatus;

    /// <summary>
    /// プレイヤーID(インスペクターで設定)
    /// </summary>
    [SerializeField, Range(0, 3)]
    private int playerID;

    /// <summary>
    /// 入力を取得する時に使用する情報
    /// </summary>
    private ControllerInformation controllerInformation;

    /// <summary>
    /// プレイヤーにアタッチするスクリプトの構造体
    /// </summary>
    private PlayerScriptStructure scriptStructure;

    /// <summary>
    /// ゴミ箱UI
    /// </summary>
    private GameObject dastBoxGage;

    // Use this for initialization
    void Awake()
    {
        switch (GetPlayerID())
        {
            case 0:
                controllerInformation = new ControllerInformation("L_XAxis_1", "L_YAxis_1", GamePad.Index.One);
                break;
            case 1:
                controllerInformation = new ControllerInformation("L_XAxis_2", "L_YAxis_2", GamePad.Index.Two);
                break;
            case 2:
                controllerInformation = new ControllerInformation("L_XAxis_3", "L_YAxis_3", GamePad.Index.Three);
                break;
            case 3:
                controllerInformation = new ControllerInformation("L_XAxis_4", "L_YAxis_4", GamePad.Index.Four);
                break;
        }
        gameObject.AddComponent<NormalState>();
        SetPlayerStatus(PlayerStatus.Normal);
        scriptStructure = new PlayerScriptStructure(
            gameObject, GetComponent<NormalState>(),
            GetComponent<PlayerMove>(), GetComponent<PlayerCollision>(),
            GetComponent<PlayerAccessController>(), GetComponent<PlayerAccessPossiblAnnounce>(),
            GetComponent<PlayerHaveInEatoy>(), GetComponent<HaveEatoyCtrl>());
        dastBoxGage = Instantiate(Resources.Load("Prefabs/DastBoxUI") as GameObject, transform.position, Quaternion.identity, transform);
        GetDastBoxUI().SetActive(false);
    }

    /// <summary>
    /// プレイヤー移動
    /// </summary>
    public void PlayerFixedUpdate() => scriptStructure.GetMove().Move();

    // Update is called once per frame
    public void PlayerUpdate()
    {
        scriptStructure.GetState().InputState();
        scriptStructure.GetState().UpdateState();
        scriptStructure.GetHaveEatoyCtrl().HaveEatoy(0);

        // 移動量セット
        scriptStructure.GetMove().SetMove(new Vector3(Input.GetAxis(controllerInformation.XAxis), 0, -(Input.GetAxis(controllerInformation.YAxis))));
    }

    public GameObject IsObjectCollision(PlayerCollision.hitObjName ObjID) => scriptStructure.GetCollision().GetHitObj(ObjID);

    public void SetHaveInEatoy(GameObject eatoy)
    {
        GetHaveInEatoy_cs().SetEatoy(eatoy);
        scriptStructure.GetHaveEatoyCtrl().SetEatoyNum((int)GetHaveInEatoy_cs().GetHaveInEatoy().GetComponent<Eatoy>().GetEatoyColor());
    }

    public int GetPlayerID() => playerID;

    public GameObject GetDastBoxUI() => dastBoxGage;

    public PlayerStatus GetPlayerStatus() => playerStatus;

    public void StopMove()
    {
        scriptStructure.GetMove().VelocityReset();
        GetComponent<PlayerAnimCtrl>().SetWalking(false);
    }

    public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public PlayerAccessController GetAccessController() => scriptStructure.GetAccessController();

    public PlayerAccessPossiblAnnounce GetAccessPossibleAnnounce_cs() => scriptStructure.GetAccessPossibleAnnounce();

    public void HiddenAnnounceSprite() => GetAccessPossibleAnnounce_cs().HiddenSprite();

    public void SetAnnounceSprite(int spriteID) => GetAccessPossibleAnnounce_cs().SetSprite(spriteID);

    public PlayerHaveInEatoy GetHaveInEatoy_cs() => scriptStructure.GetHaveInEatoy();

    public bool IsEatoyIceing() => GetHaveInEatoy_cs().GetHaveInEatoy().GetComponent<Eatoy>().IsIcing();

    public void AllNull() => scriptStructure.GetCollision().AllNull();

    public bool InputDownButton(GamePad.Button button) => GamePad.GetButtonDown(button, controllerInformation.controllerNumber);

    public bool InputUpButton(GamePad.Button button) => GamePad.GetButtonUp(button, controllerInformation.controllerNumber);

    public bool InputButton(GamePad.Button button) => GamePad.GetButton(button, controllerInformation.controllerNumber);


    /// <summary>
    ///状態コンポーネント変更
    /// </summary>
    /// <param name="stateID"></param>
    public void ChangeAttachComponent(int stateID)
    {
        // 現在アタッチされている状態Componentを削除
        if (scriptStructure.GetState() != null)
        {
            scriptStructure.GetState().DeleteComponent();
        }
        

        // 新しい状態のComponentをアタッチ
        switch (stateID)
        {
            case (int)PlayerStatus.Normal:
                gameObject.AddComponent<NormalState>();
                scriptStructure.SetState(GetComponent<NormalState>());
                break;

            case (int)PlayerStatus.Microwave:
                gameObject.AddComponent<MicrowaveState>();
                scriptStructure.SetState(GetComponent<MicrowaveState>());
                scriptStructure.GetState().AccessAction();
                break;

            case (int)PlayerStatus.Pot:
                gameObject.AddComponent<PotState>();
                scriptStructure.SetState(GetComponent<PotState>());
                scriptStructure.GetState().AccessAction();
                break;

            case (int)PlayerStatus.GrilledTable:
                gameObject.AddComponent<FlyingpanState>();
                scriptStructure.SetState(GetComponent<FlyingpanState>());
                scriptStructure.GetState().AccessAction();
                break;

            case (int)PlayerStatus.Mixer:
                gameObject.AddComponent<MixerState>();
                scriptStructure.SetState(GetComponent<MixerState>());
                scriptStructure.GetState().AccessAction();
                break;

            case (int)PlayerStatus.IceBox:
                gameObject.AddComponent<IceBoxState>();
                scriptStructure.SetState(GetComponent<IceBoxState>());
                scriptStructure.GetState().AccessAction();
                break;

            case (int)PlayerStatus.DastBox:
                gameObject.AddComponent<DastBoxState>();
                scriptStructure.SetState(GetComponent<DastBoxState>());
                scriptStructure.GetState().AccessAction();
                break;
        }
    }

    public ControllerInformation GetControllerInformation() => controllerInformation;
}
