using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    static public string[] seKey = {
        "Eat", "Entershop", "Failure_share","Leave","Offer_success","Run","Walk",//6
        "Dustshoot",//7
        "Grilled_High","Grilled_During","Grilled_Low",//10
        "Icebreak","Refrigeratorclose","Refrigeratoropen","RefrigeratorSuccess",//14
        "Microwave","MicrowaveOpen","MicrowaveStartup","Microwavetimer","Stirfry",//19
        "Mixer","Mixerend",//21
        "Boil",//22
        "Addcoins","Countdown","Endannouncement","Fire","Getangry","Metal01","Metal02","Open","ShutterClose","Success_share"//33
    };

	static public string[] seKey2 = {
		"Resulit_cracker_repeated", "Resulit_drumroll"
	};

	string FolderPass = "GamePlay/";//Result
	string FolderPass2 = "Result/";

	string[] FolderName = { "AlienSounds/", "DastBoxSounds/", "FlyingpanSounds/", "IceBoxSounds/", "MicrowaveSounds/", "MixerSounds/", "PotSounds/" };
	// Use this for initialization
	void Awake () {

        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }

        //Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Gameplay));
	}
}
