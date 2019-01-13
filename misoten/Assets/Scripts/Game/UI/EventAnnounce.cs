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
        "赤エイリアンの得点UP",
        "青エイリアンの得点UP",
        "黄エイリアンの得点UP",
        "ミキサー料理の得点UP",
        "ミキサー以外の料理の得点UP",
        "全ての料理の得点UP" };

    //　イベントマネージャ
    [SerializeField]
    GameObject eventManager;

    // 状態
    EventAnnounceState state;

    // ウインドウの移動スピード
    [SerializeField]
    float speed;

    // 初期位置
    [SerializeField]
    float startPos;

    // 停止位置
    [SerializeField]
    float[] endPos;

    [SerializeField]
    private Text announceText;

	// Use this for initialization
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



        Vector3 pos = transform.position;
        pos.x -= speed;
        transform.position = pos;
        if (transform.position.x < endPos[(int)state])
        {
            state = EventAnnounceState.Normal;
        }
	}

    public void SetState(EventAnnounceState _state) => state = _state;

    public void SetAnnoounceText(int e) => announceText.text = annouceTextArray[e];
}
