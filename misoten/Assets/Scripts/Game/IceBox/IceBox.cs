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

    private int[,] emissionRate = {
        { 33,33,33,0,0,0},// オーダーなし
        { 15,15,69,0,0,0},// 黄オーダー
        { 40,10,40,0,0,9},// 橙オーダー
        { 69,15,15,0,0,0},// 赤オーダー
        { 40,40,10,9,0,0},// 紫オーダー
        { 15,69,15,0,0,0},// 青オーダー
        { 10,40,40,0,9,0},// 緑オーダー        
    };

    // Use this for initialization
    void Awake ()
    {
        // テクスチャロード
        eatoySprite = Resources.LoadAll<Sprite>("Textures/Eatoy/Eatoy_OneMap");
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
        // 客の注文している料理を参照する
        bool IsChangeEatoy = false;

        random = new System.Random();
        // 一番最初に入店してきた客
        if (IsChangeEatoy)
        {
            // チェンジイートイがある時
        }
        else
        {
            // ベースイートイだけの時
            val = random.Next(1, 7);
            if (val == 2 || val == 4 || val == 6)
            {
                val--;
            }
        }

        return val;
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
        putEatoy.GetComponent<Eatoy>().Init(eatoyID, eatoySprite[eatoyID - 1]);
        putEatoy.GetComponent<SpriteRenderer>().enabled = false;
    }

    public int GetIceBoxID() => iceBoxID;



    /// <summary>
    /// 排出率決定
    /// </summary>
    private void DecisionEmissionRate()
    {

    }
}
