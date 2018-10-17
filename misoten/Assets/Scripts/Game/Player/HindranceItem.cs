using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class HindranceItem : MonoBehaviour
{
    // 定数
    public readonly static int MAXTASTE = 3;
    private readonly static float GAGESCALE = 0.015f;
    private readonly static float GAGE_YPOS = 1.5f;


    [SerializeField][Range(0.0f,5.0f)]
    private float[] hindranceArea;

    /// <summary>
    /// スタート時の味の素の状態
    /// true:Max,false:0
    /// </summary>
    [SerializeField]
    private bool isStartYummyBottleFull;

    /// <summary>
    /// 味の素
    /// </summary>
    [SerializeField]
    private int yummyBottle;


    //ゲージスクリプト
    //ゲージプレハブ
    [SerializeField]
    private GameObject tasteGagePrefab;
    private GameObject tasteGage;
    private TasteGage tasteGage_cs;

    /// <summary>
    /// 味の素プレハブ
    /// </summary>
    private GameObject tastePrefab;

 

    // Use this for initialization
    void Awake()
    {
        InitTasteGage();
        tastePrefab = Resources.Load("Prefabs/Taste") as GameObject;
        // スタート時のボトルの中身
        if (isStartYummyBottleFull) yummyBottle = MAXTASTE;
        else yummyBottle = 0;
    }

    /// <summary>
    /// Tasteゲージを表示
    /// </summary>
    public void DisplayTasteGage()
    {
        SetTasteGagePos();
        tasteGage.SetActive(true);
        tasteGage_cs.Init();
        GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.TasteCharge);
    }

    /// <summary>
    /// Tasteゲージ更新
    /// </summary>
    public void UpgateTasteGage()
    {
        tasteGage_cs.UpdateGage();
        SetTasteGagePos();
    }

    /// <summary>
    /// 味の素を発生
    /// </summary>
    public void SprinkleTaste()
    {
        if (tasteGage_cs.GetSmokeLevel() != TasteGage.ESmokeLevel.Zero)
        {
            int smokeLevel = (int)tasteGage_cs.GetSmokeLevel();
            float area = hindranceArea[smokeLevel - 1];
            // 味の素実体化
            InstanceTaste(area);

            // ボトルの残量を減らす
            SubYummyBottle(smokeLevel);

            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Hindrance);
        }
        else
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Normal);
        }
        // ゲージ非表示
        tasteGage.SetActive(false);

    }

    public int GetYummyBottle() => yummyBottle;

    /// <summary>
    /// tasteゲージの座標セット
    /// </summary>
    private void SetTasteGagePos()
    {
        Vector3 pos = transform.position;
        pos.y += GAGE_YPOS;
        tasteGage.transform.position = pos;
    }

    /// <summary>
    /// 味の素残量を減らす
    /// </summary>
    /// <param name="smokeLevel"></param>
    private void SubYummyBottle(int smokeLevel)
    {
        if (smokeLevel == (int)TasteGage.ESmokeLevel.First)
        {
            yummyBottle--;
        }
        else
        {
            // Burstの時は０にする
            yummyBottle = 0;
        }
    }

    /// <summary>
    /// 味の素実体化
    /// </summary>
    /// <param name="area">味の素のスケール</param>
    private void InstanceTaste(float area)
    {
        GameObject taste = Instantiate(tastePrefab, transform.position, Quaternion.identity);
        taste.GetComponent<Taste>().playerID = GetComponent<Player>().GetPlayerID();
        taste.transform.localScale = new Vector3(area, area, area);
    }

    /// <summary>
    /// Tasteゲージ関連の変数初期化
    /// </summary>
    private void InitTasteGage()
    {
        tasteGagePrefab = Resources.Load("Prefabs/UI/Tastegage") as GameObject;
        tasteGage = Instantiate(tasteGagePrefab, transform.position, Quaternion.identity);
        tasteGage.transform.localScale = new Vector3(GAGESCALE, GAGESCALE, GAGESCALE);
        tasteGage.SetActive(false);
        tasteGage_cs = tasteGage.transform.Find("Gage").GetComponent<TasteGage>();
        tasteGage_cs.SetHindranceItemCs(GetComponent<HindranceItem>());
    }

    /// <summary>
    /// 味の素補充
    /// </summary>
    public void ReplenishmentTaste() => yummyBottle = MAXTASTE;
}
