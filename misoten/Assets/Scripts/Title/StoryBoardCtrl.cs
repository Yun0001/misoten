using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBoardCtrl : MonoBehaviour {

    private bool isCompleteStoryAnimation = false;
    private Animator _animator;
    private GameObject _canvasWIO;

    private float _speed;

    void Start() {
        _animator = this.GetComponent<Animator>();
        _canvasWIO = GameObject.Find("WIO");

        _speed = _animator.speed;
    }

    void Update() {
    }

    void CompleteStoryAnimation()
    {
        _canvasWIO.GetComponent<WhiteIO>().OnSeWhiteIn();
    }

    public bool GetIsCompleteStartAnimation() => isCompleteStoryAnimation;

    public void SetIsStartStory(bool b) => _animator.SetBool("isStartStory", b);

    public void SetEnabled(bool b) => this.GetComponent<SpriteRenderer>().enabled = b;

    public void OnCompleteStoryAnimation() => isCompleteStoryAnimation = true;

    public void AddSpeed(float x) => _animator.speed = _speed * x;


}
