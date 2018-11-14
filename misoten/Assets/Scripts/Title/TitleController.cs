using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class TitleController : MonoBehaviour {

    public bool isStartingGame = false;
    private bool isCameraMoved = false;

    private GameObject mainCamera;
    private Animator doorrAnimator;
    private Animator doorlAnimator;

    // カメラが移動した際の終点座標
    private static readonly Vector3 endPosition = new Vector3(0.0f, -1.5f, -4.5f);

    [SerializeField, Range(0.5f, 1)]
    float cameraMoveTime = 0.75f;

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

    }

    void OnEnable()
    {
      
    }

    void Update () {

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

}
