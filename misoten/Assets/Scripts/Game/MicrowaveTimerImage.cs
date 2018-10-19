using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveTimerImage : MonoBehaviour
{

    public enum EMicrowaveTimerTex
    {
        Start,
        ThreeSeconds,
        TwoSeconds,
        DoNotKnow,
        Success,
        Failure,
        Explosion,
        Max
    }

    private Sprite[] textures=new Sprite[(int)EMicrowaveTimerTex.Max];

    private string[] texturePass = {
        "Timer_Go_Prototype",
        "Timer_3_Prototype",
        "Timer_2_Prototype",
        "Timer_Who_Prototype",
        "Timer_OK_Prototype",
        "Timer_NG_Prototype",
        "Timer_Explosion_Prototype"
    };

    EMicrowaveTimerTex microwaveTimerTex;
    [SerializeField]
    private GameObject timerSprite;

    private void Awake()
    {

        for (int i = 0; i < textures.Length; i++)
        {
            textures[i] = Resources.Load<Sprite>("Textures/Microwave/" + texturePass[i]);
        }
        Vector3 pos = transform.position;
        pos.x += 3;
        transform.position = pos;
        gameObject.SetActive(false);
    }

    // Use this for initialization
    public void Init ()
    {
        gameObject.SetActive(true);
        microwaveTimerTex = EMicrowaveTimerTex.Start;
        timerSprite.GetComponent<SpriteRenderer>().sprite = textures[(int)microwaveTimerTex];
    }

    public void UnInit()
    {
        gameObject.SetActive(false);
    }
	
	public void ChangeSprite ()
    {
        microwaveTimerTex++;
        if (microwaveTimerTex >= EMicrowaveTimerTex.Max) return;
        timerSprite.GetComponent<SpriteRenderer>().sprite = textures[(int)microwaveTimerTex];
    }

    public void ChangeSprite(EMicrowaveTimerTex texid)
    {
        microwaveTimerTex = texid;
        timerSprite.GetComponent<SpriteRenderer>().sprite = textures[(int)microwaveTimerTex];
    }
}
