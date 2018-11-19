using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour {

    private Animator _animator;

    [SerializeField]    private bool isFront =true;
    [SerializeField]    private bool isServing = false;
    [SerializeField]    private bool isWalking = false;


    void Start () {
        _animator = GetComponent<Animator>();
    }
	
	void Update () {

        AnimationCtrl();

    }

    void AnimationCtrl()
    {

        if (isFront)
        {
            if (isServing)
            {
                if (isWalking)
                {
                    // 配膳・前面・歩き
                    _animator.Play("serving_front_walk");
                }
                else
                {
                    // 配膳・前面・待機
                    _animator.Play("serving_front_idle");
                }
            }
            else
            {
                if (isWalking)
                {
                    // 前面・歩き
                    _animator.Play("front_walk");
                }
                else
                {
                    // 前面・待機
                    _animator.Play("front_idle");
                }
            }
        }
        else
        {
            if (isServing)
            {
                if (isWalking)
                {
                    // 配膳・後面・歩き
                    _animator.Play("serving_back_walk");
                }
                else
                {
                    // 配膳・後面・待機
                    _animator.Play("serving_back_idle");
                }
            }
            else
            {
                if (isWalking)
                {
                    // 後面・歩き
                    _animator.Play("back_walk");
                }
                else
                {
                    // 後面・待機
                    _animator.Play("back_idle");
                }
            }
        }

    }

    public void SetFront(bool b) => isFront = b;

    public void SetServing(bool b) => isServing = b;

    public void SetWalking(bool b) => isWalking = b;

}
