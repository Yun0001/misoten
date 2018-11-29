using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour {

    private Animator _animator;
    private float _animSpeed;

    [SerializeField] private bool isFront =true;
    [SerializeField] private bool isServing = false;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isPause = false;

    void Start () {
        _animator = GetComponent<Animator>();
        _animSpeed = _animator.speed;
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

        // Pause
        if (isPause)
        {
            PauseAnimation();
        }
        else
        {
            _animator.speed = _animSpeed;
        }

    }

    void PauseAnimation()
    {
        if (_animator.speed != 0)
        {
            _animSpeed = _animator.speed;
            _animator.speed = 0;
        }
    }

    public void SetFront(bool b) => isFront = b;

    public void SetServing(bool b) => isServing = b;

    public void SetWalking(bool b) => isWalking = b;

    public void SetPause(bool b) => isPause = b;

    public bool IsFront() => isFront;

    public bool IsServing() => isServing;

}
