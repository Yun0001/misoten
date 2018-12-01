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
            if (!player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<MixerAccessPoint>().Access(gameObject))
            {
            //Debug.LogError("アクセス失敗");
            return;
            }
            player_cs.SetPlayerStatus(Player.PlayerStatus.MixerAccess);
        transform.Find("Line").gameObject.SetActive(true);
        GetComponent<PlayerAccessPossiblAnnounce>().SetMixerAccessSprite();
    }
    
}
