using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // ui panel
using UnityEngine.SceneManagement;

public class WhiteOut : MonoBehaviour {

    private GameObject _parent;

    private Color _color = new Color(255, 255, 255, 0);
    private bool isWhiteOut = false;
    private Image whiteOutImg;

    [SerializeField, Range(0.01f, 0.05f)]
    float fadeSpeed = 0.02f;

    
    void Start()
    {
        _parent = transform.root.gameObject;

        whiteOutImg = GetComponent<Image>();
    }

    void Update()
    {
        if (isWhiteOut)
        {
            StartWhiteOut();
        }
    }

    void StartWhiteOut()
    {
        whiteOutImg.enabled = true;
        _color.a += fadeSpeed;

        whiteOutImg.color = _color;

        if (_color.a >= 1.0f)
        {
            isWhiteOut = false;

            NextScene();

        }

    }

    void NextScene()
    {
        SceneManager.LoadScene("Scenes/3dNewScene");
    }

    public void OnWhiteOut()
    {
        isWhiteOut = true;
    }

}
