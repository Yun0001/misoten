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

    [SerializeField] bool _tutorialFlg = false;

    private Sprite[] spritsInner;
    private Sprite[] spritsOuter;
    // Use this for initialization
    void Start () {

        isTimeUp = false;

        spritsInner = Resources.LoadAll<Sprite>("Textures/Time/Time_UI_Inner");
        spritsOuter = Resources.LoadAll<Sprite>("Textures/Time/Time_UI_Outer");
        countTime = 180;
        int suu = (int)countTime;
        for (int i = 0; i < 3; i++)
        {
            time[i].GetComponent<SpriteRenderer>().sprite = spritsInner[suu % 10];
            time[i].GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.4f, 0.7f, 1.0f);
            time[i+3].GetComponent<SpriteRenderer>().sprite = spritsOuter[suu % 10];
            suu = suu / 10;
        }

        oneSecond = 11;
    }
	
	// Update is called once per frame
	void Update () {

        if (_tutorialFlg) return;

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
            oneSecond = (int)Mathf.Floor(countTime);
            Sound.SetVolumeSe(GameSceneManager.seKey[24], 0.5f, 10);
            Sound.PlaySe(GameSceneManager.seKey[24], 10);
            // 赤い色に変更
            if (countTime < 11)
            {
                for (int i = 0; i < 3; i++)
                {
                    time[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                }
            }
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
            SoundController.StopAllSE();
            isTimeUp = true;

            GameObject[] minigame = GameObject.FindGameObjectsWithTag("MiniGame");
            foreach (var ui in minigame)
            {
                if (ui.activeInHierarchy)
                {
                    ui.SetActive(false);
                }
            }
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
            time[i].GetComponent<SpriteRenderer>().sprite = spritsInner[rement];


            time[i+3].GetComponent<SpriteRenderer>().sprite = spritsOuter[rement];
            if (rement == 0)
            {
                break;
            }
            suu = suu / 10;
        }
    }

    public bool IsTimeUp() => countTime <= 0 ? true : false;
}
