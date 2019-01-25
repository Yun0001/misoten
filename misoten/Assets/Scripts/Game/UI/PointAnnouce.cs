using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAnnouce : MonoBehaviour {

    TextMesh pointText;
    private int activeFrame;
    [SerializeField]
    private Vector3 InitPos;

    private void Awake()
    {
        pointText = GetComponent<TextMesh>();
 
        activeFrame = 0;
    }


    private void Update()
    {
        // アクティブ状態なら
        if (gameObject.activeInHierarchy)
        {
            if (activeFrame < 60)
            {
                // 徐々にｙ座標を加算        
                Vector3 pos = transform.position;
                pos.y += 0.01f;
                transform.position = pos;
                activeFrame++;
            }
            else
            {
                // 一定フレーム経過後非表示
                HiddenText();
                activeFrame = 0;
            }
        }
    }

    public void DisplayText(string text)
    {
        transform.position = InitPos;
        gameObject.SetActive(true);
        pointText.text = text;
    
    }

    private void HiddenText()
    {
        gameObject.SetActive(false);
    }
}
