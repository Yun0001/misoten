using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventAnnounce : MonoBehaviour
{

    // イベントアナウンスの状態
    public enum EventAnnounceState
    {
        Start,
        End,
        Normal,
    }

    private string[] annouceTextArray = {
        "イベントなし",
        "VIPの火星人来店中",
        "VIPの水星人来店中",
        "VIPの土星人来店中",
        "ボーナスタイム！「ミキサー」",
        "ボーナスタイム！「グリル・ボイル・レンジ」",
        "ボーナスタイム！「ALL」" };

    //　イベントマネージャ
    [SerializeField]
    GameObject eventManager;

    // 状態
    [SerializeField]
    EventAnnounceState state;

    // ウインドウの移動スピード
    [SerializeField]
    float speed;

    // 初期位置
    [SerializeField]
    float startPos;

    // 停止位置
    [SerializeField]
    float endPos;

    [SerializeField]
    float resetPos;

	// Use ths for initialization
	void Awake () {
        state = EventAnnounceState.Normal;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // -------------------テスト-----------------
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetState(EventAnnounceState.Start);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetState(EventAnnounceState.End);
        }

        //-----------------------------------------
        if (state == EventAnnounceState.Normal) return;
        if (state == EventAnnounceState.End)
        {
            Vector3 Pos = transform.position;
            Pos.x = startPos;
            transform.position = Pos;
            state = EventAnnounceState.Normal;
            return;
        }

        Vector3 pos = transform.position;
        pos.x -= speed;
        transform.position = pos;


        if (transform.position.x < endPos)
        {
           // state = EventAnnounceState.Normal;
            ResetPoaition();
        }
     
	}

    public void SetState(EventAnnounceState _state) => state = _state;


    public void ResetPoaition()
    {
        Vector3 pos = transform.position;
        pos.x = resetPos;
        transform.position = pos;
    }
}
