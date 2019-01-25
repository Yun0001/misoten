using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mixerAnimCtrl : MonoBehaviour
{

    private Animator _animator;
    
    private bool isPlayAnimation = false;
    private bool isAnimationMoment = false;     // アニメーションの始まった瞬間、終わった瞬間用のフラグ

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
        _animator.SetBool("isLooping", isLooping);
        _animator.SetBool("isOpen", isOpen);
        _animator.SetBool("isComplete", isComplete);

        if (GetAnimationMoment()) {
        //    Debug.Log(true);
        }

        // 現在のアニメーションステートを取得
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        // "open"のときの処理
        if (stateInfo.IsName("Base Layer.open"))
        {

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

    void OnIsOpen()
    {
        isOpen = true;
    }

    private void OnAnimationPlay()
    {
        isAnimationMoment = true;
        isPlayAnimation = true;
    }

    private void OnAnimationFinish()
    {
        isAnimationMoment = true;
        isPlayAnimation = false;
    }

    public bool GetAnimationMoment()
    {
        if (isAnimationMoment&&(!isPlayAnimation))
        {
            isAnimationMoment = false;
            return true;
        }

        return false;
    }

    public bool GetPlayAnimation() => isPlayAnimation;

    public void SetBool(bool b) => isLooping = b;
    public bool GetBool() => isLooping;


    public void SetIsOpen(bool flg) => isOpen = flg;
    public bool GetIsOpen() => isOpen;

    public void SetIsComplete(bool b) => isComplete = b;

}
