using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRController : MonoBehaviour {

    private GameObject _parent;

    private Animator _animator;
    public bool isOpen = true;
    private bool openAnim = true;

    // Use this for initialization
    void Start()
    {

        //親オブジェクトを取得
        _parent = transform.root.gameObject;

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
            _animator.Play("doorr|ropen");
        }
        else
        {
            _animator.Play("doorr|rclose");
        }

    }

}
