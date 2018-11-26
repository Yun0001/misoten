using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {

    private float countTime;
    private bool isTimeUp = false;
    private int oneSecond = 11;


    [SerializeField]
    private GameObject[] time;

    private Sprite[] spritsIn;
    private Sprite[] spritsOut;    // Use this for initialization
    void Start () {

        isTimeUp = false;

        spritsIn = Resources.LoadAll<Sprite>("Textures/Time/Time_UI_Inner");
        spritsOut = Resources.LoadAll<Sprite>("Textures/Time/Time_UI_Outer");
        countTime = 180;
        int suu = (int)countTime;
        for (int i = 0; i < 3; i++)
        {
            time[i].GetComponent<SpriteRenderer>().sprite = spritsIn[suu % 10];
            time[i].GetComponent<SpriteRenderer>().color = new Color(0.9725f, 0.7098f, 0.9019f, 1.0f);
            time[i + 3].GetComponent<SpriteRenderer>().sprite = spritsOut[suu % 10];
            suu = suu / 10;
        }

        oneSecond = 11;
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
        if (countTime < oneSecond && countTime > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                time[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            oneSecond = (int)Mathf.Floor(countTime);
            Sound.SetVolumeSe(GameSceneManager.seKey[24], 0.5f, 10);
            Sound.PlaySe(GameSceneManager.seKey[24], 10);
        }
    }



    //時間切れゲーム終了
    private void TimeOver()
    {
        if (countTime<=0)
        {
            countTime = 0;
            Sound.SetVolumeSe(GameSceneManager.seKey[25], 0.5f, 10);
            Sound.PlaySe(GameSceneManager.seKey[25], 10);
            Sound.StopBgm();
            isTimeUp = true;

            // 表示中のミニゲームUIを非表示にする
            GameObject[] work = GameObject.FindGameObjectsWithTag("MiniGame");
            foreach (var minigame in work)
            {
                minigame.SetActive(false);
            }

            // 再生中のSEを止める
            SoundController.StopAllSE();
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
            time[i].GetComponent<SpriteRenderer>().sprite = spritsIn[rement];
            time[i + 3].GetComponent<SpriteRenderer>().sprite = spritsOut[rement];
            if (rement == 0)
            {
                break;
            }
            suu = suu / 10;
        }
    }

    public bool IsTimeUp() => countTime <= 0 ? true : false;
}
