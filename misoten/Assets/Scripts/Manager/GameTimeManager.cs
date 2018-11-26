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

    public bool IsTimeUp() => countTime <= 0 ? true : false;
}
