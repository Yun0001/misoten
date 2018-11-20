using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {

    private float countTime;
    private float timespeed;
    private Text TimeText;
    private GameObject SpeedUpText;
    private bool isTimeUp = false;


    [SerializeField]
    private GameObject[] time;

    private Sprite[] sprits;
    // Use this for initialization
    void Start () {

        isTimeUp = false;

        sprits = Resources.LoadAll<Sprite>("Textures/UI_Digital2");
        countTime = 180;
        int suu = (int)countTime;
        for (int i = 0; i < 3; i++)
        {
            time[i].GetComponent<SpriteRenderer>().sprite = sprits[suu % 10];
            suu = suu / 10;
        }
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
            Sound.StopBgm();
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
        int suu = (int)countTime;
        int rement;
        for (int i = 0; i < 3; i++)
        {
            rement = suu % 10;
            if (i == 2 && countTime < 100)
            {
                time[i].GetComponent<SpriteRenderer>().sprite = null;
                break;
            }
            time[i].GetComponent<SpriteRenderer>().sprite = sprits[rement];
            if (rement == 0)
            {
                break;
            }
            suu = suu / 10;
        }
    }

    public bool IsTimeUp()
    {
        if (countTime <= 0)
        {
            return true;
        }
        return false;
    }

}
