﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constants;

public class TutorialCtrl : MonoBehaviour
{
    [SerializeField] bool isDebugMode = false;
    [SerializeField, Range(Tutorial.NO_1 + 1, Tutorial.NO_6 + 1)] int DebugTutorialNum = Tutorial.NO_1;
    [SerializeField] bool isNextTutorial = false;

    private int             CURRENT_TUTORIAL_STATE;

    private GameObject      _tutorialMenu;
    private MenuAnimCtrl    _menuAnimCtrl;

    private GameObject      _tutorialMenuUI;
    private SpriteRenderer  _menuSprite;

    private TutorialFrow    _tutorialFrow;
    bool isOnce = false;

    private GameObject[]    _players;

    //private GameObject[]    _eatoys;
    private Sprite[]        _eatoySprites;

    private void Awake()
    {
        _eatoySprites = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
    }

    void Start()
    {
        StartCoroutine("OpenMenu");     // Open Menu Coroutine

        if (isDebugMode) {
            isOnce = true;
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

    }

    void Update()
    {
        TutorialState();
        TutorialRenderer();
    }

    private IEnumerator OpenMenu()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME);
        _menuAnimCtrl.OpenMenu();
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
        if (CheckAllPlayerPlayComplete()|| isNextTutorial)
        {
            NextTutorial();
        }


        OnetimeOnlyOnTutorialStart();   // 各チュートリアル開始時一度きりだけ処理される

    }

    bool CheckAllPlayerPlayComplete()
    {
        foreach (GameObject player in _players)
        {
            bool isNo = !(player.GetComponent<TutorialPlayer>().IsComplete());
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
        foreach (GameObject player in _players)
        {
            player.GetComponent<Player>().AllNull();
            player.GetComponent<TutorialPlayer>().SetPlayerReder(false);
        }
        isNextTutorial = false;

        CURRENT_TUTORIAL_STATE++;
        isOnce = true;

        if (Tutorial.ERROR <= CURRENT_TUTORIAL_STATE)
        {
            CURRENT_TUTORIAL_STATE = (Tutorial.ERROR - 1);
        }

        OverPage();
    }

    void OnetimeOnlyOnTutorialStart()
    {
        // チュートリアルが遷移してから一度きり
        if (isOnce)
        {
            switch (CURRENT_TUTORIAL_STATE)
            {
                case Tutorial.NO_1: // 冷蔵庫
                    break;

                case Tutorial.NO_5: // ミキサー
                    int[] nums = { 1, 3, 3, 5 };
                    int i = 0;
                    foreach (GameObject player in _players)
                    {
                        if (player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy() != null)
                        {
                            player.GetComponent<PlayerHaveInEatoy>().RevocationHaveInEatoy(true);
                        }
                        player.GetComponent<PlayerHaveInEatoy>().SetEatoy(Instantiate(Resources.Load("Prefabs/Eatoy/Eatoy") as GameObject));
                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Init(nums[i], _eatoySprites[nums[i]]);
                        Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().transform.localScale = scale;

                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Thawing();

                        player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Catering);
                        player.GetComponent<PlayerAnimCtrl>().SetServing(true);

                        i++;
                    }
                    break;

                default:
                    foreach (GameObject player in _players)
                    {
                        if (player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy() != null)
                        {
                            player.GetComponent<PlayerHaveInEatoy>().RevocationHaveInEatoy(true);
                        }
                        player.GetComponent<PlayerHaveInEatoy>().SetEatoy(Instantiate(Resources.Load("Prefabs/Eatoy/Eatoy") as GameObject));
                        int num = Random.Range(0, 4);
                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Init(num, _eatoySprites[num]);
                        Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().transform.localScale = scale;
                        player.GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
                        player.GetComponent<PlayerAnimCtrl>().SetServing(true);
                    }
                    break;

            }
        }

        isOnce = false;

    }

    void OverPage() => _menuAnimCtrl.OverPage();
    void CloseMenu() => _menuAnimCtrl.CloseMenu();
    bool IsOverPage() => _menuAnimCtrl.IsOverPage();
}
