using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyVelocity
{
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public RigidbodyVelocity(Rigidbody rb)
    {
        velocity = rb.velocity;
        angularVelocity = rb.angularVelocity;
    }
}

public class Pause : MonoBehaviour
{
    public bool pausing;
    public GameObject[] ignoreGameObjects;
    private bool prevPausing;
    private RigidbodyVelocity[] rbVelocities;
    private Rigidbody[] pauseingRigidbodies;
    private MonoBehaviour[] pausingMonoBehaviours;

    private GameObject[] players;
    private GameObject[] stageModels;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        stageModels = GameObject.FindGameObjectsWithTag("StageModel"); 
    }

    private void Update()
    {
      
        foreach (GameObject player in players)
        {
            Debug.Log(player.name);
        }
        foreach (GameObject stageModel in stageModels)
        {
            Debug.Log(stageModel.name);
        }

        if (prevPausing != pausing)
        {
            if (pausing) PauseMode();
            else ResumeMode();
            prevPausing = pausing;
        }
    }

    private void PauseMode()
    {
        // Rigidbodyの停止
        // 子要素から有効かつ、このインスタンスでないもの、IgnoreGameObjectに含まれていないMonoBehaviourを抽出
        Predicate<Rigidbody> rigidbodyPredicate = obj => !obj.IsSleeping() && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        pauseingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
        rbVelocities = new RigidbodyVelocity[pauseingRigidbodies.Length];
        for (int i = 0; i < pauseingRigidbodies.Length; i++)
        {
            // 速度、角速度も保存しておく
            rbVelocities[i] = new RigidbodyVelocity(pauseingRigidbodies[i]);
            pauseingRigidbodies[i].Sleep();
        }

        // MonoBehaviourの停止
        Predicate<MonoBehaviour> monoBehaviourPredicate = obj => obj.enabled && obj != this && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monoBehaviourPredicate);
        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = false;
        }

        // animatorのついたオブジェクトの停止
        foreach (GameObject player in players)
        {
            player.GetComponent<PauseAnimation>().SetIsPause(false);
        }
        foreach (GameObject stageModel in stageModels)
        {
            stageModel.GetComponent<PauseAnimation>().SetIsPause(false);
        }

    }


    private void ResumeMode()
    {
        for (int i = 0; i < pauseingRigidbodies.Length; i++)
        {
            pauseingRigidbodies[i].WakeUp();
            pauseingRigidbodies[i].velocity = rbVelocities[i].velocity;
            pauseingRigidbodies[i].angularVelocity = rbVelocities[i].angularVelocity;
        }

        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = true;
        }

        // animatorのついたオブジェクトの復帰
        foreach (GameObject player in players)
        {
            player.GetComponent<PauseAnimation>().SetIsPause(true);
        }
        foreach (GameObject stageModel in stageModels)
        {
            stageModel.GetComponent<PauseAnimation>().SetIsPause(true);
        }

    }

}
