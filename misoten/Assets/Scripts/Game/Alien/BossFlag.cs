using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlag : MonoBehaviour {

    bool aliensRestartFlag;
    int restartTime;
    float count;
    static float countMax =5;
    private static bool bossFlag = false;
    public static bool offbossFlag =false; //ボス終了退避フラグ
    
    static bool bossActiveTimeFlag = false;

    [SerializeField]
    private GameObject BossAlien;

    // ゲーム制限時間スクリプト変数
	GameTimeManager gameTimeManager;

    [SerializeField]
    private int bossActiveTime = 176;
    [SerializeField]
    private int bossActiveDifferece = 3;

	// Use this for initialization
	void Start () {
        aliensRestartFlag = false;
        restartTime = 5;
        count =5;
        bossActiveTimeFlag = false;
       
        gameTimeManager = GameObject.Find("TimeManager").gameObject.GetComponent<GameTimeManager>();

	}

   void Awake()
    {
       //Debug.Log("boss"+BossFlag.GetBossFlag());
       bossFlag = false;
       offbossFlag =false;
       //CreateAlien_Stop();
    }
	


	// Update is called once per frame
	void Update () {
        //デバッグ用
        if (Input.GetKeyDown("0"))
        {
            //CreateAlien_Stop();
            //BossActive();
            bossFlag = true;
            
        }
        if (Input.GetKeyDown("9"))
        {
            //aliensRestartFlag =true; 
            bossFlag = false;  
            //offbossFlag =true;
        }
        //Debug.Log("bossFlag"+bossFlag);

        //if(bossFlag = true)
        //{
        //    CreateAlien_Stop();
        //}
        //if(offbossFlag= true)
        //{
        //    aliensRestartFlag =true; 
        //    offbossFlag =false;
        //}

        
        BossActiveTime();
        BossActive();
        BossSetLeaving();//ToDo　退避仮置き


        //if (aliensRestartFlag)
        //{
        //    AliensRestart();
        //    count -= 0.2f;
        //    if (count < 0)
        //    {
        //        aliensRestartFlag = false;
        //        count = countMax;        
        //        AliensMax();
        //    }
        //    //Debug.Log(count);
        //}
        //Debug.Log(bossFlag);
         //Debug.Log("boss"+BossFlag.GetBossFlag());
	}

 

    private void CreateAlien_Stop()
    {
        //GetComponent<AlienCall>().AlienLeaving();    

    }

    //ToDo　ノーマル再出現　修正箇所
    private void AliensRestart()
    {
        //GetComponent<AlienCall>().AliensEntershop();
        //AlienMove.LeavingStoreFlag = false;
    }

    private void AliensMax()
    {
        //GetComponent<AlienCall>().AliensMaxBefore();
    }

    //ボスフラグ
    public static bool GetBossFlag() => bossFlag;

   //ボス出現開始時間
    private void BossActiveTime()
    {
        //gameTimeManager.GetCountTime();
        //Debug.Log("gameTime" +gameTimeManager.GetCountTime());

        if(gameTimeManager.GetCountTime()<=bossActiveTime && bossActiveTimeFlag ==false)
        {
            bossFlag = true;
            //ToDo　演出追加
            Debug.Log("ボス出現開始" );

            //一度だけ通るフラグ
            bossActiveTimeFlag = true;
        }

    }

    public void BossActive()
    {
        
        //ToDo　数秒後にボス出現
        if(gameTimeManager.GetCountTime()<=bossActiveTime -bossActiveDifferece 
            && bossFlag ==true)
        {
            //アクティブ化
            if (BossAlien.gameObject.activeSelf == false)
            {
                BossAlien.SetActive(true);
            }
        }
    }

    //ボス終了処理
    public void BossSetLeaving()
    {
        if(bossFlag ==false && bossActiveTimeFlag == true && offbossFlag == false)
        {
            bossActiveTimeFlag =false;
            offbossFlag=true;
            //AlienCall.BossOne = false;
            Debug.Log("退避" );
        }

    }

    


}
