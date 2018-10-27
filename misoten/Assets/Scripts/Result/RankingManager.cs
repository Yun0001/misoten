using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour {

    private GameObject SceneObject;
    private SceneManagerScript sceneManagerScript;
    private ScoreManager scoreManagerScript;

    private int[] ResultScore;

    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        SceneObject = GameObject.Find("SceneManager");
        sceneManagerScript = SceneObject.GetComponent<SceneManagerScript>();
        //scoreManagerScript = GetComponent<ScoreManager>();

    }
	
	// Update is called once per frame
	void Update () {
        Seane();
        GetScore();
    }

    private void GetScore()
    {
        //ID = 0;
        //scoreManagerScript.GetPlayerScore(ID);
        //ScoreManager.Instance.GetPlayerScore(ID);

        //scoreManagerScript::getInstance().playerScore[1];

        for (int pID=0; pID<4;pID++)
        {
            //ResultScore[pID]= ScoreManager.GetInstance().GetPlayerScore(pID);
            //Debug.Log( pID+" : "+ ResultScore[pID]);
        }

    }

    private void Seane()
    {
        //仮置き
        if (Input.GetKeyDown(KeyCode.Q))
        {
            sceneManagerScript.LoadNextScene();
            //SceneManager.ADD(); 
        }
    }

}
