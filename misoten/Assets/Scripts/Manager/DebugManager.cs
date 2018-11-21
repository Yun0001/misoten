using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugManager : MonoBehaviour {

	void Start () {
        Cursor.visible = false;
    }
	
	void Update () {

        ForcedTermination();

    }

    private void ForcedTermination()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

}
