using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessPossiblAnnounce : MonoBehaviour {

    private Sprite[] announceUISprits;
    private Sprite mixerAccessAnnounceSprite;

    [SerializeField]
    private GameObject announceUI;

    [SerializeField]
    private GameObject buttonUI;


    private void Awake()
    {
        announceUISprits= Resources.LoadAll<Sprite>("Textures/AccessUI/AccessUI_Re");
        mixerAccessAnnounceSprite = Resources.Load<Sprite>("Textures/Mixer/FuKidaShi");
    }


    public void SetSprite(int spriteID)
    {
        ResetButtonUIPos();
        announceUI.GetComponent<SpriteRenderer>().flipX = false;
        announceUI.GetComponent<SpriteRenderer>().sprite = announceUISprits[spriteID];
        buttonUI.SetActive(true);
    }

    public void HiddenSprite()
    {
        ResetButtonUIPos();
        announceUI.GetComponent<SpriteRenderer>().sprite = null;
        buttonUI.SetActive(false);
    }

    public void SetMixerAccessSprite()
    {
        announceUI.GetComponent<SpriteRenderer>().sprite = mixerAccessAnnounceSprite;
        announceUI.GetComponent<SpriteRenderer>().flipX = true;
        buttonUI.SetActive(true);
        Vector3 pos = buttonUI.transform.position;
        Vector3 annoucePos = announceUI.transform.position;
        pos.x = annoucePos.x;
        pos.y = annoucePos.y + 0.03f;
        buttonUI.transform.position = pos;
    }

    public void ResetButtonUIPos()
    {
        Vector3 annoucePos = announceUI.transform.position;
        Vector3 pos = new Vector3(-0.346f, 0.473f, 0.01f);
        buttonUI.transform.position = annoucePos + pos;
    }



}
