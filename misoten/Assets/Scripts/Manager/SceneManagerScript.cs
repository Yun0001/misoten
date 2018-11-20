using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

public class SceneManagerScript : Singleton<SceneManagerScript> {

    private bool IsLoadScene = false;
    private string currentScene;
    private GameObject Time;
    private GameTimeManager GameTime;
    private GameObject gamepad;
    private GamePad.Index PlayerControllerNumber;
    private Player player_cs;


    private string ScenesFolderPass = "Scenes/";

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
        SceneManager.LoadScene(ScenesFolderPass + m_sceneNameDictionary[sceneID]);
    }

    private new void Awake()
    {
        // 現在読み込んでいるシーンの名前を取得
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Game")
        {
            Time = GameObject.Find("TimeManager");
            GameTime = Time.GetComponent<GameTimeManager>();

        } 
        ////ゲームパッド初期処理
        //if (currentScene == "Result")
        //{
        //    //Gamepad
        //    gamepad = GameObject.Find("Gamepad");
        //    player_cs = gamepad.GetComponent<Player>();
        //    PlayerControllerNumber = player_cs.GetPlayerControllerNumber();
        //}
      }

    // Update is called once per frame
    private void Update () {
        // 現在読み込んでいるシーンの名前を取得
        currentScene = SceneManager.GetActiveScene().name;

        KeyPress();

        if (IsLoadScene)
        {
            LoadNextScene();
            IsLoadScene = false;
        }
        //if (currentScene=="Game")
        //{            
        //    timeUPloadResult();
        //}
    }

    //使ってない
    private void ReloadScene()    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(currentSceneIndex);
    }

    public void LoadNextScene()
    {
        /*　ToDo： バグ中
        switch ((SceneName)SceneManager.GetActiveScene().buildIndex)
        {
            case SceneName.Title:
                //LoadScene(SceneName.Tutorial); チュートリアルができたらコッチ
                LoadScene(SceneName.Game); // 今だけ
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

            default:
                Debug.LogError("不正なシーン");
                break;
        }
        */

        switch(currentScene){
            case "Title_heita":
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

    private void LoadSceneFlg()
    {
        IsLoadScene = true;
    }

    /// <summary>
    /// キー入力仮置き
    /// </summary>
    void KeyPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentScene == "Title_heita")
            {
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (currentScene =="Game" )
            {
                SceneManager.LoadScene("Result", LoadSceneMode.Single);
            }
        }

        //if (GamePad.GetButtonDown(GamePad.Button.A, PlayerControllerNumber)
        //    ||Input.GetKeyDown(KeyCode.T))
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentScene == "Result")
            {
                SceneManager.LoadScene("Title_heita", LoadSceneMode.Single);
            }
        }
        
  
      

        //SPACEキー仮置き
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Space");
            LoadSceneFlg();
        }
    }

    private void timeUPloadResult()
    {
        if(GameTime.GetCountTime()<=0)
        {
            SceneManager.LoadScene("Result", LoadSceneMode.Single);
        }
    }

}

