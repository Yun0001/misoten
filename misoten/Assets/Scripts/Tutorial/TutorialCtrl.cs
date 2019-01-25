﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GamepadInput;
using Constants;

public class TutorialCtrl : MonoBehaviour
{
    [SerializeField] bool isDebugMode = false;
    [SerializeField] bool isTestMode = false;
    [SerializeField, Range(Tutorial.NO_1 + 1, Tutorial.NO_7 + 1)] int DebugTutorialNum = Tutorial.NO_1;
    [SerializeField] bool isNextTutorial = false;
    private bool _isCallNextTutorial = false;

    private int             CURRENT_TUTORIAL_STATE;

    private GameObject      _tutorialMenu;
    private MenuAnimCtrl    _menuAnimCtrl;

    private GameObject      _tutorialMenuUI;
    private SpriteRenderer  _menuSprite;

    private TutorialFrow    _tutorialFrow;
    bool isOnce = false;

    private GameObject[]    _players;
    private Vector3[]       _playerInitPos = new Vector3[4];

    //private GameObject[]    _eatoys;
    private Sprite[]        _eatoySprites;
    private Sprite[] _mazuEatoySprites;
    private Sprite[] _kusomazuEatoySprites;

    private void Awake()
    {
        _eatoySprites = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
        _mazuEatoySprites = Resources.LoadAll<Sprite>("Textures/Eatoy/MazuEatoy_BitMap");
        _kusomazuEatoySprites = Resources.LoadAll<Sprite>("Textures/Eatoy/KusoMazuEatoy_BitMap");
    }

    void Start()
    {
        StartCoroutine("OpenMenu");     // Open Menu Coroutine

        if (isTestMode)
        {
            StartCoroutine("TestOver");
        }

        isOnce = true;
        if (isDebugMode) {
            CURRENT_TUTORIAL_STATE = DebugTutorialNum - 1;
        }
        else {
            CURRENT_TUTORIAL_STATE = Tutorial.NO_1;
        }

        _tutorialMenu = GameObject.Find("OBJ_menu");
        _menuAnimCtrl = _tutorialMenu.GetComponent<MenuAnimCtrl>();

        _tutorialMenuUI = GameObject.Find("UI_menu");
        _menuSprite = _tutorialMenuUI.GetComponent<SpriteRenderer>();
        _menuSprite.color = MyColor.ALPHA_0;

        _tutorialFrow = GameObject.Find("TutorialFrow").GetComponent<TutorialFrow>();
       
        _players = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        foreach (GameObject player in _players)
        {
            _playerInitPos[i] = player.transform.position;
            i++;
        }

    }

    void Update()
    {
        int i = 0;
        foreach (GameObject player in _players)
        {
            // ミキサーと最後は以外スキップ可能
            if ((CURRENT_TUTORIAL_STATE != Tutorial.NO_5) && (CURRENT_TUTORIAL_STATE != Tutorial.NO_7))
            {
                // 調理中以外のみスキップ可能
                if ((player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Normal) ||
                    (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy))
                {
                    bool b = player.GetComponent<TutorialPlayer>().IsSkip();
                    if (!b)
                    {
                        if (player.GetComponent<Player>().InputDownButton(GamePad.Button.RightShoulder))
                        {
                            // skip なったら　それ以降　ずっとスキップ              
                            player.GetComponent<TutorialPlayer>().SetIsSkip(true);
                        }
                    }
                    else
                    {
                        player.transform.position = _playerInitPos[i];
                    }
                }
            }

            player.GetComponent<Player>().PlayerUpdate();
            i++;
        }
        DebugKey();

        TutorialState();
        TutorialRenderer();
    }

    private void FixedUpdate()
    {
        foreach (GameObject player in _players)
        {
            player.GetComponent<Player>().PlayerFixedUpdate();
        }
    }

    private IEnumerator OpenMenu()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME);
        _menuAnimCtrl.OpenMenu();
        Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.open),10);
    }

    private IEnumerator TestOver()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME * 10.0f);
        isNextTutorial = true;
        Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.flicker), 11);
    }

    private IEnumerator CallNextTutorial()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME * 3.0f);
        isNextTutorial = true;
    }

    private IEnumerator CallEndTutorial()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME * 12.0f);
        isNextTutorial = true;
    }

    float alpha = 0.0f;
    void TutorialRenderer()
    {
        // メニューを開けていないならば非表示
        if (!(_menuAnimCtrl.IsOpen()))
        {
            _menuSprite.color = MyColor.ALPHA_0;
            alpha = 0.0f;
            return;
        }

        // ページをめくってるならば非表示
        if (IsOverPage()) {
            _menuSprite.color = MyColor.ALPHA_0;
            alpha = 0.0f;
            return;
        }

        if (_menuSprite.color.a <= 1.0f)
        {
            alpha += 0.1f;
            _menuSprite.color = new Color(1, 1, 1, alpha);

            _tutorialFrow.SetTutorial(CURRENT_TUTORIAL_STATE, true);

            foreach (GameObject player in _players) player.GetComponent<TutorialPlayer>().SetPlayerReder(true);

            return;
        }

    }

    void TutorialState()
    {

        if (CheckAllPlayerPlayComplete() && (!_isCallNextTutorial))
        {
            _isCallNextTutorial = true;
            StartCoroutine(CallNextTutorial());
        }

        if ((CURRENT_TUTORIAL_STATE==Tutorial.NO_7)&&(!_isCallNextTutorial))
        {
            _isCallNextTutorial = true;
            StartCoroutine(CallEndTutorial());
        }

        if (isNextTutorial) NextTutorial();

        OnetimeOnlyOnTutorialStart();   // 各チュートリアル開始時一度きりだけ処理される

        CheckAllIsCookingComplete();
    }

    bool CheckAllPlayerPlayComplete()
    {
        foreach (GameObject player in _players)
        {
            bool isSkip = player.GetComponent<TutorialPlayer>().IsSkip();
            bool isComplete= player.GetComponent<TutorialPlayer>().IsComplete();

            bool isNo = !(isSkip | isComplete);
            if (isNo)
            {
                return false;
            }
        }
        return true;
    }

    void NextTutorial()
    {
        foreach (GameObject player in _players)
        {
            player.GetComponent<TutorialPlayer>().UnComplete();
        }

        _tutorialFrow.SetTutorial(CURRENT_TUTORIAL_STATE, false);
        int i = 0;
        foreach (GameObject player in _players)
        {
            player.transform.position = _playerInitPos[i];
            player.GetComponent<Player>().AllNull();
            player.GetComponent<TutorialPlayer>().SetPlayerReder(false);
            i++;
        }
        isNextTutorial = false;
        _isCallNextTutorial = false;

        CURRENT_TUTORIAL_STATE++;
        //if (CURRENT_TUTORIAL_STATE==Tutorial.NO_5)
        //{
        //    CURRENT_TUTORIAL_STATE++;
        //}
        isOnce = true;

        if (Tutorial.ERROR <= CURRENT_TUTORIAL_STATE)
        {
            CURRENT_TUTORIAL_STATE = (Tutorial.ERROR - 1);

            SceneManagerScript.LoadNextScene();
        }

        OverPage();
        Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.flicker),11);
    }

    void OnetimeOnlyOnTutorialStart()
    {
        // チュートリアルが遷移してから一度きり
        if (isOnce)
        {
            // チュートリアルの状態によって分岐
            switch (CURRENT_TUTORIAL_STATE)
            {
                case Tutorial.NO_1: // 冷蔵庫
                    break;

                case Tutorial.NO_5: // ミキサー
                    MixerTutorialSetting();
                    break;

                case Tutorial.NO_7:
                    foreach (GameObject player in _players)
                    {
                        player.GetComponent<TutorialPlayer>().UnSkip();
                    }
                    break;

                default:
                    DefaultTutorialSetting();
                    break;
            }
        }

        isOnce = false;

    }

    void CheckAllIsCookingComplete()
    {
        switch (CURRENT_TUTORIAL_STATE)
        {
            case Tutorial.NO_1: // フリーザ
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
                    {
                        if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                        {
                            Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                        }
                        player.GetComponent<TutorialPlayer>().OnComplete();
                    }
                }
                break;

            case Tutorial.NO_2: // レンジ
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Catering)
                    {
                        if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                        {
                            Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                        }
                        player.GetComponent<TutorialPlayer>().OnComplete();
                    }
                }
                break;

            case Tutorial.NO_3: // ボイル
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Catering)
                    {
                        if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                        {
                            Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                        }
                        player.GetComponent<TutorialPlayer>().OnComplete();
                    }
                }
                break;

            case Tutorial.NO_4: // グリル
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Catering)
                    {
                        if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                        {
                            Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                        }
                        player.GetComponent<TutorialPlayer>().OnComplete();
                    }
                }
                break;

            case Tutorial.NO_5: // ミキサー
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().IsObjectCollision(PlayerCollision.hitObjName.Mixer) != null)
                    {
                        if (player.GetComponent<Player>().IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() == Mixer.Status.End)
                        {
                            if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                            {
                                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                            }
                            player.GetComponent<TutorialPlayer>().OnComplete();
                        }
                    }
                }
                break;

            case Tutorial.NO_6: // リリース
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Normal)
                    {
                        if (!(player.GetComponent<TutorialPlayer>().IsComplete()))
                        {
                            Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.tutorial_success), 12);
                        }
                        player.GetComponent<TutorialPlayer>().OnComplete();
                    }
                }
                break;

            case Tutorial.NO_7: // 
                foreach (GameObject player in _players)
                {
                    player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Normal);
                    player.SetActive(false);
                }
                break;

            default:
                break;

        }
    }

    private void MixerTutorialSetting()
    {
        GameObject.Find("kitchen_limit_wall").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("kitchen_limit_wall").transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = false;
        int[] nums = { 0, 2, 2, 4 };
        int i = 0;
        foreach (GameObject player in _players)
        {
            if (player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy() != null)
            {
                player.GetComponent<PlayerHaveInEatoy>().RevocationHaveInEatoy(true);
            }
            player.GetComponent<PlayerHaveInEatoy>().SetEatoy(Instantiate(Resources.Load("Prefabs/Eatoy/Eatoy") as GameObject));
            player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Init(nums[i], _eatoySprites[nums[i]], _mazuEatoySprites[nums[i]], _kusomazuEatoySprites[nums[i]]);
            Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
            player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().transform.localScale = scale;

            player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Thawing();

            player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Catering);
            player.GetComponent<PlayerAnimCtrl>().SetServing(true);

            player.GetComponent<TutorialPlayer>().UnSkip();

            i++;
        }
    }

    private void DefaultTutorialSetting()
    {
        GameObject.Find("kitchen_limit_wall").transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("kitchen_limit_wall").transform.GetChild(1).gameObject.GetComponent<BoxCollider>().enabled = true;
        foreach (GameObject player in _players)
        {
            if (player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy() != null)
            {
                player.GetComponent<PlayerHaveInEatoy>().RevocationHaveInEatoy(true);
            }
            player.GetComponent<PlayerHaveInEatoy>().SetEatoy(Instantiate(Resources.Load("Prefabs/Eatoy/Eatoy") as GameObject));
            int num = Random.Range(0, 4);
            player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Init(num, _eatoySprites[num], _mazuEatoySprites[num], _kusomazuEatoySprites[num]);
            Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
            player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().transform.localScale = scale;
            player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
            player.GetComponent<PlayerAnimCtrl>().SetServing(true);
            player.GetComponent<Player>().GetAccessPossibleAnnounce_cs().HiddenSprite();
        }
    }

    void DebugKey()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            foreach (GameObject player in _players)
            {
                player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Normal);
                player.GetComponent<TutorialPlayer>().UnSkip();
                player.GetComponent<TutorialPlayer>().OnComplete();
            }
        }

    }


    void OverPage() => _menuAnimCtrl.OverPage();
    void CloseMenu() => _menuAnimCtrl.CloseMenu();
    bool IsOverPage() => _menuAnimCtrl.IsOverPage();
}
