using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{

    // ディスプレイの数だけカメラを配置する

    
    [SerializeField]
    private int m_useDisplayCount;
    private void Awake()
    {
        // ディスプレイの数を取得
        int count = Mathf.Min(Display.displays.Length, m_useDisplayCount);

        //ディスプレイをアクティブにする
        for (int i = 0; i < count; i++)
            Display.displays[i].Activate();
        
    }
}
