using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingMixer : MonoBehaviour {

    private Player player_cs;

	// Use this for initialization
	void Awake () {
        player_cs = GetComponent<Player>();
	}

    public void Preparation()
    {
        if (player_cs.GetHitObj((int)Player.hitObjName.Mixer) != null)
        {
            if (!player_cs.GetHitObj((int)Player.hitObjName.Mixer).GetComponent<Mixer>().Access(transform.position))
            {
                Debug.LogError("アクセス失敗");
            }
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        }
    }
}
