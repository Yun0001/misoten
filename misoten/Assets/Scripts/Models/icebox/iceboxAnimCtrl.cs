using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceboxAnimCtrl : MonoBehaviour {

    private Animator _animator;
    private bool isAnimation = false;

    [SerializeField]
    private bool isLooping = false;
    [SerializeField]
    private bool isOpen = false;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        OneAction();
        LoopAction();

        _animator.SetBool("isLooping", isLooping);
        _animator.SetBool("isOpen", isOpen);
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
        if (isLooping && (!isOpen))
        {
            _animator.Play("loopAction");
        }
    }

    void OnIsOpen()
    {
        isOpen = true;
    }

    public void SetBool(bool b) => isLooping = b;
    public bool GetBool() => isLooping;

    public void SetIsOpen(bool b) => isOpen = b;
    public bool GetIsOpen() => isOpen;
}
