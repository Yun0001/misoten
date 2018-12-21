using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject pauseManager;
    private Pause pause_cs;

    [SerializeField]
    private GameObject timeManager;
    private GameTimeManager timeManager_cs;




    // Use this for initialization
    void Awake()
    {
        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
        pause_cs = pauseManager.GetComponent<Pause>();
        timeManager_cs = timeManager.GetComponent<GameTimeManager>();

        Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Gameplay));
    }

    private void FixedUpdate()
    {
        // タイムアップならreturn
        if (timeManager_cs.IsTimeUp()) return;
        // ポーズ中ならreturn
        if (pause_cs.IsPausing()) return;


        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().PlayerFixedUpdate();
        }
    }

    private void Update()
    {
        // タイムアップならreturn
        if (timeManager_cs.IsTimeUp())
        {
            foreach (var player in players)
            {
                player.GetComponent<Player>().StopMove();
            }
            return;
        } 
        // ポーズ中ならreturn
        if (pause_cs.IsPausing()) return;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().PlayerUpdate();
        }
    }

}
