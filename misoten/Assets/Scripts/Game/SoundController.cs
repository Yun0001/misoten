using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour {

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

    private Dictionary<MenuSE, string> menuSEDictionary = new Dictionary<MenuSE, string>
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
        Entershop,
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
        MAXGameSE
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
        { GameSE.Grilled_During,"Grilled_During"},
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

    private Dictionary<ResultSE, string> resultSEDictionary = new Dictionary<ResultSE, string>
    {
        { ResultSE.applause,"applause"},
        { ResultSE.cracker_repeated,"cracker_repeated"},
        { ResultSE.drumroll,"drumroll"},
        { ResultSE.register,"register"},
    };

    


    private void Awake()
    {
        foreach (var se in Enum.GetValues(typeof(MenuSE)))
        {
            Sound.LoadSe(Enum.GetName(typeof(MenuSE), se), "Menu/" + Enum.GetName(typeof(MenuSE), se).ToString());
        }

        foreach (var se in Enum.GetValues(typeof(GameSE)))
        {
            Sound.LoadSe(Enum.GetName(typeof(GameSE), se), "Gameplay/" + Enum.GetName(typeof(GameSE), se).ToString());
        }

        foreach (var se in Enum.GetValues(typeof(ResultSE)))
        {
            Sound.LoadSe(Enum.GetName(typeof(ResultSE), se), "Result/" + Enum.GetName(typeof(ResultSE), se).ToString());
        }

    }

    public static string GetGameSEName(GameSE name)
    {
        return gameSEDictionary[name];
    }

    public void PlayGameSE(GameSE name, bool Loop = false, float Valume = 1.0f)
    {
       // Sound.SetLoopFlgSe(name, Loop,);
       // Sound.PlaySe();
    }
}
