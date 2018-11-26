using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    [SerializeField] private bool isCompleteTitleLogoAnimation = false;
    [SerializeField] private bool isStartingGame = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    private GameObject mainCamera;
    [SerializeField, Range(0.5f, 1)]
    private bool isCameraMoved = false;
    float cameraMoveTime = 0.75f;
    private static readonly Vector3 endPosition = new Vector3(0.0f, -1.5f, -4.5f); // カメラが移動した際の終点座標

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
        
        // ショップオブジェクト取得
        doorrAnimator = GameObject.Find("doorr").GetComponent<Animator>();
        doorlAnimator = GameObject.Find("doorl").GetComponent<Animator>();

        // BGM再生
        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;

        _audioSource.Play();

        _canvasWIO = GameObject.Find("WIO");

    }

    private void OnDisable()
    {
        _audioSource.Stop();
    }

    void Update () {

        // タイトルBGMが終了でタイトルシーンの再ロード
        if (!_audioSource.isPlaying)
        {
            _canvasWIO.GetComponent<WhiteIO>().OnRsWhiteOut();
        }

        // タイトルロゴのアニメーションが終わってない場合以下スルー
        if (!isCompleteTitleLogoAnimation)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isStartingGame)
            {
                startTime = Time.timeSinceLevelLoad;
            }

            isStartingGame = true;
        }
        else if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        {
            if (!isStartingGame)
            {
                startTime = Time.timeSinceLevelLoad;
            }

            isStartingGame = true;
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

}
