using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constants;

public class TutorialCtrl : MonoBehaviour
{

    private int             CURRENT_TUTORIAL_STATE;

    private GameObject      _tutorialMenu;
    private MenuAnimCtrl    _menuAnimCtrl;

    private GameObject      _tutorialMenuUI;
    private SpriteRenderer  _menuSprite;

    private GameObject[]    _players;


    void Start()
    {
        StartCoroutine("OpenMenu");     // Open Menu Coroutine
        CURRENT_TUTORIAL_STATE = Tutorial.NO_1;

        _tutorialMenu = GameObject.Find("OBJ_menu");
        _menuAnimCtrl = _tutorialMenu.GetComponent<MenuAnimCtrl>();

        _tutorialMenuUI = GameObject.Find("UI_menu");
        _menuSprite = _tutorialMenuUI.GetComponent<SpriteRenderer>();
        _menuSprite.color = MyColor.ALPHA_0;

        _players = GameObject.FindGameObjectsWithTag("Player");
        
    }

    void Update()
    {
        TutorialState();
        TutorialMenuUIRenderer();
    }

    private IEnumerator OpenMenu()
    {
        yield return new WaitForSeconds(Tutorial.WAIT_TIME);
        _menuAnimCtrl.OpenMenu();
    }

    float alpha = 0.0f;
    void TutorialMenuUIRenderer()
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
        }
    }

    void TutorialState()
    {
        if (CheckAllPlayerPlayComplete())
        {
            NextTutorial();
        }

        switch (CURRENT_TUTORIAL_STATE)
        {
            case Tutorial.NO_1:
                break;
        }
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
        CURRENT_TUTORIAL_STATE++;
        Debug.Log(CURRENT_TUTORIAL_STATE);

        OverPage();
    }

    void OverPage() => _menuAnimCtrl.OverPage();
    void CloseMenu() => _menuAnimCtrl.CloseMenu();
    bool IsOverPage() => _menuAnimCtrl.IsOverPage();
}
