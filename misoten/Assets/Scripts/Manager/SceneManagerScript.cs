using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : Singleton<SceneManagerScript>
{

    private static string currentScene;
    private static string ScenesFolderPass = "Scenes/";

    public enum SceneName
    {
        Title,
        Tutorial,
        Game,
        Result,
        MaxCount
    }

    // enumからシーン名を取得するために必要
    private static Dictionary<SceneName, string> m_sceneNameDictionary = new Dictionary<SceneName, string> {
        { SceneName.Title, "Title_heita"},        //"Aseets/Scenes/Title.unity"
        { SceneName.Tutorial,"Tutorial" },  //"Aseets/Scenes/Tutorial.unity"
        { SceneName.Game,"Game" },          //"Aseets/Scenes/Game.unity"
        { SceneName.Result,"Result" }       //"Aseets/Scenes/Result.unity"
    };

    protected static new void CreateSingletonObject()
    {

    }

    /// <summary>
    /// 次のシーンに遷移
    /// </summary>
    public static void LoadNextScene() => Instance._LoadNextScene();

    private void _LoadNextScene()
    {
        SoundController.SoundStop();

        // 現在読み込んでいるシーンの名前を取得
        currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Title_heita":
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
                break;
            case "Tutorial":
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                break;
            case "Game":
                SceneManager.LoadScene("Result", LoadSceneMode.Single);
                break;
            case "Result":
                SceneManager.LoadScene("Title_heita", LoadSceneMode.Single);
                break;
            default:
                Debug.LogError("不正なシーン");
                break;
        }
    }


    /// <summary>
    /// 指定したシーンに遷移
    /// </summary>
    /// <param name="sceneID"></param>
    public static void LoadScene(SceneName sceneID) => Instance._LoadScene(sceneID);

    private void _LoadScene(SceneName sceneID)
    {
        SoundController.SoundStop();
        SceneManager.LoadScene(m_sceneNameDictionary[sceneID], LoadSceneMode.Single);
    }

    /// <summary>
    /// 現在のシーンをリロード
    /// </summary>
    public static void ReloadScene() => Instance._ReloadScene(); 

    private void _ReloadScene()
    {
        SoundController.SoundStop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

