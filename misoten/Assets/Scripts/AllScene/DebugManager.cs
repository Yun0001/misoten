using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Exit();
    }

    /// <summary>
    /// exe終了処理
    /// </summary>
    private void Exit()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Exit");
            Application.Quit();
        }
    }

    
    //private void VisualArea()
    //{
    //}

    //private void VisualLog()
    //{
    //}


}
