using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimEvent : MonoBehaviour {

    private GameObject _canvasWO;
    
	void Start () {
        _canvasWO = GameObject.Find("WO");
    }

    void IsOpened()
    {
        Debug.Log("Opened");
        _canvasWO.GetComponent<WhiteOut>().OnWhiteOut();
    }

}
