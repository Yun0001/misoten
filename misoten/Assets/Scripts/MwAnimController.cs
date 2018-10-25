using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwAnimController : MonoBehaviour {

    private Animator _animator;
    public bool isOpen=false;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Return))
        {
            isOpen = !isOpen;
        }

        _animator.SetBool("isOpen", isOpen);

    }

}
