using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePickPosInit : MonoBehaviour {

    private Vector3 _position;

    // Use this for initialization
    void Start()
    {
        _position = this.transform.parent.gameObject.transform.position;
        this.transform.position = _position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
