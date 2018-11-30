using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissAnnounce : MonoBehaviour
{
    TextMesh missText;
    private int activeFrame;
    [SerializeField]
    private Vector3 InitPos;

    [SerializeField] private bool _isTutorialMode = false;

    private void Awake()
    {
        missText = GetComponent<TextMesh>();
        missText.text = "MISS";
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

    public void DisplayText()
    {
        if (_isTutorialMode)
        {
            gameObject.SetActive(true); 
        }
        else
        {
            transform.position = InitPos;
            gameObject.SetActive(true);
        }
    }

    private void HiddenText()
    {
        gameObject.SetActive(false);
    }

}
