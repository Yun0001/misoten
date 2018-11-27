using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessPossiblAnnounce : MonoBehaviour {

    private Sprite[] announceUISprits;

    [SerializeField]
    private GameObject announceUI;

    [SerializeField]
    private GameObject buttonUI;


    private void Awake()
    {
        announceUISprits= Resources.LoadAll<Sprite>("Textures/AccessUI/AccessUI_Re");
    }


    public void SetSprite(int spriteID)
    {
        announceUI.GetComponent<SpriteRenderer>().sprite = announceUISprits[spriteID];
        buttonUI.SetActive(true);
    }

    public void HiddenSprite()
    {
        announceUI.GetComponent<SpriteRenderer>().sprite = null;
        buttonUI.SetActive(false);
    }



}
