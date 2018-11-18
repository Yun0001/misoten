using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoxMiniGame : MonoBehaviour {

    [SerializeField]
    private int takeCount;

    [SerializeField]
    private int addBarragePoint;

    private float playerBarrage = 0;

    private float freesResist = 0f;

    [SerializeField]
    private GameObject IceBox;

    private bool isStart = false;

    [SerializeField]
    private GameObject iceImage;

    [SerializeField]
    private GameObject icePickImage;

    bool moveflg =false;
    int moveframe=0;

	// Use this for initialization
	void Start () {
		
	}

    public void Init()
    {
        isStart = false;
        playerBarrage = 0;
        freesResist = 0f;
        HiddenSprite();
        moveflg = false;
        moveframe = 0;
    }

    public void flgOn()
    {
        iceImage.SetActive(true);
        icePickImage.SetActive(true);
        isStart = true;
        PlayIcePickSE();
        Sound.PlaySe(GameSceneManager.seKey[13]);
    }
	// Update is called once per frame
	void Update () {
        if(isStart)
        {
            freesResist += 0.1f;
            Vector3 scale = iceImage.transform.localScale;
            float fval = ((playerBarrage - freesResist) / takeCount);
            float Scale = 1f - fval;
            if (Scale > 1) Scale = 1f;
            iceImage.transform.localScale = new Vector3(Scale, Scale, Scale);
            MoveIcePick();
        }
	}

    public bool AddPlayerBarrage()
    {
        playerBarrage += addBarragePoint;
        Vector3 scale = iceImage.transform.localScale;

        return IsOverTakeCount(); 
    }

    public void HiddenSprite()
    {
        iceImage.SetActive(false);
        icePickImage.SetActive(false);
        StopIcePickSE();
        Sound.PlaySe(GameSceneManager.seKey[14]);
        Sound.PlaySe(GameSceneManager.seKey[12]);
    }

    private bool IsOverTakeCount() => (playerBarrage - freesResist) >= takeCount;

    private void MoveIcePick()
    {
        Vector3 pos = icePickImage.transform.position;
        if (moveflg)
        {
            pos.x += 0.008f;
            pos.y += 0.008f;
        }
        else
        {
            pos.x -= 0.008f;
            pos.y -= 0.008f;
        }
        icePickImage.transform.position = pos;
        moveframe++;
        if (moveframe >= 10)
        {
            moveflg = !moveflg;
            moveframe = 0;
        }
    }

    private void PlayIcePickSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[11], true, 5);
        Sound.PlaySe(GameSceneManager.seKey[11], 5);
    }

    private void StopIcePickSE()
    {
        Sound.SetLoopFlgSe(GameSceneManager.seKey[11], false, 5);
        Sound.StopSe(GameSceneManager.seKey[11], 5);
    }
}
