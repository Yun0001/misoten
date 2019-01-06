using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnnounce : MonoBehaviour
{

    // イベントアナウンスの状態
    public enum EventAnnounceState
    {
        Start,
        End,
        Normal,
    }

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

	// Use this for initialization
	void Awake () {
        state = EventAnnounceState.Normal;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == EventAnnounceState.Normal) return;

        Vector3 pos = transform.position;
        pos.x -= speed;
        transform.position = pos;
        if (transform.position.x < endPos[(int)state])
        {
            state = EventAnnounceState.Normal;
        }
	}

    public void SetState(EventAnnounceState _state)
    {
        state = _state;
    }
}
