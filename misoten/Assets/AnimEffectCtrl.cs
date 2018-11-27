﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffectCtrl : MonoBehaviour {

    private Animator _animator;
    private ParticleSystem _effect;

    private GameObject _obj;

    [SerializeField] private GameObject _effectObj;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();

        _obj = Instantiate(_effectObj, this.transform);
        _effect = _obj.GetComponent<ParticleSystem>();
        _effect.Stop();

    }
	
	// Update is called once per frame
	void Update () {

        if (_animator.GetBool("isOpen"))
        {
            if (!_effect.isPlaying)
            {
                _effect.Play();
            }
        }
        else
        {
            if (_effect.isPlaying)
            {
                _effect.Stop();
            }
        }

    }
}
