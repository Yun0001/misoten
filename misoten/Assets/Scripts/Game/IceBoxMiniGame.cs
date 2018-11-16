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
    }
	// Update is called once per frame
	void Update () {
        if(isStart)
        {
            freesResist += 0.1f;
            Vector3 scale = iceImage.transform.localScale;
            scale.x += 0.001f;
            scale.y += 0.001f;
            iceImage.transform.localScale = scale;
            MoveIcePick();
        }
	}

    public bool AddPlayerBarrage()
    {
        playerBarrage += addBarragePoint;
        Vector3 scale = iceImage.transform.localScale;
        scale.x -= 0.01f;
        scale.y -= 0.01f;
        iceImage.transform.localScale = scale;

        return IsOverTakeCount(); 
    }

    public void HiddenSprite()
    {
        iceImage.SetActive(false);
        icePickImage.SetActive(false);
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
}
