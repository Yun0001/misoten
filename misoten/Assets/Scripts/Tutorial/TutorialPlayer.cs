using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour {

    [SerializeField] private bool isComplete = false;

    void Start()
    {
    }

    void Update() {
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public void UnComplete() => isComplete = false;

}
