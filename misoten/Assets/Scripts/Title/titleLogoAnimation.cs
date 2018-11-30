using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class titleLogoAnimation : MonoBehaviour {

    private RectTransform _titleLogo;
    Vector3 _target = new Vector3(0, -0.5f, 0);
    [SerializeField, Range(1.0f, 10.0f)] float slideSpeed = 5.0f;

    private GameObject _canvasWIO;

    void Start()
    {
        _titleLogo = GameObject.Find("titleLogo").GetComponent<RectTransform>();

        _canvasWIO = GameObject.Find("WIO");
    }

    void Update()
    {

        if (_titleLogo.localPosition.y <= 10.0f)
        {
            _titleLogo.localPosition = new Vector3(0, 10.0f, 0);
            _canvasWIO.GetComponent<WhiteIO>().OnWhiteIn();
        }
        else
        {
            float t = Time.deltaTime / slideSpeed;
            _titleLogo.localPosition = Vector3.Slerp(this.transform.localPosition, _target, t);
        }

    }

}
