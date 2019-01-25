using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensBoss : MonoBehaviour {


    public static int bossEatCount =10;

    private bool eatCountstartFlag = false;
    private int BossTotalEatScore = 0;
    
    [SerializeField]
    public GameObject BossAlien;

    // Use this for initialization
    void Start()
    {
        //eatCountFlag = false;
        BossTotalEatScore = 0;
        bossEatCount = 10;
        BossAlien = GameObject.Find("BossAlien");
    }

    void Awake()
    {
        BossFlag.normalAlientime = 0;
      
    }


    // Update is called once per frame
    void Update()
    {
        if (BossFlag.GetBossFlag() == true && AlienSatisfaction.eatCountFlag == true)
        {
            BossEatCount();
            //一度だけ処理
            AlienSatisfaction.eatCountFlag = false;
        }

        BossLeaving();

        if (Input.GetKeyDown("7"))
        {
            bossEatCount -= 1;
            //Debug.Log("EatCount" + bossEatCount);
        }

        if (Input.GetKeyDown("6"))
        {
            Debug.Log("activeSelf" + BossAlien.gameObject.activeSelf);

            //非アクティブ化
            if (BossAlien.gameObject.activeSelf == true)
            {
                BossAlien.SetActive(false);
                Destroy(BossAlien);
                Debug.Log("ボスDestroy");
            }
           
            //Debug.Log("BossAlien");
        }
    }


    public void BossEatCount()
    {
        //ScoreManager.bossEatScore = 0;

        //bossEatCount -= 1;
        //ボススコアイートイカウントで帰る処理
        if (ScoreManager.bossEatScore < 500)
        {
            BossTotalEatScore += ScoreManager.bossEatScore;
            bossEatCount -= 1;
            //if (eatCountstartFlag)
            //{

            //}
            //eatCountstartFlag = true;

        }
        else if (ScoreManager.bossEatScore < 1000)
        {
            bossEatCount -= 1;
            BossTotalEatScore = 0;
        }
        else if (ScoreManager.bossEatScore < 5000)
        {
            bossEatCount -= 2;
            BossTotalEatScore = 0;
        }
        else if (ScoreManager.bossEatScore >= 5000)
        {
            bossEatCount -= 3;
            BossTotalEatScore = 0;
        }
        //Debug.Log("BossScore" + ScoreManager.bossEatScore);
        Debug.Log("EatCount" + bossEatCount);

        if (bossEatCount <= 0)
        {
            Debug.Log("カウント超えました");
        }
    }

    //TODOボス退避
    private void BossLeaving()
    {
        if (bossEatCount <= 0)
        {
            BossFlag.SetBossFlag();
            //Debug.Log("BossFlag" + BossFlag.GetBossFlag());
            //ToDo数秒後退避　退避フラグ他に立てる
            BossFlag.BossSetLeaving();
            //Debug.Log("BossLeaving");
            BossFlag.AliensRestart();
        }
    }

    public void BossDestroy()
    {
        //非アクティブ化
        if (BossAlien.gameObject.activeSelf == true)
        {
            BossAlien.SetActive(false);
            Destroy(BossAlien);
            Debug.Log("ボスDestroy");
        }
        Destroy(BossAlien);
    }

    //public int GetBossEatCount() => bossEatCount;
}
