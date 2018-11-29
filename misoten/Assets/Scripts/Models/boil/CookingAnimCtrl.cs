using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingAnimCtrl : MonoBehaviour {

    private bool _isCooking = false;
    private Animator _animator;
    private PauseAnimation _pause;

	void Start () {

        _animator = this.GetComponent<Animator>();
        _pause = this.GetComponent<PauseAnimation>();

    }

	void Update () {

        if (_isCooking)
        {
            PlayLoopAction();
            _pause.SetIsPause(false);
        }
        else
        {
            _pause.SetIsPause(true);
        }

    }

    void PlayLoopAction() => _animator.Play("loopAction");

    public void SetIsCooking(bool b) => _isCooking = b;

}
