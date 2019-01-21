using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlag : MonoBehaviour {

    bool aliensRestartFlag;
    int restartTime;
    float count;
    static float countMax =5;
    static bool bossFlag = false;
    static bool offbossFlag =false; //ボス終了Flag仮置き


    [SerializeField]
    private GameObject BossAlien;

	// Use this for initialization
	void Start () {
        aliensRestartFlag = false;
        restartTime = 5;
        count =5;
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
            CreateAlien_Stop();
            BossActive();
            bossFlag = true;
            
        }
        if (Input.GetKeyDown("9"))
        {
            aliensRestartFlag =true; 
            bossFlag = false;  
            offbossFlag =true;
        }

        //if(bossFlag = true)
        //{
        //    CreateAlien_Stop();
        //}
        //if(offbossFlag= true)
        //{
        //    aliensRestartFlag =true; 
        //    offbossFlag =false;
        //}


        if (aliensRestartFlag)
        {
            AliensRestart();
            count -= 0.2f;
            if (count < 0)
            {
                aliensRestartFlag = false;
                count = countMax;        
                AliensMax();
            }
            //Debug.Log(count);
        }
        //Debug.Log(bossFlag);
         //Debug.Log("boss"+BossFlag.GetBossFlag());
	}

 

    private void CreateAlien_Stop()
    {
        //GetComponent<AlienCall>().AlienLeaving();    

    }

    private void AliensRestart()
    {
        GetComponent<AlienCall>().AliensEntershop();
        //AlienMove.LeavingStoreFlag = false;
    }

    private void AliensMax()
    {
        GetComponent<AlienCall>().AliensMaxBefore();
    }

    public static bool GetBossFlag() => bossFlag;


    public void BossActive()
    {
        //アクティブ化
        if (BossAlien.gameObject.activeSelf == false)
        {
            BossAlien.SetActive(true);
        }
    }


}
