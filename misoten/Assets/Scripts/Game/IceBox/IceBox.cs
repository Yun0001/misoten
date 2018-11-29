using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceBox : MonoBehaviour {

    enum Status
    {
        AccessOff,
        AccessOn,
        AccessOne,
        AccessTwo,
        AccessThree,
        AccessFull,
        Take
    }

    [SerializeField]
    private Status status = Status.AccessOff;

    private readonly static int MAX_PLAYER = 4;
    private int[] playerAccessOrder = Enumerable.Repeat(MAX_PLAYER, MAX_PLAYER).ToArray();

    [SerializeField]
    private GameObject eatoyPrefab;

    [SerializeField]
    private GameObject putEatoy;

    private System.Random random = new System.Random();

    [SerializeField]
    private GameObject MiniGameUI;

    private Sprite[] eatoySprite;

    [SerializeField]
    int val;

    [SerializeField]
    private bool isDebugMode;

    [SerializeField]
    private int iceBoxID;

    private int emissionRateID = 0;

    /*
     * 0　黄
     * 1　橙
     * 2　赤
     * 3　紫
     * 4　青
     * 5　緑
     * */
    private int[,] emissionRate = {
        { 33,0,33,0,33,0},// オーダーなし
        { 69,0,15,0,15,0},// 黄オーダー
        { 40,9,40,0,10,0},// 橙オーダー
        { 15,0,69,0,15,0},// 赤オーダー
        { 10,0,40,9,40,0},// 紫オーダー
        { 15,0,15,0,69,0},// 青オーダー
        { 40,0,10,0,40,9},// 緑オーダー        
    };

    // Use this for initialization
    void Awake ()
    {
        // テクスチャロード
        eatoySprite = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
        emissionRateID = 0;
    }

    /// <summary>
    /// プレイヤーアクセス
    /// </summary>
    /// <param name="pID">プレイヤーID</param>
    /// <returns></returns>
    public bool Access(int pID)
    {
        // 冷蔵庫がアクセス可状態以外なら抜ける
        if (status < Status.AccessOn || status > Status.AccessThree)
        {
            Debug.LogError("この状態の時はアクセスできません");
            return false;
        }

        // ステータスを進める
        status++;
        if (status < Status.AccessOne || status > Status.AccessFull)
        {
            Debug.LogError("IceBox 不正な状態");
            return false;
        }

        // プレイヤーのアクセス順を保持
        for (int i = 0; i < playerAccessOrder.Length; i++)
        {
            if (playerAccessOrder[i] >= MAX_PLAYER)
            {
                playerAccessOrder[i] = pID;
                break;
            }
        }

        // 一人目がアクセスした時だけ
        if (status == Status.AccessOne)
        {
            MiniGameUI.GetComponent<IceBoxMiniGame>().Display();
            if (iceBoxID == 0)
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Refrigeratoropen), 5);
            }
            else
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Refrigeratoropen), 6);
            }
            transform.Find("icebox").GetComponent<iceboxAnimCtrl>().SetIsOpen(true);
        }

        return true;
    }

    public void ReturnSatus()
    {
        status--;
        if (status < Status.AccessOn)
        {
            Debug.LogError("IceBox 不正な状態");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (status == Status.AccessOff && other.tag=="Player")
        {
            status = Status.AccessOn;
        }

        // 誰もアクセスしていない時にエイリアンが当たると排出率を変更
        if (status <= Status.AccessOn　&& other.tag == "Alien")
        {
            DecisionEmissionRate(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (status == Status.AccessOff && other.tag == "Player")
        {
            status = Status.AccessOn;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (status == Status.AccessOn && other.tag == "Player")
        {
            status = Status.AccessOff;
        }
    }

    /// <summary>
    /// イートイを生成
    /// </summary>
    /// <returns></returns>
    public int DecisionPutEatoyElement()
    {
        random = new System.Random();
        int sum = 0;
        val = random.Next(1, 99);
        if (val <= emissionRate[emissionRateID, 0])
        {
            return 0;
        }
        for (int i = 1; i < 6; i++)
        {
            sum += emissionRate[emissionRateID, i - 1];
            if (val <= sum + emissionRate[emissionRateID, i])
            {
                return i;
            }
        }


        Debug.LogError("ここまでくるのはありえないはず");
        return 100;
    }

    public void ActionMiniGame()
    { 
        if (MiniGameUI.GetComponent<IceBoxMiniGame>().AddPlayerBarrage() || isDebugMode)
        {
            // ミニゲーム終了時
            // ミニゲームの状態を初期化
            MiniGameUI.GetComponent<IceBoxMiniGame>().Init();
            // イートイ生成
            InstanceEatoty();

            status = Status.Take;
        }
    }

    public GameObject PassEatoy() => putEatoy;

    public void ResetEatoy()
    {
        putEatoy = null;

        // アクセス順をずらす
        for (int i = 1; i < playerAccessOrder.Length; i++)
        {
            playerAccessOrder[i - 1] = playerAccessOrder[i];
        }
        // バグ防止
        playerAccessOrder[MAX_PLAYER - 1] = MAX_PLAYER;

        // アクセスしている人数分ステータスを進める
        Status workStatus = Status.AccessOn;
        for (int i = 0; i < playerAccessOrder.Length; i++)
        {
            if (playerAccessOrder[i] < MAX_PLAYER) workStatus++;
        }
        // ステータス設定
        status = workStatus;

        // まだアクセスしているプレイヤーがいるならミニゲーム開始
        if (status > Status.AccessOn)
        {
            if (iceBoxID == 0)
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Refrigeratoropen), 5);
            }
            else
            {
                Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Refrigeratoropen), 6);
            }

            MiniGameUI.GetComponent<IceBoxMiniGame>().Display();
        }
        else
        {
            transform.Find("icebox").GetComponent<iceboxAnimCtrl>().SetIsOpen(false);
        }
    }
    public bool IsPutEatoy() => status == Status.Take;

    public bool IsAccessOnePlayer(int pID)
    {
        return pID == playerAccessOrder[0];
    }

    /// <summary>
    /// イートイ生成
    /// </summary>
    private void InstanceEatoty()
    {
        putEatoy = Instantiate(eatoyPrefab, transform.position, Quaternion.identity);
        Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
        putEatoy.transform.localScale = scale;
        
        // イートイを初期化
        int eatoyID = DecisionPutEatoyElement();
        putEatoy.GetComponent<Eatoy>().Init(eatoyID, eatoySprite[eatoyID]);
        putEatoy.GetComponent<SpriteRenderer>().enabled = false;
    }

    public int GetIceBoxID() => iceBoxID;



    /// <summary>
    /// 排出率決定
    /// </summary>
    private void DecisionEmissionRate(GameObject Alien)
    {
        AlienOrder order_cs = Alien.GetComponent<AlienOrder>();
        int orderEatoy = 0;
        // オーダーするイートイIDを取得
        if (order_cs.GetOrderType() == 0)
        {
            //  ベースイートイ
            orderEatoy = order_cs.GetOrderBaseSave();
        }
        else
        {
            // チェンジイートイ
            orderEatoy = order_cs.GetOrderChangeSave();
        }

        emissionRateID =  orderEatoy;
    }
}
