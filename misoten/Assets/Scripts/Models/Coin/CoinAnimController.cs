using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimController : MonoBehaviour {

    private Animator _animator;
    private bool isSpin = true;
    public bool isLooping = false;
    private bool isAnimTrig = true;

    // Use this for initialization
    void Start()
    {

        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        isAnimTrig = isSpin;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isSpin = !isSpin;
        }

        if (isSpin != isAnimTrig)
        {
            ModelAnimation();
        }

        _animator.SetBool("isLooping", isLooping);

    }

    void ModelAnimation()
    {
        _animator.Play("spin");
    }

}
