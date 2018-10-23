using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGrilledGage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (collision.tag == "SuccessArea")
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
