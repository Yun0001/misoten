using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

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
//#if UNITY_EDITOR
          //  EditorApplication.isPlaying = false;
//#endif
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Title);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Tutorial);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Game);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManagerScript.LoadScene(SceneManagerScript.SceneName.Result);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManagerScript.ReloadScene();
        }

        //SPACEキー仮置き
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManagerScript.LoadNextScene();
        }
    }

}
