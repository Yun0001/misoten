using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {

    private float countTime;
    private float timespeed;
    private Text TimeText;
    private GameObject SpeedUpText;
    private bool isTimeUp=false;

    // Use this for initialization
    void Start () {
        countTime = 60;
        timespeed = 1;

        TimeText = GetComponent<Text>();
     //   SpeedUpText = GameObject.Find("SpeedUpText");
    }
	
	// Update is called once per frame
	void Update () {
        if (!isTimeUp)
        {
            GameTimer();
            TimeOver();
            UpdateText();
        }


    }

    //時間管理
    private void GameTimer()
    {
        countTime -= Time.deltaTime; //スタートしてからの秒数を格納
    }

    //時間加速
    private void TimerSpeed()
    {
        CountSpeed();
        countTime -= Time.deltaTime * timespeed;
    }

    private void CountSpeed()
    {
        //ToDo:仮置き
        if (Input.GetKeyDown(KeyCode.P))
        {
            timespeed += 0.5f;
            //SpeedUpText.GetComponent<Text>().text = "スピードアップ:"+ timespeed.ToString();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            timespeed -= 0.5f;
            //SpeedUpText.GetComponent<Text>().text = "スピードダウン:"+ timespeed.ToString();
        }

        //通常スピード
        if (timespeed == 1.0f)
        {
           // SpeedUpText.GetComponent<Text>().text = " 通常スピード :" +timespeed.ToString();
        }


        //スピード限界
        if (timespeed <= 0.5f)
        {
            timespeed = 0.5f;
           // SpeedUpText.GetComponent<Text>().text = "スピードダウン:" + timespeed.ToString();
        }
        if (timespeed >= 10.0f)
        {
            timespeed = 10.0f;
           // SpeedUpText.GetComponent<Text>().text = "スピードアップ:" + timespeed.ToString();
        }
    }

    //時間切れゲーム終了
    private void TimeOver()
    {
        if (countTime<=0)
        {
            countTime = 0;
            isTimeUp = true;
           // SpeedUpText.GetComponent<Text>().text = "Time Up" ;
        }
    }

    public float GetCountTime()
    {
        return countTime;
    }


    private void UpdateText()
    {
        TimeText.text = "Time : " + countTime.ToString("F2"); //小数2桁にして表示

    }


}
