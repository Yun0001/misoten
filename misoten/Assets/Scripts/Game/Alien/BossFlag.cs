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
    public static float normalAlientime = 3;
    static bool oneActiveSelf =false;

    [SerializeField]
    private GameObject BossAlien;

    private GameObject CameraObj;
    private Transform CameraObjTransform;
    private Vector3 pos;

    // ゲーム制限時間スクリプト変数
	GameTimeManager gameTimeManager;

    [SerializeField]
    private int bossActiveTime ;
    [SerializeField]
    private int bossActiveDifferece ;

    float height=2; //落下高さ
    float g =1.0f; //加速度
    float damping=0.5f;  //減衰
    float dy; //y方向の速度
    int r = 0; // 円の半径
    float timeleft;

	// Use this for initialization
	void Start () {
        aliensRestartFlag = false;
        restartTime = 5;
        count =5;
        bossActiveTimeFlag = false;
        oneActiveSelf =false;
       
        gameTimeManager = GameObject.Find("TimeManager").gameObject.GetComponent<GameTimeManager>();
        CameraObj = GameObject.Find("Camera");
        CameraObjTransform = GameObject.Find("Camera").gameObject.GetComponent<Transform>();
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

        //if (Input.GetKeyDown("6"))
        //{
        //    //非アクティブ化
        //    if (BossAlien.gameObject.activeSelf == true)
        //    {
        //        BossAlien.SetActive(false);
        //    }
        //    BossAlien.SetActive(false);
        //    Debug.Log("BossAlien");
        //}



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
        //BossSetLeaving();//ToDo　退避仮置き
        //BossVibration();

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
    public static void AliensRestart()
    {
        //GetComponent<AlienCall>().AliensEntershop();
        //AlienMove.LeavingStoreFlag = false;
        normalAlientime += Time.deltaTime;
        bossFlag =false;

       
        
    }

    private void AliensMax()
    {
        //GetComponent<AlienCall>().AliensMaxBefore();
    }

    //ボスフラグ
    public static bool GetBossFlag() => bossFlag;

    public static float GetNormalAlientime() =>normalAlientime;

   //ボス出現開始時間
    private void BossActiveTime()
    {
        //gameTimeManager.GetCountTime();
        //Debug.Log("gameTime" +gameTimeManager.GetCountTime());

        if(gameTimeManager.GetCountTime()<=bossActiveTime && bossActiveTimeFlag ==false)
        {
            bossFlag = true;
            //ToDo　演出追加
            //Debug.Log("ボス出現開始" );

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
            if (BossAlien.gameObject.activeSelf == false && oneActiveSelf == false)
            {
                BossAlien.SetActive(true);
                oneActiveSelf =true;
            }
        }


        //Debug.Log("bossActiveTimeFlag" +bossActiveTimeFlag);
        //Debug.Log("offbossFlag" +offbossFlag);
        if(  normalAlientime >1 
            && bossActiveTimeFlag == false && offbossFlag ==true)
        {
            ////非アクティブ化
            //if (BossAlien.gameObject.activeSelf == true)
            //{
            //    BossAlien.SetActive(false);
            //}
            //Debug.Log("ボス非アクティブ関数");

            GetComponent<AliensBoss>().BossDestroy();
        }


    }

    //ボス終了処理
    public static void BossSetLeaving()
    {
        if(bossFlag ==false 
            && bossActiveTimeFlag == true 
            && offbossFlag == false
            && AliensBoss.bossEatCount<=0)
        {
            bossActiveTimeFlag =false;
            offbossFlag=true;
            //AlienCall.BossOne = false;
            Debug.Log("退避" );
        }
        //Debug.Log("bossFlag" +bossFlag);
        //Debug.Log("bossActiveTimeFlag" +bossActiveTimeFlag);
        //Debug.Log("offbossFlag" +offbossFlag);
        //Debug.Log("bossEatCount" +AliensBoss.bossEatCount);

    }

    public void BossVibration()
    {
        if(bossFlag==true)
        {
            //カメラ揺らす
            //CameraObjTransform.position.y +=1.0f ;
            //CameraObj.transform.position.y+=1.0f;

            //時間ごとに計算
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.98) {
            timeleft = 1.0f;

                pos = CameraObj.transform.position;

                dy = dy + g; //加速度による速度の変化を計算
                pos.y= pos.y + dy;     //ｙ座標を動かす
                Debug.Log("dy"+dy);
                if (pos.y>height-r) { //下の壁
                pos.y = height-r;  //めり込み調整
                dy = -dy * damping;
                    if (System.Math.Abs(dy)<=g/2) { //ほぼ止まった
                        dy=0; 
                        pos.y=0;
                        //ToDoフラグ立てる
                    }
                }
            }

            CameraObj.transform.position = pos;
            Debug.Log("pos.y"+pos.y);
        }
      
    }

    public static bool SetBossFlag() => bossFlag = !bossFlag;
}
