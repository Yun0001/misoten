using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilledGage : MonoBehaviour {

    enum EArea
    {
        TimingPoint,
        Normal,
        Hard,
        Hell
    }

    [SerializeField]
    private GameObject[] successArea;

    [SerializeField]
    private float areaSpeed;

    private float frameCount;

	// Use this for initialization
	void Start () {
        //　エリアを非表示
        for (int i = 1; i < successArea.Length; i++)
        {
            //successArea[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //はじめは何フレームか待つ
        Vector3 pos = successArea[(int)EArea.Normal].transform.position;
        pos.x--;
        successArea[(int)EArea.Normal].transform.position = pos;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
