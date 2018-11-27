﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constants;

public class TutorialCtrl : MonoBehaviour
{
    [SerializeField] bool isDebugMode = false;
    [SerializeField, Range(Tutorial.NO_1, Tutorial.NO_6)] int DebugTutorialNum = Tutorial.NO_1;
    [SerializeField] bool isOnce = false;

    private int             CURRENT_TUTORIAL_STATE;

    private GameObject      _tutorialMenu;
    private MenuAnimCtrl    _menuAnimCtrl;

    private GameObject      _tutorialMenuUI;
    private SpriteRenderer  _menuSprite;

    private TutorialFrow    _tutorialFrow;

    private GameObject[]    _players;

    private GameObject[]    _eatoys;

    private Sprite[] _eatoySprites;
    private void Awake()
    {
        _eatoySprites = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
    }

    void Start()
    {
        StartCoroutine("OpenMenu");     // Open Menu Coroutine

        if (isDebugMode) {
            CURRENT_TUTORIAL_STATE = DebugTutorialNum;
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

        _eatoys = GameObject.FindGameObjectsWithTag("Eatoy");

        // ページをめくってるならば非表示
        if (IsOverPage()) {
            
            if (_eatoys.Length != 0)
            {
                foreach (GameObject eatoy in _eatoys)
                {
                    eatoy.GetComponent<SpriteRenderer>().enabled = false;
                    GameObject child1 = eatoy.transform.GetChild(0).gameObject;
                    GameObject child2 = child1.transform.GetChild(0).gameObject;
                    child2.SetActive(false);
                    child1.SetActive(false);
                    eatoy.SetActive(false);
                }
            }
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

            if (_eatoys.Length != 0)
            {
                foreach (GameObject eatoy in _eatoys)
                {
                    eatoy.SetActive(true);
                    GameObject child1 = eatoy.transform.GetChild(0).gameObject;
                    GameObject child2 = child1.transform.GetChild(0).gameObject;
                    eatoy.GetComponent<SpriteRenderer>().enabled = true;
                    child1.SetActive(true);
                    child2.SetActive(true);
                }
            }

            return;
        }

    }

    void TutorialState()
    {
        if (CheckAllPlayerPlayComplete())
        {
            NextTutorial();
        }

        TutorialDebug();

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
        foreach (GameObject player in _players) player.GetComponent<TutorialPlayer>().SetPlayerReder(false);

        CURRENT_TUTORIAL_STATE++;
        if (Tutorial.ERROR <= CURRENT_TUTORIAL_STATE)
        {
            CURRENT_TUTORIAL_STATE = (Tutorial.ERROR - 1);
        }

        OverPage();
    }

    void TutorialDebug()
    {
        // チュートリアルが遷移してから一度きり
        if (isOnce)
        {
            switch (CURRENT_TUTORIAL_STATE)
            {
                case Tutorial.NO_1:
                    break;
                case Tutorial.NO_2:
                    foreach (GameObject player in _players)
                    {
                        player.GetComponent<PlayerHaveInEatoy>().SetEatoy(
                            Instantiate(
                                Resources.Load("Prefabs/Eatoy/Eatoy") as GameObject));
                        player.GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().Init(
                            1,
                            _eatoySprites[0]
                            );
                        // todo : scale調整

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
