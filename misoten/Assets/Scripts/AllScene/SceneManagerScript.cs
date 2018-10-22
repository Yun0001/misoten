using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    private bool IsLoadScene = false;
    private bool IsKeyEnter = false;
    //private bool IsKey1 = false;
    //private bool IsKey2 = false;
    //private bool IsKey3 = false;
    //private bool IsKey4 = false;

    private enum SceneName
    {
        Title,
        Tutorial,
        Game,
        Result,
        MaxCount
    }

    // enumからシーン名を取得するために必要
    private Dictionary<SceneName, string> m_sceneNameDictionary = new Dictionary<SceneName, string> {
        { SceneName.Title, "Title"},        //"Aseets/Scenes/Title.unity"
        { SceneName.Tutorial,"Tutorial" },  //"Aseets/Scenes/Tutorial.unity"
        { SceneName.Game,"Game" },          //"Aseets/Scenes/Game.unity"
        { SceneName.Result,"Result" }       //"Aseets/Scenes/Result.unity"
    };

    // これを呼んでシーン遷移を行う
    void LoadScene(SceneName sceneID)
    {
        //Debug.Log("sceneID"+ sceneID);
        SceneManager.LoadScene(m_sceneNameDictionary[sceneID]);
    }

    // Use this for initialization
    private void Start () {
        KeyFalse();
    }

    // Update is called once per frame
    private void Update () {
        KeyPress();

        if (IsLoadScene)
        {
            LoadNextScene();
            IsLoadScene = false;
            KeyFalse();
        }
    }

    //使ってない
    private void ReloadScene()    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //if (IsKeyEnter)
        //{
        //    currentSceneIndex++;
        //}
        //if ((SceneName)SceneManager.GetActiveScene().buildIndex==SceneName.MaxCount)
        //{
        //    currentSceneIndex = 0;
        //}
        //currentSceneIndex = 2;
        //Debug.Log("currentSceneIndex"+currentSceneIndex);

        SceneManager.LoadSceneAsync(currentSceneIndex);
    }

    private void LoadNextScene()
    {

        switch ((SceneName)SceneManager.GetActiveScene().buildIndex)
        {
            case SceneName.Title:
                LoadScene(SceneName.Tutorial);
                break;

            case SceneName.Tutorial:
                LoadScene(SceneName.Game);
                break;

            case SceneName.Game:
                LoadScene(SceneName.Result);
                break;

            case SceneName.Result:
                LoadScene(SceneName.Title);
                break;
        }

    }

    private void LoadSceneFlg()
    {
        IsLoadScene = true;
    }

    /// <summary>
    /// キー入力仮置き
    /// </summary>
    void KeyPress()
    {
        //if (Input.GetButtonDown("1"))
        //{
        //    IsKey1 = true;
        //    //SceneManager.GetActiveScene().buildIndex;
        //    LoadScene();
        //}
        //if (Input.GetButtonDown("2"))
        //{
        //    IsKey1 = true;
        //    LoadScene();
        //}
        //if (Input.GetButtonDown("3"))
        //{
        //    IsKey1 = true;
        //    LoadScene();
        //}
        //if (Input.GetButtonDown("4"))
        //{
        //    IsKey1 = true;
        //    LoadScene();
        //}

        //SPACEキー仮置き
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Space");
            IsKeyEnter = true;
            LoadSceneFlg();
        }
    }

    void KeyFalse()
    {
        //IsKey1 = false;
        //IsKey2 = false;
        //IsKey3 = false;
        //IsKey4 = false;
        IsKeyEnter = false;
    }

}

