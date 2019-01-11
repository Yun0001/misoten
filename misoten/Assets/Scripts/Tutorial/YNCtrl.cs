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

    private bool _isLeftChose = true;
    private float _logoScale = 0.5f;
 
    void Start () {
        _skipLogo.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _isLeftChose = !_isLeftChose;
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
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
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

    }
}
