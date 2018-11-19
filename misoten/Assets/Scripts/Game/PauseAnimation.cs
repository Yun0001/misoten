using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour {

    private Animator _animator;
    [SerializeField] private bool isPause = false;
    private float _animSpeed;

	void Start () {
        _animator = GetComponent<Animator>();
        _animSpeed = _animator.speed;
    }

    void Update()
    {

        if (isPause)
        {
            Pause();
        }
        else
        {
            // ポーズ中（animator.speed==0）以外はアニメーションスピードを更新する
            if (_animator.speed != 0)
            {
                _animSpeed = _animator.speed;
            }
            _animator.speed = _animSpeed;
        }

    }

    void Pause()
    {
        if (_animator.speed!=0)
        {
            _animSpeed = _animator.speed;
            _animator.speed = 0;
        }
    }

    public void SetIsPause(bool b) => isPause = b;

}
