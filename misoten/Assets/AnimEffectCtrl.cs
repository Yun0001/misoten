using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffectCtrl : MonoBehaviour {

    private Animator _animator;
    private ParticleSystem _effect;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _effect = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {

        if (_animator.GetBool("isLooping"))
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
