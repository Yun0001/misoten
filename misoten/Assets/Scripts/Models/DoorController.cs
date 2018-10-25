using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Animator doorRAnimator;
    private Animator doorLAnimator;
    public bool isOpen = false;
    private bool openAnim = false;

    // Use this for initialization
    void Start () {

        doorRAnimator = GameObject.Find("doorr").GetComponent<Animator>();
        doorLAnimator = GameObject.Find("doorl").GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {

        openAnim = isOpen;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            isOpen = !isOpen;
        }

        //_animation.SetBool("isOpen", isOpen);

        if (isOpen!=openAnim)
        {
            DoorAnimation();
        }

    }

    void DoorAnimation()
    {

        if (isOpen) {
            doorRAnimator.Play("doorr|ropen");
            doorLAnimator.Play("doorl|lopen");
        }
        else
        {
            doorRAnimator.Play("doorr|rclose");
            doorLAnimator.Play("doorl|lclose");
        }


    }

}
