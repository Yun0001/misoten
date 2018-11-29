using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplateEatoyAnnounce : MonoBehaviour {

    [SerializeField]
    private GameObject eatoy;

    public  void SetSprite(Sprite sp = null)
    {
        eatoy.GetComponent<SpriteRenderer>().sprite = sp;
    }
}
