using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimEvent : MonoBehaviour {

    private GameObject _canvasWIO;
    
	void Start () {
        _canvasWIO = GameObject.Find("WIO");
    }

    void IsOpened()
    {
       _canvasWIO.GetComponent<WhiteIO>().OnWhiteOut();
    }

}
