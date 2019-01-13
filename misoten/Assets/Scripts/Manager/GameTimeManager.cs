using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {

    private float countTime;
    private bool isTimeUp = false;
	private bool endSeFlag = false;
	private int oneSecond = 11;

	static public bool uiGameFinishFlag = false;
	static public bool eventAlienFlg = false;

	[SerializeField]
    private GameObject[] time;

    [SerializeField] bool _tutorialFlg = false;

    private Sprite[] sprites;


    [SerializeField]
    private GameObject eventManager;

    private EventManager eventManager_cs;

    private int[] eventOccurrenceTime = { 150, 100, 50 };
    private readonly int eventTime = 20;
    private int eventStartTime;


    // Use this for initialization
    void Start () {

        isTimeUp = false;
		uiGameFinishFlag = false;
		endSeFlag = false;
		eventAlienFlg = false;

		sprites = Resources.LoadAll<Sprite>("Textures/Time/UI_Digital_Custom");
        countTime = 181;
        int suu = (int)countTime;
        for (int i = 0; i < 3; i++)
        {
            time[i].GetComponent<SpriteRenderer>().sprite = sprites[suu % 10];
            suu = suu / 10;
        }

        oneSecond = 11;

        eventManager_cs = eventManager.GetComponent<EventManager>();
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

        // イベントが発生していないとき
        EventManager.EventState eventState = eventManager_cs.GetState();
        
        // イベントエイリアンが出現しているか判定するフラグ
        // 津野くん側で調整お願いします
        //bool eventAlienFlg = false;

        // イベント中でない&&イベントエイリアンがいないとき
        if (eventState == EventManager.EventState.Standby && !eventAlienFlg)
        {
            // 既定時間になるとイベントエイリアン出現
            if (countTime <= eventOccurrenceTime[eventManager_cs.GetEventOccurrenceNum()])
            {
				eventAlienFlg = true;
				eventStartTime = (int)countTime;
                // ここにイベントエイリアン出現のコード
                // エイリアン側で記述でもOK
            }
        }
        // イベント中
        else if (eventState == EventManager.EventState.Now)
        {
            // ２０秒経過したか判定
            if (countTime <= eventStartTime - eventTime)
            {
                // イベント終了
                eventManager_cs.EndEvent();
				eventAlienFlg = false;
			}
        }

        // 残り１０秒からSE再生
        if (countTime < oneSecond && countTime > 0)
        {
            oneSecond = (int)Mathf.Floor(countTime);
            Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Countdown), 0.5f, 10);
            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Countdown), 10);
        }
    }



    //時間切れゲーム終了
    private void TimeOver()
    {
        if (countTime<=0)
        {
			if (!endSeFlag) { endSeFlag = true; Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Gong_played2), 21); }
			countTime = 0;

			uiGameFinishFlag = true;

			GameObject[] minigame = GameObject.FindGameObjectsWithTag("MiniGame");
            foreach (var ui in minigame)
            {
                if (ui.activeInHierarchy)
                {
                    ui.SetActive(false);
                }
            }

            GameObject[] annouceUI = GameObject.FindGameObjectsWithTag("MiniGameUI");
            foreach (var ui in annouceUI)
            {
                    ui.SetActive(false);
            }
        }

		if(UI_GameFinish.gameEndFlag)
		{
			isTimeUp = true;
			Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Endannouncement), 0.5f, 10);
			Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Endannouncement), 10);
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
            if (countTime <= 11)
            {
                // １０秒から
                time[i].GetComponent<SpriteRenderer>().sprite = sprites[rement + 10];
            }
            else
            {
                // １０秒まで
                time[i].GetComponent<SpriteRenderer>().sprite = sprites[rement];
            }
            suu = suu / 10;
        }
    }

	public bool GetIsTimeUp() => isTimeUp;

	public bool IsTimeUp() => countTime <= 0 ? true : false;

    public void SetEventAlienFlg(bool b)
    {
        eventAlienFlg = b;
    }
}
