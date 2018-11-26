using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // ui panel
using UnityEngine.SceneManagement;

public class WhiteIO : MonoBehaviour {

    private Color _color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private bool isWhiteOut = false;
    private bool isWhiteIn = false;
    private bool isRsWhiteOut = false;
    private Image _whiteOutImg;
    private Image _titleLogoImg;
    private Image _pushImg;
    private GameObject _titleCtrl;

    [SerializeField, Range(0.01f, 0.05f)] float fadeInSpeed = 0.02f;
    [SerializeField, Range(0.01f, 0.05f)] float fadeOutSpeed = 0.02f;

    private float _pushImgAlpha = 0.0f;
    private bool _retAlpha = false;

    void Start()
    {
        _whiteOutImg = GetComponent<Image>();
        _titleLogoImg = GameObject.Find("titleLogo").GetComponent<Image>();
        _titleCtrl = GameObject.Find("TitleController");
        _pushImg = GameObject.Find("push").GetComponent<Image>();
    }

    void Update()
    {
        PushImgAnimation();

        if (isWhiteIn)
        {
            StartWhiteIn();
        }
        if (isWhiteOut)
        {
            StartWhiteOut();
        }
        if (isRsWhiteOut)
        {
            RestartWhiteOut();
        }
    }

    void StartWhiteOut()
    {
        _pushImg.enabled = false;

        _whiteOutImg.enabled = true;
        _color.a += fadeOutSpeed;

        _whiteOutImg.color = _color;

        if (_color.a >= 1.0f)
        {
            isWhiteOut = false;

            NextScene();

        }

    }

    void StartWhiteIn()
    {
        _color.a -= fadeInSpeed / 2;

        _whiteOutImg.color = _color;
        _titleLogoImg.color = _color;

        if (_color.a <= 0.0f)
        {
            isWhiteIn = false;
            _color.a = 0.0f;
            _whiteOutImg.enabled = false;
            _titleLogoImg.enabled = false;
            _titleCtrl.GetComponent<TitleController>().OnIsCompleteTitleLogoAnimation();

            _pushImg.enabled = true;
        }

    }

    void RestartWhiteOut()
    {
        _pushImg.enabled = false;

        _whiteOutImg.enabled = true;
        _color.a += fadeOutSpeed;

        _whiteOutImg.color = _color;

        if (_color.a >= 1.0f)
        {
            isRsWhiteOut = false;

            Scene loadScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadScene.name);

        }

    }

    void PushImgAnimation()
    {
        if (_pushImg.enabled)
        {
            Color clr = new Color(1, 1, 1, _pushImgAlpha);
            _pushImg.color = clr;

            if (_retAlpha)
            {
                _pushImgAlpha -= 0.01f;
                if (_pushImgAlpha <= 0.1f)
                {
                    _retAlpha = !_retAlpha;
                }
            }
            else
            {
                _pushImgAlpha += 0.01f;
                if (_pushImgAlpha >= 1.0f)
                {
                    _retAlpha = !_retAlpha;
                }
            }
        }
    }

    void NextScene()
    {
        //SceneManager.LoadScene("Scenes/3dNewScene");
        SceneManagerScript.GetInstance().LoadNextScene();
    }

    public void OnWhiteOut()
    {
        isWhiteOut = true;
    }

    public void OnWhiteIn()
    {
        isWhiteIn = true;
    }

    public void OnRsWhiteOut()
    {
        isRsWhiteOut = true;
    }

    public bool GetRsWhiteOut() => isRsWhiteOut;

}
