using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour {


    private int[] ResultScore;
    // Use this for initialization

	
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
            //ResultScore[pID] = ScoreManager.GetInstance().GetPlayerScore(pID);
            //ResultScore[pID] = scoreManager.GetPlayerScore(pID);
            //Debug.Log(pID + " : " + ResultScore[pID]);
           //Debug.Log(pID + " : " + scoreManager.GetPlayerScore(pID));
            
            //scoreManager.GetPlayerScore(pID);
            //Debug.Log(pID + " : " + ScoreManager.GetInstance().GetPlayerScore(pID));
            //Debug.Log(pID + " : " + pScore[pID]);
       }

    }

    private void Seane()
    {
        //仮置き
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManagerScript.LoadNextScene();
        }
    }

}
