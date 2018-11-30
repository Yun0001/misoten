using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePickPosInit : MonoBehaviour {

    private Vector3 _position;
    private IceBoxMiniGame _iceBoxMiniGame;

    // Use this for initialization
    void Start()
    {
        _position = this.transform.localPosition;
        _iceBoxMiniGame = GetComponent<IceBoxMiniGame>();
    }
	
	// Update is called once per frame
	void Update () {

        //_iceBoxMiniGame.SetInitPos(_position);

    }

}
