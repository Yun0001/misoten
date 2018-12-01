using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] player;

    [SerializeField]
    private GameObject pauseManager;

    private Pause pause_cs;
    
    
    // Use this for initialization
	void Awake () {

        if (!SoundController.Loadflg)
        {
            SoundController.SoundLoad();
        }
        pause_cs = pauseManager.GetComponent<Pause>();

        Sound.PlayBgm(SoundController.GetBGMName(SoundController.BGM.Gameplay));
	}

    private void FixedUpdate()
    {
        // ポーズ中でないとき
        if (!pause_cs.IsPausing())
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Player>().PlayerFixedUpdate();
            }
        }
    }

    private void Update()
    {
        // ポーズ中でないとき
        if (!pause_cs.IsPausing())
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Player>().PlayerUpdate();
            }
        }
    }
}
