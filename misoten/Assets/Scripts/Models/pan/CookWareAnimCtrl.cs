﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookWareAnimCtrl : MonoBehaviour {

    private Animator _animator;
    [SerializeField]
    private bool isAnimation = false;

    [SerializeField]
    private bool isLooping = false;

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

    public void SetBool(bool b) => isLooping = b;
	public bool GetBool() => isLooping;
}