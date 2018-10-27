using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookWareAnimCtrl : MonoBehaviour {

    private Animator _animator;
    private bool isAnimation = false;

    public bool isLooping = false;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        OneAction();
        LoopAction();

        _animator.SetBool("isLooping", isLooping);

    }

    void OneAction()
    {
        if (isAnimation)
        {
            isAnimation = false;
            _animator.Play("action");
        }
    }

    void LoopAction()
    {
        if (isLooping)
        {
            _animator.Play("loopAction");
        }
    }

}
