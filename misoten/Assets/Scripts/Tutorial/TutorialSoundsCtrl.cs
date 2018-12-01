using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSoundsCtrl : MonoBehaviour {

    void Awake()
    {
        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
        Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Menu));
    }

}
