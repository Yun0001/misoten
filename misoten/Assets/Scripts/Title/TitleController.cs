using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    [SerializeField] private bool _isTestMode = false;

    private bool isCompleteTitleLogoAnimation = false;
    private bool isStartingGame = false;

    private bool isOneShot = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    private GameObject mainCamera;
    [SerializeField, Range(0.5f, 1)]
    private bool isCameraMoved = false;
    float cameraMoveTime = 0.75f;
    private static readonly Vector3 endPosition = new Vector3(0.0f, -1.5f, -4.5f); // カメラが移動した際の終点座標

    private GameObject _storyBoard;

    private Animator doorrAnimator;
    private Animator doorlAnimator;

    private GameObject _canvasWIO;

    private float startTime;
    private Vector3 startPosition;

    void Start () {

        if (cameraMoveTime <= 0)
        {
            mainCamera.transform.position = endPosition;
            enabled = false;
            return;
        }

        // メインカメラを取得
        mainCamera = Camera.main.gameObject;
        startPosition = mainCamera.transform.position;

        _storyBoard = GameObject.Find("StoryBoard");

        // ショップオブジェクト取得
        doorrAnimator = GameObject.Find("doorr").GetComponent<Animator>();
        doorlAnimator = GameObject.Find("doorl").GetComponent<Animator>();

        // BGM再生
        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
		//Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Title));

		_audioSource = GetComponent<AudioSource>();
		_audioSource.clip = _clip;
		_audioSource.Play();

        _canvasWIO = GameObject.Find("WIO");

    }

    void Update () {
        // タイトルBGMが終了でタイトルシーンの再ロード
        if (!_audioSource.isPlaying)
        {
            //isStartingGame = true;
            _canvasWIO.GetComponent<WhiteIO>().OnRsWhiteOut();
        }

        // タイトルロゴのアニメーションが終わってない場合以下スルー
        if (!isCompleteTitleLogoAnimation)
        {
            return;
        }

        // 強制ロード
        TitleInputKey();
        if (isStartingGame)
        {
            CameraMove();
            // カメラの移動が完了していない場合以下スルー
            if (!isCameraMoved)
            {
                return;
            }
            TitleAnimation();
            return;
        }

        _storyBoard.GetComponent<StoryBoardCtrl>().SetIsStartStory(true);

        // ストーリーアニメーションが終わってなければ以下スルー
        if (!(_storyBoard.GetComponent<StoryBoardCtrl>().GetIsCompleteStartAnimation()))
        {
            if (_isTestMode) isStartingGame = true;
            return;
        }

        // リスタート中はキー入力不可
        if (!_canvasWIO.GetComponent<WhiteIO>().GetRsWhiteOut()) {
            TitleInputKey();
        }

        // ゲームスタートじゃない場合、以下はスルー
        if (!isStartingGame)
        {
            return;
        }

        CameraMove();

        // カメラの移動が完了していない場合以下スルー
        if (!isCameraMoved)
        {
            return;
        }

        TitleAnimation();
    }

    void TitleAnimation()
    {
        doorrAnimator.Play("open");
        doorlAnimator.Play("open");
    }

    void CameraMove()
    {

        var diff = Time.timeSinceLevelLoad - startTime;
        if (diff > cameraMoveTime)
        {
            mainCamera.transform.position = endPosition;
            enabled = false;
        }

        var rate = diff / cameraMoveTime;
     
        mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, rate);
      
        if (mainCamera.transform.position==endPosition) {
            isCameraMoved = true;
        }
    }

    public void OnIsCompleteTitleLogoAnimation()
    {
        isCompleteTitleLogoAnimation = true;
    }

    public void StopTitleBgm()
    {
        _audioSource.Stop();
    }

    void TitleInputKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isOneShot)
            {
                isOneShot = true;
                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.decisionkey_share), 0);
            }
            if (!isStartingGame)
            {
                startTime = Time.timeSinceLevelLoad;
            }

            isStartingGame = true;
        }
        else if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        {
            if (!isOneShot)
            {
                isOneShot = true;
                Sound.PlaySe(SoundController.GetMenuSEName(SoundController.MenuSE.decisionkey_share), 0);
            }
            if (!isStartingGame)
            {
                startTime = Time.timeSinceLevelLoad;
            }

            isStartingGame = true;
        }
    }

}
