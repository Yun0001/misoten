using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnnounceUI : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    readonly float maxPosx = 4.2f;
    readonly float limitUIPos =4.8f;
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.position;
        Vector3 pPos = player.transform.position;
        if (player.transform.position.x > maxPosx)
        {
            pos.x = limitUIPos;
            transform.position = pos;
        }
        else
        {
            pPos.x += 0.6f;
            pPos.y += 0.8f;
            transform.position = pPos;

        }
	}
}

