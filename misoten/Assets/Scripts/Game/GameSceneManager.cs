using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour {


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
    string FolderPass = "GamePlay/";
    string[] FolderName = { "AlienSounds/", "DastBoxSounds/", "FlyingpanSounds/", "IceBoxSounds/", "MicrowaveSounds/", "MixerSounds/", "PotSounds/" };
	// Use this for initialization
	void Awake () {
        for (int i = 0; i < seKey.Length; i++)
        {
            if (i < 7) Sound.LoadSe(seKey[i], FolderPass + FolderName[0] + seKey[i]);
            else if (i < 8) Sound.LoadSe(seKey[i], FolderPass + FolderName[1] + seKey[i]);
            else if (i < 11) Sound.LoadSe(seKey[i], FolderPass + FolderName[2] + seKey[i]);
            else if (i < 15) Sound.LoadSe(seKey[i], FolderPass + FolderName[3] + seKey[i]);
            else if (i < 20) Sound.LoadSe(seKey[i], FolderPass + FolderName[4] + seKey[i]);
            else if (i < 22) Sound.LoadSe(seKey[i], FolderPass + FolderName[5] + seKey[i]);
            else if (i < 23) Sound.LoadSe(seKey[i], FolderPass + FolderName[6] + seKey[i]);
            else if (i < 34) Sound.LoadSe(seKey[i], FolderPass + seKey[i]);
        }
	}

}
