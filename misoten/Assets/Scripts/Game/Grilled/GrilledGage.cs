using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private EArea[,] pattern = new EArea[PATTERN_NUM, SUCCESSAREA_NUM] {
        { EArea.Hard, EArea.Hell, EArea.Normal, EArea.Hell, EArea.Hell },
        { EArea.Hell, EArea.Normal, EArea.Hell, EArea.Hard, EArea.Hell },
        { EArea.Normal, EArea.Normal, EArea.Normal, EArea.Normal, EArea.Hard }, 
        { EArea.Hard, EArea.Hard, EArea.Hard, EArea.Hell, EArea.Hell }
    };

    [SerializeField]
    private EGrilledGageStatus grilledGageStatus;

    private GameObject[] successAreaPrefab = new GameObject[3];
    private string successAreaPrefabPass = "Prefabs/GrilledGage/";
    private string[] successAreaPrefabName = { "SuccessArea_Normal", "SuccessArea_Hard", "SuccessArea_Hell" };
    
    // 各パターンは５つのエリアが流れてくる
    private GameObject[,] allPatternSuccessArea = new GameObject[PATTERN_NUM, SUCCESSAREA_NUM];

    // ノーツを流す時に使う変数
    [SerializeField]
    private GameObject[] successAreaGroup = new GameObject[SUCCESSAREA_NUM];
    
    // エリア移動速度
    [SerializeField]
    private float areaSpeed;

    // ゲージ表示から流れてくるまでのフレーム数
    [SerializeField]
    private float STAY_TIME =1;
    [SerializeField]
    private float stayTime;

    private TimingPoint timingPoint_cs;





    private void Awake()
    {
        grilledGageStatus = EGrilledGageStatus.Standby;
        var parent = this.transform;
        // SuccessAreaをロード
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
                allPatternSuccessArea[j, i].SetActive(false);
            }
        }

        timingPoint_cs = transform.Find("TimingPoint").GetComponent<TimingPoint>();
}


    public void Init(int pRank)
    {
        grilledGageStatus = EGrilledGageStatus.Stay;
        stayTime = STAY_TIME;
        DisicionFlowSuccessArea(pRank);
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
            pos.y += 0.08f;
            successAreaGroup[i].transform.position = pos;
            successAreaGroup[i].SetActive(true);
        }
    }

    public void ResetSuccessArea()
    {
        ResetPosition();
        for (int i = 0; i < successAreaGroup.Length; i++)
        {
            if (successAreaGroup[i].tag == "SuccessAreaNormalParent")
            {
                successAreaGroup[i].transform.Find("SuccessArea_Normal1").GetComponent<SuccessArea>().isInGageFrame = false;
                successAreaGroup[i].transform.Find("SuccessArea_Normal1").GetComponent<SuccessArea>().isOutGageFrame = false;
                successAreaGroup[i].transform.Find("SuccessArea_Normal1").GetComponent<SuccessArea>().Init();
                successAreaGroup[i].transform.Find("SuccessArea_Normal2").GetComponent<SuccessArea>().isInGageFrame = false;
                successAreaGroup[i].transform.Find("SuccessArea_Normal2").GetComponent<SuccessArea>().isOutGageFrame = false;
                successAreaGroup[i].transform.Find("SuccessArea_Normal2").GetComponent<SuccessArea>().Init();
            }
            else
            {
                successAreaGroup[i].GetComponent<SuccessArea>().isInGageFrame = false;
                successAreaGroup[i].GetComponent<SuccessArea>().isOutGageFrame = false;
                successAreaGroup[i].GetComponent<SuccessArea>().Init();
            }
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
    public bool CookingGrilledCuisine()
    {
        switch (grilledGageStatus)
        {
            case EGrilledGageStatus.Stay:
                if (CountDownStayTime() <= 0)
                {
                    SetStatus(EGrilledGageStatus.Play);
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
                    if (successAreaGroup[i].activeInHierarchy) return false;
                }

                return true;
                //grilledGageStatus = EGrilledGageStatus.End;
                //break;

            case EGrilledGageStatus.End:
                break;
        }

        return false;
    }

    public EGrilledGageStatus GetStatus() => grilledGageStatus;

    public void SetStatus(EGrilledGageStatus status) => grilledGageStatus = status;

    private float CountDownStayTime() => stayTime -= Time.deltaTime;

    public void ResetStayTime() => stayTime = STAY_TIME;

    public GameObject DecisionIsHit()
    {
        for (int i = 0; i < 4; i++)
        {
            if (timingPoint_cs.IsHit(i))
            {
                return GetTimingPointHitObj(i);
            }
        }
        return null;
    }

    public GameObject GetTimingPointHitObj(int i)
    {
        return timingPoint_cs.GetHitObj(i);
    }

    public void ResetIsHit(int i)
    {
        timingPoint_cs.SetIsHit(i, false);
        timingPoint_cs.ResetHitObj(i);
    }
}
