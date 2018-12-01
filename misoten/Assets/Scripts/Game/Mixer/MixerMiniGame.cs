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

    private bool SEflg;


    // Use this for initialization
    void Awake () {

    }



    public void Init()
    {
        power = 0;
        rotationFlg = false;
        mixerArrow.GetComponent<MixerArrow>().Init();
        SEflg = false;
    }

    // Update is called once per frame
    void Update () {
        mixerArrow.GetComponent<MixerArrow>().Rotation();
}

    public bool AddPowerPoint()
    {
        if (!SEflg)
        {
            Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Mixer), true, 20);
            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Mixer),20);
            SEflg = true;
        }
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

    public void CoinStopSound()
    {
        Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Mixer), false, 20);
        Sound.StopSe(SoundController.GetGameSEName(SoundController.GameSE.Mixer), 20);
    }
}
