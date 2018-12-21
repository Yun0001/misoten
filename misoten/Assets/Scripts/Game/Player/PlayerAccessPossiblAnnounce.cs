using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessPossiblAnnounce : MonoBehaviour
{

    // 定数
    private readonly static Vector3 RESET_BUTTON_UI_POSITION = new Vector3(-0.346f, 0.473f, 0.01f);
    private readonly static Vector3 BUTTON_UI_SCALE = new Vector3(0.15f, 0.15f, 0.15f);
    private readonly static Vector3 STANDBY_SPRITE_SCALE = new Vector3(0.25f, 0.25f, 0.25f);



    private Sprite[] announceUISprits;
    private Sprite mixerAccessAnnounceSprite;
    private Sprite standbySprite;
    private Sprite buttonSprite;

    [SerializeField]
    private GameObject announceUI;

    [SerializeField]
    private GameObject buttonUI;




    private void Awake()
    {
        announceUISprits= Resources.LoadAll<Sprite>("Textures/AccessUI/AccessUI_Re");
        mixerAccessAnnounceSprite = Resources.Load<Sprite>("Textures/Mixer/FuKidaShi");
        standbySprite = Resources.Load<Sprite>("Textures/Mixer/UI_Standby");
        buttonSprite = Resources.Load<Sprite>("Textures/UI_BottonB");
    }

    /// <summary>
    /// 表示するアナウンス画像をセット
    /// </summary>
    /// <param name="spriteID"></param>
    public void SetSprite(int spriteID)
    {
        ResetButtonUIPos();
        announceUI.GetComponent<SpriteRenderer>().flipX = false;
        announceUI.GetComponent<SpriteRenderer>().sprite = announceUISprits[spriteID];
        buttonUI.SetActive(true);
    }

    /// <summary>
    /// アナウンスUIを非表示
    /// </summary>
    public void HiddenSprite()
    {
        ResetButtonUIPos();
        announceUI.GetComponent<SpriteRenderer>().sprite = null;
        HiddenStandbySprite();
        buttonUI.SetActive(false);
    }

    public Sprite GetAnnounceUISprite() => announceUI.GetComponent<SpriteRenderer>().sprite;

    /// <summary>
    /// ミキサー用のUIに変更
    /// </summary>
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
        Vector3 pos = RESET_BUTTON_UI_POSITION;
        buttonUI.transform.position = annoucePos + pos;
    }

    /// <summary>
    /// 「 準備完了」表示
    /// </summary>
    public void DisplayStandbySprite()
    {
        buttonUI.GetComponent<SpriteRenderer>().sprite = standbySprite;
        buttonUI.transform.localScale = STANDBY_SPRITE_SCALE;
    }

    /// <summary>
    /// 「準備完了非表示」
    /// </summary>
    public void HiddenStandbySprite()
    {
        buttonUI.GetComponent<SpriteRenderer>().sprite = buttonSprite;
        buttonUI.transform.localScale = BUTTON_UI_SCALE;
    }
}
