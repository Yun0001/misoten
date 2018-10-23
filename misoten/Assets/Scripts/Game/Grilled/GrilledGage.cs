using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilledGage : MonoBehaviour {

     enum EArea
    {
        Normal,
        Hard,
        Hell
    }

    public enum EGrilledGageStatus
    {
        Standby,
        Stay,
        Play,
        End
    }

    private const int PATTERN_NUM = 4;
    private const int SUCCESSAREA_NUM = 5;

    EArea[,] pattern = new EArea[PATTERN_NUM, SUCCESSAREA_NUM] {
        { EArea.Hard, EArea.Hell, EArea.Normal, EArea.Hell, EArea.Hell },
        { EArea.Hell, EArea.Normal, EArea.Hell, EArea.Hard, EArea.Hell },
        { EArea.Normal, EArea.Normal, EArea.Normal, EArea.Normal, EArea.Hard }, 
        { EArea.Hard, EArea.Hard, EArea.Hard, EArea.Hell, EArea.Hell }
    };



    private GameObject[] successAreaPrefab = new GameObject[3];
    private string successAreaPrefabPass = "Prefabs/GrilledGage/";
    private string[] successAreaPrefabName = { "SuccessArea_Normal", "SuccessArea_Hard", "SuccessArea_Hell" };
    private Vector3[] scale = { new Vector3(0.1f, 0.4f, 1f), new Vector3(0.1f, 0.4f, 1f), new Vector3(0.1f, 0.4f, 1f) };
    private float[] successAreaPosx = { 2.0f, 3.0f, 4.0f, 5.0f, 6.0f };
    private TimingPoint timingPoint_cs;
    
    // 各パターンは５つのエリアが流れてくる
    [SerializeField]
    private GameObject[,] allPatternSuccessArea = new GameObject[PATTERN_NUM, SUCCESSAREA_NUM];

    // エリアを流す時に使う変数
    GameObject[] successAreaGroup = new GameObject[SUCCESSAREA_NUM];
    

    // エリア移動速度
    [SerializeField]
    private float areaSpeed;

    // ゲージ表示から流れてくるまでのフレーム数
    [SerializeField]
    private readonly float STAY_FRAME;
    private float stayFrame;

    private EGrilledGageStatus grilledGageStatus;



    private void Awake()
    {
        grilledGageStatus = EGrilledGageStatus.Standby;
        var parent = this.transform;
        // SuccessAreaを作成
        for (int i = 0; i < successAreaPrefab.Length; i++)
        {
            successAreaPrefab[i] = Resources.Load(successAreaPrefabPass + successAreaPrefabName[i]) as GameObject;
        }

        // 全パターン分作成
        for (int j = 0; j < PATTERN_NUM; j++)
        {
            for (int i = 0; i < SUCCESSAREA_NUM; i++)
            {
                allPatternSuccessArea[j, i] = Instantiate(successAreaPrefab[(int)pattern[j, i]], transform.position, Quaternion.identity, parent);
                //allPatternSuccessArea[j, i].transform.localScale = new Vector3(0.2f, 0.4f, 1f);

                //allPatternSuccessArea[j, i].GetComponent<SuccessArea>().SetMoveSpeed(areaSpeed);
                allPatternSuccessArea[j, i].SetActive(false);
            }
        }
    }


    public void Init(int pRank)
    {
        grilledGageStatus = EGrilledGageStatus.Stay;
        stayFrame = STAY_FRAME;
        DisicionFlowSuccessArea(pRank);
    }

    public void GagePlay()
    {
        grilledGageStatus = EGrilledGageStatus.Play;
    }

    /// <summary>
    /// 順位に応じたGroupを使う
    /// </summary>
    /// <param name="pRank"></param>
    private void DisicionFlowSuccessArea(int pRank)
    {
        for (int i = 0; i < successAreaGroup.Length; i++)
        {
            successAreaGroup[i] = allPatternSuccessArea[pRank - 1, i];
            Vector3 pos = successAreaGroup[i].transform.position;
            pos.x += i + 2f;
            successAreaGroup[i].transform.position = pos;
            successAreaGroup[i].SetActive(true);
        }
    }


    public void ResetPosition()
    {
        for (int i = 0; i < successAreaGroup.Length; i++)
        {
            Vector3 pos = transform.position;
            successAreaGroup[i].transform.position= pos;
        }
           
    }
    


    // Update is called once per frame
    void Update()
    {
        switch (grilledGageStatus)
        {
            case EGrilledGageStatus.Stay:
                stayFrame -= Time.deltaTime;
                if (stayFrame <= 0)
                {
                    GagePlay();
                }
                break;

            case EGrilledGageStatus.Play:
                // successareaを動かす
                for (int i = 0; i < successAreaGroup.Length; i++)
                {
                    Vector3 pos = successAreaGroup[i].transform.position;
                    pos.x -= areaSpeed;
                    successAreaGroup[i].transform.position = pos;
                }
                for (int i = 0; i < successAreaGroup.Length; i++)
                {
                    if (successAreaGroup[i].activeInHierarchy) return;
                }
                grilledGageStatus = EGrilledGageStatus.End;
                break;

            case EGrilledGageStatus.End:
                break;
        }
    }

    public EGrilledGageStatus GetStatus() => grilledGageStatus;

    public void SetStatus(EGrilledGageStatus status) => grilledGageStatus = status;
}
