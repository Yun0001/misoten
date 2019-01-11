using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GamepadInput;
using System.Linq;

public class TutorialPlayer : MonoBehaviour {

    [SerializeField] private bool isComplete = false;
    [SerializeField] private bool isSkip = false;

    public bool IsComplete() => isComplete;
    public bool IsSkip() => isSkip;
    public void OnComplete() => isComplete = true;
    public void UnComplete() => isComplete = false;
    public void SetPlayerReder(bool b) => GetComponent<SpriteRenderer>().enabled = b;


}
