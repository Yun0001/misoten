using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

    public bool isStartingGame = false;
    private bool isCameraMoved = false;

    private GameObject mainCamera;
    private Animator shopAnimator;

    // カメラが移動した際の終点座標
    private static readonly Vector3 endPosition = new Vector3(1.73f, -1.72f, -4.79f);

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
        shopAnimator = GameObject.FindGameObjectWithTag("Shop").GetComponent<Animator>();
        

    }

    void OnEnable()
    {
      
    }

    void Update () {

        if (Input.anyKeyDown)
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
        shopAnimator.Play("open");
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
        //var pos = curve.Evaluate(rate);

        mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, rate);
        //transform.position = Vector3.Lerp (startPosition, endPosition, pos);

        if (mainCamera.transform.position==endPosition) {
            isCameraMoved = true;
            Debug.Log(mainCamera.transform);
        }
    }

}
