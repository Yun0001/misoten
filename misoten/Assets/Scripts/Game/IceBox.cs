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
    private GameObject[] eatoys;

    private System.Random random = new System.Random();

    [SerializeField]
    private GameObject MiniGameUI;

    [SerializeField]
    private GameObject putEatoy;

    [SerializeField]
    int val;

	// Use this for initialization
	void Awake () {
		
	}


    public bool Access(int pID)
    {
        if (status < Status.AccessOn || status > Status.AccessThree)
        {
            Debug.LogError("この状態の時はアクセスできません");
            return false;
        }

        status++;
        if (status < Status.AccessOne || status > Status.AccessFull)
        {
            Debug.LogError("IceBox 不正な状態");
            return false;
        }


        for (int i = 0; i < playerAccessOrder.Length; i++)
        {
            if (playerAccessOrder[i] >= MAX_PLAYER)
            {
                playerAccessOrder[i] = pID;
                break;
            }
        }

        MiniGameUI.GetComponent<IceBoxMiniGame>().flgOn();
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

        //int val = 100;

        // 一番最初に入店してきたきゃじゅ
        if (IsChangeEatoy)
        {
            // チェンジイートイがある時
        }
        else
        {
            // ベースイートイだけの時
            val = (int)Mathf.Ceil(((random.Next(1, 100) / 33)));
        }

        return val;
    }

    public void ActionMiniGame()
    {
        if (MiniGameUI.GetComponent<IceBoxMiniGame>().AddPlayerBarrage())
        {
            MiniGameUI.GetComponent<IceBoxMiniGame>().Init();
            putEatoy = eatoys[DecisionPutEatoyElement()];
            status = Status.Take;
        }
    }

    public GameObject PassEatoy() => putEatoy;

    public void ResetEatoy()
    {
        putEatoy = null;

        for (int i = 1; i < playerAccessOrder.Length; i++)
        {
            playerAccessOrder[i - 1] = playerAccessOrder[i];
        }
        playerAccessOrder[MAX_PLAYER - 1] = MAX_PLAYER;

        status = Status.AccessOn;
        for (int i = 0; i < playerAccessOrder.Length; i++)
        {
            if (playerAccessOrder[i] < MAX_PLAYER) status++;
        }

    }
    public bool IsPutEatoy() => status == Status.Take;

    public bool IsAccessOnePlayer(int pID)
    {
        return pID == playerAccessOrder[0];
    }
}
