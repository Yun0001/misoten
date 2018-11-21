using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimCtrl : MonoBehaviour {

    private Animator _animator;

    [SerializeField] private bool isOver = false;
    [SerializeField] private bool isOpen = false;
    private bool isOverPage = false;
    
    void Start () {
        _animator = GetComponent<Animator>();
    }
	
	void Update () {
  
        _animator.SetBool("isOver", isOver);
        _animator.SetBool("isOpen", isOpen);

        if (isOver)
        {
            isOverPage = true;
            _animator.Play("over");
        }

    }
   

    public void OverPage() => isOver = true;
    public void OpenMenu() => isOpen = isOverPage = true;
    public void CloseMenu() => isOpen = false;
    public bool IsOpen() => isOpen;
    public bool IsOverPage() => isOverPage;

    void OverPageFinish() => isOverPage = false;
    void OffIsOver() => isOver = false;

}
