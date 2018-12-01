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
	private ParticleSystem[] pauseingParticleSystem;
	private Animator[] pauseingAnimation;
	private MonoBehaviour[] pausingMonoBehaviours;

    private Animator[] _allAnimator;
  
    private bool _isPausing = false;
    private void Update()
    {
       if (prevPausing != pausing)
        {
            if (pausing) PauseMode();
            else ResumeMode();
        }
    }

    private void PauseMode()
    {
 
        if (!_isPausing)
        {

            Predicate<Animator> PDCAnimator = obj => Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
            _allAnimator = Array.FindAll(transform.GetComponentsInChildren<Animator>(), PDCAnimator);
            GameObject[] _allAnimatorObj = new GameObject[_allAnimator.Length];

            for (int i=0;i<_allAnimator.Length;i++)
            {
                _allAnimatorObj[i] = _allAnimator[i].gameObject;
            }

            foreach (GameObject animObj in _allAnimatorObj)
            {
                animObj.GetComponent<PauseAnimation>().SetIsPause(true);
            }

            _isPausing = true;
            return;
        }

        // Rigidbodyの停止
        // 子要素から有効かつ、このインスタンスでないもの、IgnoreGameObjectに含まれていないMonoBehaviourを抽出
        Predicate<Rigidbody> rigidbodyPredicate = obj => !obj.IsSleeping() && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		Predicate<ParticleSystem> particleSystemPredicate = obj => Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		Predicate<Animator> animationPredicate = obj => Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;

		pauseingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
		pauseingParticleSystem = Array.FindAll(transform.GetComponentsInChildren<ParticleSystem>(), particleSystemPredicate);
		pauseingAnimation = Array.FindAll(transform.GetComponentsInChildren<Animator>(), animationPredicate);

		rbVelocities = new RigidbodyVelocity[pauseingRigidbodies.Length];
        for (int i = 0; i < pauseingRigidbodies.Length; i++)
        {
            // 速度、角速度も保存しておく
            rbVelocities[i] = new RigidbodyVelocity(pauseingRigidbodies[i]);
            pauseingRigidbodies[i].Sleep();
        }

		for (int i = 0; i < pauseingParticleSystem.Length; i++)
		{
			if (pauseingParticleSystem[i].isPlaying)
			{
				pauseingParticleSystem[i].Pause();
			}
		}

		for (int i = 0; i < pauseingAnimation.Length; i++)
		{
			if (pauseingAnimation[i].GetFloat("Base Layer") == 1.0f)
			{
				pauseingAnimation[i].SetFloat("Base Layer", 0.0f);
			}
		}

		// MonoBehaviourの停止
		Predicate<MonoBehaviour> monoBehaviourPredicate = obj => obj.enabled && obj != this && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monoBehaviourPredicate);
        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = false;
        }

        prevPausing = pausing;
    }


    private void ResumeMode()
    {
        if (_isPausing)
        {

            for (int i = 0; i < pauseingRigidbodies.Length; i++)
            {
                pauseingRigidbodies[i].WakeUp();
                pauseingRigidbodies[i].velocity = rbVelocities[i].velocity;
                pauseingRigidbodies[i].angularVelocity = rbVelocities[i].angularVelocity;
            }

            for (int i = 0; i < pauseingParticleSystem.Length; i++)
            {
                if (pauseingParticleSystem[i].isPaused)
                {
                    pauseingParticleSystem[i].Play();
                }
            }

            for (int i = 0; i < pauseingAnimation.Length; i++)
            {
                if (pauseingAnimation[i].GetFloat("Base Layer") == 0.0f)
                {
                    pauseingAnimation[i].SetFloat("Base Layer", 1.0f);
                }
            }


            foreach (var monoBehaviour in pausingMonoBehaviours)
            {
                monoBehaviour.enabled = true;
            }

            _isPausing = false;
            return;
        }

        Predicate<Animator> PDCAnimator = obj => Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        _allAnimator = Array.FindAll(transform.GetComponentsInChildren<Animator>(), PDCAnimator);
        GameObject[] _allAnimatorObj = new GameObject[_allAnimator.Length];

        for (int i = 0; i < _allAnimator.Length; i++)
        {
            _allAnimatorObj[i] = _allAnimator[i].gameObject;
        }

        foreach (GameObject animObj in _allAnimatorObj)
        {
            animObj.GetComponent<PauseAnimation>().SetIsPause(false);
        }

        prevPausing = pausing;
    }

    public bool IsPausing() => pausing;
}
