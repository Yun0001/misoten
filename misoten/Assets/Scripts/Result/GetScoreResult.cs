using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GetScoreResult : MonoBehaviour
{

    public int[] pScore;

    private string currentScene;
    private GameObject scoreManager;
    private GameObject Score;
    private GameObject ScoreCanvas;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Game")
        {
            //Debug.Log("Score:Game");
            Score = GameObject.Find("Score");
            scoreManager = GameObject.Find("ScoreManager");
        }
        DontDestroyOnLoad(Score);
        DontDestroyOnLoad(scoreManager);
        ScoreCanvas = GameObject.Find("Score/Canvas");
    }

	// Use this for initialization
	void Start () {
 

	}
	
	// Update is called once per frame
	void Update () {
        // 現在読み込んでいるシーンの名前を取得
        currentScene = SceneManager.GetActiveScene().name;
        ScoreSearch();
        SceneManager.sceneUnloaded += OnSceneUnloaded;  
	}

    private void ScoreSearch()
    {
        if (currentScene == "Game")
        {
            
            for (int pID = 0; pID < 4; pID++)
            {
                //ResultScore[pID] = ScoreManager.GetInstance().GetPlayerScore(pID);
                //ResultScore[pID] = scoreManager.GetPlayerScore(pID);
                //Debug.Log(pID + " : " + ResultScore[pID]);
                //Debug.Log(pID + " : " + ScoreManager.GetInstance().GetPlayerScore(pID));
                //pScore[pID] = ScoreManager.GetInstance().GetPlayerScore(pID);
            }
        }
        if (currentScene == "Result")
        {
            //Debug.Log("Score:" + ScoreCanvas.gameObject.activeSelf);
            //Debug.Log("Score:" + ScoreCanvas.gameObject.name);
            //オブジェ非活性化
            if (ScoreCanvas.gameObject.activeSelf == true)
            {
                ScoreCanvas.gameObject.SetActive(false);
            }
        }
    }

    //シーン破棄処理
    void OnSceneUnloaded(Scene Result)
    {        
        if (currentScene == "Result")
        {
            //Destroy(Score, .2f);
            Destroy(Score);
            Destroy(scoreManager);
        }
    }

}
