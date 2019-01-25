using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlag : MonoBehaviour {

    bool aliensRestartFlag;
    int restartTime;
    float count;
    static float countMax =5;
    private static bool bossFlag;
    private static bool bossInFlag;

    public static bool offbossFlag; //ボス終了退避フラグ
    private bool bossVibrationFlag;
    
    static bool bossActiveTimeFlag;
    public static float normalAlientime;
    static bool oneActiveSelf;

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

    [SerializeField] float height=0.8f; //落下高さ
    [SerializeField] float g =0.8f; //加速度
    [SerializeField] float damping=0.99f;  //減衰
    float dy; //y方向の速度
    float dyOld;
    int dyCount;
    static int dyCountMax=100;
    int r; // 円の半径
    float timeleft;

    private int Speed = 0;

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
       bossInFlag =false;
       offbossFlag =false;
       bossVibrationFlag =false;
       //CreateAlien_Stop();

       normalAlientime = 5;

       //揺れ初期化
       dy= 0; 
       dyOld= 0;;
       dyCount=0;
       r = 0;
       timeleft=0;

       Speed=1;
    }
	


	// Update is called once per frame
	void Update () {
        //デバッグ用
        //if (Input.GetKeyDown("0"))
        //{
        //    bossFlag = true;
                   
        //}
        if (Input.GetKeyDown("9"))
        {
            
              Speed +=1;
                   
        }

        BossKnockDown();
        BossActiveTime();
        BossActive();
        //BossSetLeaving();//ToDo　退避仮置き
        
        //画面揺らす処理
        //BossVibration();


	}

 
    //残りスコア加算
    public void BossKnockDown()
    {
        if(bossFlag ==false 
            && bossActiveTimeFlag == true 
            && offbossFlag == false
            && AliensBoss.bossEatCount<=0)
        {
              Speed =5;
              gameTimeManager.GetComponent<GameTimeManager>().SetTimeSpeed(Speed);
              gameTimeManager.GetComponent<GameTimeManager>().SetBossKnockDownFlag();
        }
        //GetComponent<AlienCall>().AlienLeaving();    
   
    }

    //ToDo　ノーマル再出現　修正箇所
    public static void AliensRestart()
    {

        normalAlientime += Time.deltaTime;
        bossFlag =false; 
        
    }


   //ボス出現開始時間
    private void BossActiveTime()
    {


        if(gameTimeManager.GetCountTime()<=bossActiveTime && bossActiveTimeFlag ==false)
        {
            bossFlag = true;
            bossInFlag =true;
            bossVibrationFlag =true;
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
                
                //GetComponent<AlienCall>().BossOne=false;
            }
        }



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
    static public void BossSetLeaving()
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
        if(bossFlag==true &&bossVibrationFlag ==true)//&&bossVibrationFlag ==true
        {
            //カメラ揺らす
            //CameraObjTransform.position.y +=1.0f ;
            //CameraObj.transform.position.y+=1.0f;

            //時間ごとに計算
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.98f) {
            timeleft = 1.0f;

                pos = CameraObj.transform.position;

                dy = dy + g; //加速度による速度の変化を計算
                pos.y= pos.y + dy;     //ｙ座標を動かす                
                if (pos.y>height) { 
                pos.y = height;  //めり込み調整
                dy = -dy * damping;
                //Debug.Log("dy"+dy);
                Debug.Log("pos.y"+pos.y);
                //Debug.Log("g/2:"+(g/2+0.2f));
                    if (System.Math.Abs(dy)<=(System.Math.Abs(dyOld)+0.0005f)
                        ||dyCount>=dyCountMax) { //ほぼ止まった
                        dy=0; 
                        pos.y=0;
                        //ToDoフラグ立てる
                        //一度だけ通る
                        bossVibrationFlag =false;                     
                        
                        Debug.Log("bossVibrationFlag"+bossVibrationFlag);
                    }
                dyCount++;
                    if(dyCount%10==0){
                        dyOld = dy;
                    }
                }
                CameraObj.transform.position = pos;
             
            }
            else
            {                
                //CameraObj.transform.position = pos;
                //Debug.Log("pos.y"+pos.y);
            }

        }
       
        if(bossVibrationFlag == false)
        {
            //pos= CameraObj.transform.position;           
            pos.y= 0;
            CameraObj.transform.position = pos;
            
        }
        //Debug.Log("pos.y"+pos.y);
        //Debug.Log("bossVibrationFlag"+bossVibrationFlag);
      
    }

    
    //ボスフラグ
    public static bool GetBossFlag() => bossFlag;

    public static float GetNormalAlientime() =>normalAlientime;

    public static bool GetBossInFlag() => bossInFlag;

    public static bool SetBossFlag() => bossFlag = !bossFlag;
}
