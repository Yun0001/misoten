using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {

    public enum EventState
    {
        Standby,
        Now,
        End
    }

	public enum FeverPattern
    {
        None,
        RedAlien,
        BuleAlien,
        YellowAlien,
        Mixer,
        Cooking,
        All,
    }

    enum ScoreState
    {
        Low,
        Mid,
        High
    }

    EventState eventstate;
    FeverPattern nowPattern;
    FeverPattern[,] pattern=
    {
		{ FeverPattern.RedAlien,FeverPattern.BuleAlien,FeverPattern.YellowAlien,FeverPattern.Cooking},
        { FeverPattern.RedAlien,FeverPattern.BuleAlien,FeverPattern.YellowAlien,FeverPattern.Mixer}
    };
    int eventOccurrenceNum;
    int[,] scoreBorder= 
    {
        { 1000,2000},
        { 3000,6000},
        { 5000,1000}
    };

    [SerializeField]
    private GameObject eventAnnounce;

    private void Awake()
    {
        eventstate = EventState.Standby;
        nowPattern = FeverPattern.None;
        eventOccurrenceNum = 0;
    }


    /// <summary>
    /// イベントスタート
    /// この関数をイベントエイリアンに料理を渡した後に呼ぶ
    /// </summary>
    public void StartEvent()
    {
        // イベントスタート
        eventstate = EventState.Now;
        
        // イベントパターン決定
        DecisionPattern();

        // イベントの発生回数を加算
        eventOccurrenceNum++;

        // イベントアナウンスの状態をstartに変更
        EventAnnounceStart();

        // アナウンスウインドウのテキストをセット
        SetEventAnnouceText();
    }

    private ScoreState DecisionScore()
    {
        // スコア参照
        int score = 0;

        // 現在のスコアに応じて状態を返す
        for (int i = 1; i >= 0; i--)
        {
            if (score >= scoreBorder[eventOccurrenceNum, i])
            {
                return (ScoreState)Enum.ToObject(typeof(ScoreState), i + 1);
            }
        }
        return ScoreState.Low;
    }

    private void DecisionPattern()
    {
        // 乱数宣言
        System.Random r = new System.Random();
        int rand;

        // 現在のスコアを参照しFerverPatternを設定
        switch (DecisionScore())
        {
            case ScoreState.Low:
                rand = r.Next(4);
                nowPattern = pattern[(int)ScoreState.Low, rand];
                break;

            case ScoreState.Mid:
                rand = r.Next(4);
                nowPattern = pattern[(int)ScoreState.Mid, rand];
                break;

            case ScoreState.High:
                nowPattern = FeverPattern.Mixer;
                break;
        }
    }

    public void EndEvent()
    {
        eventstate = EventState.Standby;
        EventAnnounceEnd();
    }

    public EventState GetState() => eventstate;

    public int GetEventOccurrenceNum() => eventOccurrenceNum;

	public FeverPattern GetNowPattern() => nowPattern;

    private void EventAnnounceStart() => eventAnnounce.GetComponent<EventAnnounce>().SetState(EventAnnounce.EventAnnounceState.Start);

    private void EventAnnounceEnd()=> eventAnnounce.GetComponent<EventAnnounce>().SetState(EventAnnounce.EventAnnounceState.End);

    private void SetEventAnnouceText() => eventAnnounce.GetComponent<EventAnnounce>().SetAnnoounceText((int)nowPattern);
    
}