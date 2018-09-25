using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{

    //public GameObject player;
    private Text scoreTextP1;
    private Text scoreTextP2;
    private Text scoreTextP3;
    private Text scoreTextP4;

    private int score = 0;

    void Start()
    {
        
        //playerTrans = player.GetComponent<Transform>();
        //GameObject child = transform.Find("ScoreP1").gameObject;
        scoreTextP1 = GameObject.Find("ScoreP1").GetComponent<Text>();
        scoreTextP2 = GameObject.Find("ScoreP2").GetComponent<Text>();
        //scoreTextP3 = GameObject.Find("ScoreP3").GetComponent<Text>();
        //scoreTextP4 = GameObject.Find("ScoreP3").GetComponent<Text>();
        ////scoreTextP1.text = child.GetComponent("Text" )();
        scoreTextP1.text = "Score: 0";
        scoreTextP2.text = "Score: 0";
        //scoreTextP3.text = "Score: 0";
        //scoreTextP4.text = "Score: 0";
    }

    void Update()
    {
        //OキーでScoreCount(仮)：TODO
        if (Input.GetKeyDown(KeyCode.O))
        {
            score += 10; //scoreに加算
                         //スコアを更新して表示
            scoreTextP1.text = "Score: " + score.ToString();
        }
        //PキーでScoreCount(仮)：TODO
        if (Input.GetKeyDown(KeyCode.P))
        {
            score += 10; //scoreに加算
                         //スコアを更新して表示
            scoreTextP2.text = "Score: " + score.ToString();
        }
    }
}

