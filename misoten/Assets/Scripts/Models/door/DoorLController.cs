using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLController : MonoBehaviour {

    private GameObject _parent;

    private Animator _animator;
    public bool isOpen = true;
    private bool openAnim = true;

    // Use this for initialization
    void Start()
    {

        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        openAnim = isOpen;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isOpen = !isOpen;
        }

        //_animation.SetBool("isOpen", isOpen);

        if (isOpen != openAnim)
        {
            DoorAnimation();
        }

    }

    void DoorAnimation()
    {

        if (isOpen)
        {
            _animator.Play("doorl|lopen");
        }
        else
        {
            _animator.Play("doorl|lclose");
        }

    }

}
