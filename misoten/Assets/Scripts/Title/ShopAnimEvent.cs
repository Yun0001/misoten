using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAnimEvent : MonoBehaviour {

    private GameObject _canvasWO;
    
	void Start () {
        _canvasWO = transform.GetChild(0).GetChild(0).gameObject;
    }

    void IsOpened()
    {
        _canvasWO.GetComponent<WhiteOut>().OnWhiteOut();
    }

}
