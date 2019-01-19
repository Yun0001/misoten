using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial7Ctrl : MonoBehaviour {

    [SerializeField] private Sprite _t7sprite;

	void Start () {
    }
	
	void Update () {
        GameObject.Find("UI_menu").GetComponent<SpriteRenderer>().sprite = _t7sprite;

    }
}
