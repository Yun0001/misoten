using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour {

    public enum BGM
    {
        Title,
        Menu,
        Gameplay,
        Result
    }

    private static Dictionary<BGM, string> BGMDictionary = new Dictionary<BGM, string>
    {
            { BGM.Title,"Title"},
                { BGM.Menu,"Gamemenu02"},
                    { BGM.Gameplay,"Gameplay01"},
                        { BGM.Result,"Gameresult01"},
    };



    public enum MenuSE
    {
        Fadeout,
        Cancelkey_share,
        decisionkey_share,
        flicker,
        open,
        textslide_share,
        tutorial_failure,
        tutorial_success
    }

    private static Dictionary<MenuSE, string> menuSEDictionary = new Dictionary<MenuSE, string>
    {
        { MenuSE.Fadeout,"Fadeout"},
        { MenuSE.Cancelkey_share,"Cancelkey_share"},
        { MenuSE.decisionkey_share,"decisionkey_share"},
        { MenuSE.flicker,"flicker"},
        { MenuSE.open,"open"},
        { MenuSE.textslide_share,"textslide_share"},
        { MenuSE.tutorial_failure,"tutorial_failure"},
        { MenuSE.tutorial_success,"tutorial_success"},
    };

    // Sounds/GamePlay/
    public enum GameSE
    {
        Addcoins,
        Fire,
        Success_share,
        // Alien/
        Offer_succes,
        Getangry,
        Eat_success,
        Eat_Getangry,
        // Boiling/
        Boil,
        // EnterShop/
        Open,
        Bell,
        // Gameend/
        Countdown,
        Endannouncement,
        // Grilled/
        Grilled_During,
        Stirfry,
        // Lentin/
        Microwave,
        MicrowaveOpen,
        MicrowaveStartup,
        // Mixer/
        Mixer,
        Mixerend,
        // Refrigerator/
        Icebreak,
        Refrigeratorclose,
        Refrigeratoropen,
        RefrigeratorSuccess,
        // Refusebox/
        Dustshoot,
    }

    private static Dictionary<GameSE, string> gameSEDictionary = new Dictionary<GameSE, string>
    {
        { GameSE.Addcoins,"Addcoins"},
        { GameSE.Fire,"Fire"},
        { GameSE.Success_share,"Success_share"},
        { GameSE.Offer_succes,"Offer_succes"},
        { GameSE.Getangry,"Getangry"},
        { GameSE.Eat_success,"Eat_success"},
        { GameSE.Eat_Getangry,"Eat_Getangry"},
        { GameSE.Boil,"Boil"},
        { GameSE.Open,"Open"},
        { GameSE.Bell,"Bell"},
        { GameSE.Countdown,"Countdown"},
        { GameSE.Endannouncement,"Endannouncement"},
        { GameSE.Grilled_During,"Grilled_ During"},
        { GameSE.Stirfry,"Stirfry"},
        { GameSE.Microwave,"Microwave"},
        { GameSE.MicrowaveOpen,"MicrowaveOpen"},
        { GameSE.MicrowaveStartup,"MicrowaveStartup"},
        { GameSE.Mixer,"Mixer"},
        { GameSE.Mixerend,"Mixerend"},
        { GameSE.Icebreak,"Icebreak"},
        { GameSE.Refrigeratorclose,"Refrigeratorclose"},
        { GameSE.Refrigeratoropen,"Refrigeratoropen"},
        { GameSE.RefrigeratorSuccess,"RefrigeratorSuccess"},
        { GameSE.Dustshoot,"Dustshoot"},
    };

    public enum ResultSE
    {
        applause,
        cracker_repeated,
        drumroll,
        register
    }

    private static Dictionary<ResultSE, string> resultSEDictionary = new Dictionary<ResultSE, string>
    {
        { ResultSE.applause,"applause"},
        { ResultSE.cracker_repeated,"cracker_repeated"},
        { ResultSE.drumroll,"drumroll"},
        { ResultSE.register,"register"},
    };


    public static bool Loadflg = false;

    public static void SoundLoad()
    {
        foreach (var bgm in Enum.GetValues(typeof(BGM)))
        {
            string fileName = GetBGMName((BGM)Enum.ToObject(typeof(BGM), bgm));
            Sound.LoadBgm(fileName, "BGM/" + fileName);
        }

        foreach (var se in Enum.GetValues(typeof(MenuSE)))
        {
            string fileName = GetMenuSEName((MenuSE)Enum.ToObject(typeof(MenuSE), se));
            Sound.LoadSe(fileName, "Menu/" + fileName);
        }

        foreach (var se in Enum.GetValues(typeof(GameSE)))
        {
            string fileName = GetGameSEName((GameSE)Enum.ToObject(typeof(GameSE), se));
            Sound.LoadSe(fileName, "Gameplay/" + fileName);
        }


        foreach (var se in Enum.GetValues(typeof(ResultSE)))
        {
            string fileName = GetResultSEName((ResultSE)Enum.ToObject(typeof(ResultSE), se));
            Sound.LoadSe(fileName, "Result/" + fileName);
        }

        Loadflg = true;
    }

    public static string GetMenuSEName(MenuSE name)
    {
        return menuSEDictionary[name];
    }

    public static string GetGameSEName(GameSE name)
    {
        return gameSEDictionary[name];
    }
    public static string GetResultSEName(ResultSE name)
    {
        return resultSEDictionary[name];
    }

    public static string GetBGMName(BGM name)
    {
        return BGMDictionary[name];
    }

    public static bool GetIsPlaySE(int Channel)
    {
        return Sound.GetInstance().GetAudioSource(Channel).isPlaying;
    }

    public static void StopAllSE()
    {
        for (int i = 0; i < Sound.GetInstance().GetChannelNum(); i++)
        {
            if (GetIsPlaySE(i))
            {
                Sound.GetInstance().GetAudioSource(i).Stop();
            }
        }
    }
}
