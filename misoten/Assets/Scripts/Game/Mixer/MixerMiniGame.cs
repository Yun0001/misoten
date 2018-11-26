using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class MixerMiniGame : MonoBehaviour {
    [SerializeField]
    private GameObject mixer;


    [SerializeField]
    private GameObject mixerArrow;

    [SerializeField]
    private int powerBorder;

    [SerializeField]
    private int power = 0;

    private readonly int powerPoint=8;

    private bool rotationFlg = false;

    [SerializeField]
    GameObject timeManager;


    // Use this for initialization
    void Awake () {

    }



    void Init()
    {
        power = 0;
    }

    // Update is called once per frame
    void Update () {
        mixerArrow.GetComponent<MixerArrow>().Rotation();
}

    public bool AddPowerPoint()
    {
        power+= powerPoint;
        if (power >= powerBorder && !rotationFlg)
        {
            ChangeRotationflg();
            return true;
        }
        return false;
    }

    public void ChangeRotationflg()
    {
        rotationFlg = true;
        mixerArrow.GetComponent<MixerArrow>().ReverseDirection();

    }


    public bool GetRotation() => rotationFlg;

    public int GetPowerPoint() => power;
}
