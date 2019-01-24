using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GamepadInput;
using UnityEngine.SceneManagement;

public class YNCtrl : MonoBehaviour {

    [SerializeField] private GameObject _symbol;
    [SerializeField] private GameObject _skipLogo;
    [SerializeField] private GameObject _yes, _no;

    private AnimatorStateInfo _animState;

    private GameObject[] _players;

    private bool _isLeftChose = true;
    private float _logoScale = 0.5f;

    private bool _isNextScene = false;

    void Awake()
    {
        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
        Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Menu));
    }

    void Start () {
        _skipLogo.SetActive(false);

        _players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	void Update () {

        _animState = _symbol.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (_animState.IsName("Base Layer.move"))
        {
            _skipLogo.SetActive(true);
        }
        else
        {
            return;
        }

        if (_isNextScene) {
            if(Sound.IsPlayingSe(SoundController.GetMenuSEName(SoundController.MenuSE.decisionkey_share), 12))
            {
                return;
            }

            if (_isLeftChose)
            {
                SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Tutorial);
            }
            else
            {
                SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Game);
            }
            return;
        }

        foreach (GameObject player in _players)
        {
            // 1pのみ
            if (player.GetComponent<Player>().GetPlayerID() != 0)
            {
                continue;
            }
            if (player.GetComponent<Player>().InputDownButton(GamePad.Button.RightShoulder))
            {
                _isLeftChose = !_isLeftChose;
                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.textslide_share), 11);
            }
            else if (player.GetComponent<Player>().InputDownButton(GamePad.Button.LeftShoulder))
            {
                _isLeftChose = !_isLeftChose;
                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.textslide_share), 11);
            }
            if (player.GetComponent<Player>().InputDownButton(GamePad.Button.B))
            {
                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.decisionkey_share), 12);

                _isNextScene = true;
            }

            player.GetComponent<Player>().PlayerUpdate();
        }

        //if (player.GetComponent<Player>().InputDownButton(GamePad.Button.RightShoulder))
        //{
        //    _isLeftChose = !_isLeftChose;
        //}
        //else if (player.GetComponent<Player>().InputDownButton(GamePad.Button.LeftShoulder))
        //{
        //    _isLeftChose = !_isLeftChose;
        //}

        // 左右入力
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _isLeftChose = !_isLeftChose;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _isLeftChose = !_isLeftChose;
        }

        // 入力
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_isLeftChose)
            {
                SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Tutorial);
            }
            else
            {
                SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Game);
            }
        }


        // 大小表示
        if (_isLeftChose)
        {
            _yes.GetComponent<SpriteRenderer>().color = Color.black;
            _yes.transform.localScale = new Vector3(1, 1, 1);

            _no.GetComponent<SpriteRenderer>().color = Color.gray;
            _no.transform.localScale = new Vector3(_logoScale, _logoScale, 1);

        }
        else
        {
            _yes.GetComponent<SpriteRenderer>().color = Color.gray;
            _yes.transform.localScale = new Vector3(_logoScale, _logoScale, 1);

            _no.GetComponent<SpriteRenderer>().color = Color.black;
            _no.transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
