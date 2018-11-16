using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimCtrl : MonoBehaviour {

    private Animator _animator;

    [SerializeField] private bool isOver = false;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool isLooping = false;
    
    void Start () {
        _animator = GetComponent<Animator>();
    }
	
	void Update () {
  
        _animator.SetBool("isOver", isOver);
        _animator.SetBool("isOpen", isOpen);
        _animator.SetBool("isLooping", isLooping);

        if (isOver)
        {
            _animator.Play("over");
        }

    }

    void OffIsOver()
    {
        isOver = false;
    }

    public void OverPage() => isOver = true;
    public void OpenMenu() => isOpen = true;
    public void CloseMenu() => isOpen = false;

}
