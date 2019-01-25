using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{

    public enum EventAnnouceUI
    {
        ALL_FEVER,
        ALL_FEVER_TEX,
        MIXER,
        SOLOPLAY,
        BLUE,
        RED,
        YELLOW,
        MAX
    }



    private string folderpass = "Textures/EventAnnouce/";
    private string[] textureName = { "TextWindow_Red", "TextWindow_Blue", "TextWindow_Yellow", "Mixer_Bonus", "SoloPlay_Bonus", "All_Fever_" };
    private Sprite[] sprites = new Sprite[(int)EventAnnouceUI.MAX];
    // Use this for initialization
    void Awake () {
        for (int i = 0; i < textureName.Length; i++)
        {
            sprites[i] = Resources.Load<Sprite>(folderpass + textureName[i]);
        }
	}
	

    public void ChangeSprite(int i) => GetComponent<SpriteRenderer>().sprite = sprites[i];
    

}
