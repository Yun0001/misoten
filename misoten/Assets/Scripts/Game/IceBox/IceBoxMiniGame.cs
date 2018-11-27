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

    [SerializeField]
    private Vector3 icePickInitPosition;

    [SerializeField]
    private ParticleSystem iceFragment;

    [SerializeField]
    private GameObject UIButton;

  

    bool moveflg =false;
    int moveframe=0;


    public void Init()
    {
        isStart = false;
        playerBarrage = 0;
        freesResist = 0f;
        HiddenSprite();
        moveflg = false;
        moveframe = 0;
        UIButton.SetActive(false);
    }

    public void Display()
    {
        iceImage.SetActive(true);
        icePickImage.SetActive(true);
        icePickImage.gameObject.transform.position = transform.position + icePickInitPosition;
        isStart = true;
        UIButton.SetActive(true);
    }
	// Update is called once per frame
	void Update ()
    {
        // ミニゲームを開始しているとき
        if(isStart)
        {
            // プレイヤーの入力Pointのほうが
            // freesResistより大きいとき
            if (playerBarrage > freesResist)
            {
                // freesResist加算
                AddFreesResist();

            }
            // 氷のスケールをセット
            SetIceImageScale();
            MoveIcePick();
        }
	}

    public bool AddPlayerBarrage()
    {
        playerBarrage += addBarragePoint;
        Vector3 scale = iceImage.transform.localScale;
        iceFragment.GetComponent<IceFragment>().EffectPlay();
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
            pos.x += 0.016f;
            pos.y += 0.016f;
        }
        else
        {
            pos.x -= 0.016f;
            pos.y -= 0.016f;
        }
        icePickImage.transform.position = pos;
        moveframe++;
        if (moveframe >= 5)
        {
            moveflg = !moveflg;
            moveframe = 0;
        }
    }

    private void AddFreesResist() => freesResist += 0.1f;

    private void SetIceImageScale()
    {
        Vector3 scale = iceImage.transform.localScale;
        float fval = ((playerBarrage - freesResist) / takeCount);
        float Scale = 1f - fval;
        if (Scale > 1) Scale = 1f;
        iceImage.transform.localScale = new Vector3(Scale, Scale, Scale);
    }

	public bool GetMoveflg() => moveflg;

    public void SetInitPos(Vector3 pos) => icePickInitPosition = pos;

}
