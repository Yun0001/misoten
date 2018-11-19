using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimatorManager : MonoBehaviour {

    [SerializeField] private bool isPause = false;

    private GameObject[] players;
    private GameObject[] stageModels;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        stageModels = GameObject.FindGameObjectsWithTag("StageModel");
    }

    // Update is called once per frame
    void Update () {
        if (isPause)
        {
            PauseAnim();
        }
        else
        {
            ActiveAnim();
        }
    }

    void PauseAnim()
    {
        // animatorのついたオブジェクトの停止
        foreach (GameObject player in players)
        {
            player.GetComponent<PauseAnimation>().SetIsPause(true);
        }
        foreach (GameObject stageModel in stageModels)
        {
            stageModel.GetComponent<PauseAnimation>().SetIsPause(true);
        }
    }

    void ActiveAnim()
    {
        // animatorのついたオブジェクトの復帰
        foreach (GameObject player in players)
        {
            player.GetComponent<PauseAnimation>().SetIsPause(false);
        }
        foreach (GameObject stageModel in stageModels)
        {
            stageModel.GetComponent<PauseAnimation>().SetIsPause(false);
        }
    }

    public void SetIsPause(bool b) => isPause = b;

    void DebugLog()
    {
        foreach (GameObject player in players)
        {
            Debug.Log(player.name);
        }
        foreach (GameObject stageModel in stageModels)
        {
            Debug.Log(stageModel.name);
        }
    }

}
