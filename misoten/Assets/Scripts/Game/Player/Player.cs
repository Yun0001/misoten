using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;


public class Player : MonoBehaviour
{

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


    [SerializeField]
    private PlayerStatus playerStatus;

    [SerializeField]
    private int playerID;//プレイヤーID(インスペクターで設定)

    private GamePad.Index playerControllerNumber;
    private string inputXAxisName;
    private string inputYAxisName;

    private PlayerMove playerMove_cs;
    private PlayerHaveInEatoy haveInEatoy_cs;
    private PlayerAccessController playerAccessController_cs;
    private PlayerAccessPossiblAnnounce playerAccessPosssibleAnnounce_cs;
    private PlayerCollision collision_cs;
    private HaveEatoyCtrl haveEatoyCtrl_cs;
    private PlayerStateBase status_cs;

    [SerializeField]
    private GameObject timeManager;

    private GameObject dastBoxGage;

    // Use this for initialization
    void Awake()
    {
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
        gameObject.AddComponent<NormalState>();
        SetPlayerStatus(PlayerStatus.Normal);
        SetScript();
        playerMove_cs.Init();
        dastBoxGage = Instantiate(Resources.Load("Prefabs/DastBoxUI") as GameObject, transform.position, Quaternion.identity, transform);
        GetDastBoxUI().SetActive(false);
    }

    /// <summary>
    /// プレイヤー移動
    /// </summary>
    public void PlayerFixedUpdate() => playerMove_cs.Move();

    // Update is called once per frame
    public void PlayerUpdate()
    {
        if (timeManager.GetComponent<GameTimeManager>().IsTimeUp())
        {
            StopMove();
            return;
        }

        status_cs.InputState();
        status_cs.UpdateState();
        haveEatoyCtrl_cs.HaveEatoy();

        // 移動量セット
        playerMove_cs.SetMove(new Vector3(Input.GetAxis(inputXAxisName), 0, -(Input.GetAxis(inputYAxisName))));
    }

    public void HiddenAnnounceSprite() => playerAccessPosssibleAnnounce_cs.HiddenSprite();

    public void SetAnnounceSprite(int spriteID) => playerAccessPosssibleAnnounce_cs.SetSprite(spriteID);

    public GameObject IsObjectCollision(PlayerCollision.hitObjName ObjID) => collision_cs.GetHitObj(ObjID);

    public void SetHaveInEatoy(GameObject eatoy)
    {
        haveInEatoy_cs.SetEatoy(eatoy);
        SetHaveEatoyCtrlNum((int)GetHaveInEatoyColor());
    }

    public Eatoy.EEatoyColor GetHaveInEatoyColor() => haveInEatoy_cs.GetHaveInEatoy().GetComponent<Eatoy>().GetEatoyColor();

    public void RevocationHaveInEatoy(bool b) => haveInEatoy_cs.RevocationHaveInEatoy(b);


    public int GetPlayerID() => playerID;

    public GameObject GetDastBoxUI() => dastBoxGage;

    public PlayerStatus GetPlayerStatus() => playerStatus;

    public void StopMove()
    {
        playerMove_cs.VelocityReset();
        GetComponent<PlayerAnimCtrl>().SetWalking(false);
    }

    private void SetScript()
    {
        status_cs = GetComponent<NormalState>();
        playerMove_cs = GetComponent<PlayerMove>();
        haveInEatoy_cs = GetComponent<PlayerHaveInEatoy>();
        playerAccessController_cs = GetComponent<PlayerAccessController>();
        playerAccessPosssibleAnnounce_cs = GetComponent<PlayerAccessPossiblAnnounce>();
        collision_cs = GetComponent<PlayerCollision>();
        haveEatoyCtrl_cs = GetComponent<HaveEatoyCtrl>();
    }

    public void SetPlayerStatus(PlayerStatus state) => playerStatus = state;

    public void SetHaveEatoyCtrlNum(int num) => haveEatoyCtrl_cs.SetEatoyNum(num);

    public PlayerHaveInEatoy GetHaveInEatoy_cs() => haveInEatoy_cs;

    public bool IsEatoyIceing() => haveInEatoy_cs.GetHaveInEatoy().GetComponent<Eatoy>().IsIcing();

    public void AllNull() => collision_cs.AllNull();

    public void DisplaySandbySprite() => playerAccessPosssibleAnnounce_cs.DisplayStandbySprite();

    public void HiddenStandbySprite() => playerAccessPosssibleAnnounce_cs.HiddenStandbySprite();

    public bool InputDownButton(GamePad.Button button) => GamePad.GetButtonDown(button, playerControllerNumber);

    public bool InputUpButton(GamePad.Button button) => GamePad.GetButtonUp(button, playerControllerNumber);

    public bool InputButton(GamePad.Button button) => GamePad.GetButton(button, playerControllerNumber);

    public PlayerAccessController GetAccessController() => playerAccessController_cs;

    public PlayerAccessPossiblAnnounce GetAccessPossibleAnnounce() => playerAccessPosssibleAnnounce_cs;

    public void ChangeAttachComponent(int stateID)
    {
        // 現在アタッチされている状態Componentを削除
        if (status_cs != null)
        {
            status_cs.DeleteComponent();
        }
        

        // 新しい状態のComponentをアタッチ
        switch (stateID)
        {
            case (int)PlayerStatus.Normal:
                gameObject.AddComponent<NormalState>();
                status_cs = GetComponent<NormalState>();
                break;

            case (int)PlayerStatus.Microwave:
                gameObject.AddComponent<MicrowaveState>();
                status_cs = GetComponent<MicrowaveState>();
                status_cs.AccessAction();
                break;

            case (int)PlayerStatus.Pot:
                gameObject.AddComponent<PotState>();
                status_cs = GetComponent<PotState>();
                status_cs.AccessAction();
                break;

            case (int)PlayerStatus.GrilledTable:
                gameObject.AddComponent<FlyingpanState>();
                status_cs = GetComponent<FlyingpanState>();
                status_cs.AccessAction();
                break;

            case (int)PlayerStatus.Mixer:
                gameObject.AddComponent<MixerState>();
                status_cs = GetComponent<MixerState>();
                status_cs.AccessAction();
                break;

            case (int)PlayerStatus.IceBox:
                gameObject.AddComponent<IceBoxState>();
                status_cs = GetComponent<IceBoxState>();
                status_cs.AccessAction();
                break;

            case (int)PlayerStatus.DastBox:
                gameObject.AddComponent<DastBoxState>();
                status_cs = GetComponent<DastBoxState>();
                status_cs.AccessAction();
                break;
        }
    }

    private void SetInputAxisName(string x, string y)
    {
        inputXAxisName = x;
        inputYAxisName = y;
    }

    public string GetInputXAxisName() => inputXAxisName;

    public string GetInputYAxisName() => inputYAxisName;

    public GamePad.Index GetPlayerControllerNumber() => playerControllerNumber;

    private void SetPlayerControllerNumber(GamePad.Index index)
    {
        if (index < GamePad.Index.One || index > GamePad.Index.Four)
        {
            Debug.LogError("不正なコントローラindex");
            return;
        }
        playerControllerNumber = index;
    }
}
