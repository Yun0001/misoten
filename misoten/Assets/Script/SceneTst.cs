using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTst : MonoBehaviour {

    private bool IsLoadScene = false;

    private enum SceneName
    {
        Title,
        Tutorial,
        Game,
        Result,
        MaxCount
    }

    // Use this for initialization
    private void Start () {
	
	}

    // Update is called once per frame
    private void Update () {
		
	}

    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex);
    }

    private void LoadNextScene()
    {

        switch ((SceneName)SceneManager.GetActiveScene().buildIndex)
        {
            case SceneName.Title:

                break;

            case SceneName.Tutorial:

                break;

            case SceneName.Game:

                break;

            case SceneName.Result:

                break;
        }

    }

    private void LoadScene()
    {
        IsLoadScene = true;
    }

}
