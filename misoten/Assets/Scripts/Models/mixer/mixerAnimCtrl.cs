using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mixerAnimCtrl : MonoBehaviour
{

    private Animator _animator;
    private bool isAnimation = false;

    [SerializeField]
    private bool isLooping = false;
    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private bool isComplete = false;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        OneAction();
        
        _animator.SetBool("isLooping", isLooping);
        _animator.SetBool("isOpen", isOpen);
        _animator.SetBool("isComplete", isComplete);
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

    void CompAnimFinish()
    {
        isComplete = false;
    }

    public void SetBool(bool b) => isLooping = b;

    public void SetIsOpen(bool flg) => isOpen = flg;
    public bool GetIsOpen() => isOpen;

}
