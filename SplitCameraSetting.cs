using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCameraSetting: MonoBehaviour {
    // カメラの分割方法
    public enum SplitCameraMode
    {
        horizontal,
        vertical,
        square
    }

    
    public SplitCameraMode mode;    // カメラ分割モード

    // 各プレイヤーカメラ
    public Camera player1Camera;    
    public Camera player2Camera;
    public Camera player3Camera;
    public Camera player4Camera;

    //初期処理
    void Start ()
    {
        SettingCameraSplit();
    }


    private void SettingCameraSplit()
    {
        switch (mode)
        {
            case SplitCameraMode.horizontal:
                SettingHorizontal();
                break;

            case SplitCameraMode.vertical:
                SettingVirtical();
                break;

            case SplitCameraMode.square:
                SettingSquare();
                break;

            default:
                break;
        }
    }

    //左右分割
    private void SettingHorizontal()
    {
        InactivePlayerCamera(player3Camera);
        InactivePlayerCamera(player4Camera);
        player1Camera.rect = new Rect(0f, 0f, 0.5f, 1f);
        player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }

    //上下分割
    private void SettingVirtical()
    {
        InactivePlayerCamera(player3Camera);
        InactivePlayerCamera(player4Camera);
        player1Camera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
        player2Camera.rect = new Rect(0f, 0f, 1f, 0.5f);
    }

    //４分割
    private void SettingSquare()
    {
        player1Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
        player2Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        player3Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        player4Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
    }

    // カメラ非アクティブ
    private void InactivePlayerCamera(Camera camera)
    {
        camera.gameObject.SetActive(false);
    }
}
