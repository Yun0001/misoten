using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveTimerImage : MonoBehaviour
{ 
    public enum EMicrowaveTimerSprite
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

    private EMicrowaveTimerSprite spriteID;

    /// <summary>
    /// レンチンで使用する各テクスチャ
    /// </summary>
    private Sprite[] textures=new Sprite[(int)EMicrowaveTimerSprite.Max];

    private string folderPass = "Textures/Microwave/";

    /// <summary>
    /// テクスチャファイル名
    /// 順番はenumと同じにする
    /// </summary>
    private string[] textureName = 
    {
        "Timer_Go_Prototype",
        "Timer_3_Prototype",
        "Timer_2_Prototype",
        "Timer_Who_Prototype",
        "Timer_OK_Prototype",
        "Timer_NG_Prototype",
        "Timer_Explosion_Prototype"
    };

   // [SerializeField]
   // private GameObject timerSprite;

    /// <summary>
    /// 初期処理
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < textures.Length; i++)
            textures[i] = Resources.Load<Sprite>(folderPass + textureName[i]);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// タイマーを画面に表示
    /// </summary>
    public void Display(Vector3 pPos)
    {
        gameObject.SetActive(true);
        SetPosition(pPos);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;        //手前に描画する
        spriteID = EMicrowaveTimerSprite.Start;
        ChangeSprite();
    }

    /// <summary>
    /// タイマー非表示
    /// </summary>
    public void HiddenTimer() => gameObject.SetActive(false);
   
	
    /// <summary>
    /// 次のスプライトに切り替え
    /// </summary>
	public void SetNextSprite ()
    {
        spriteID++;
        if (spriteID >= EMicrowaveTimerSprite.Max)
        {
            Debug.LogError("不正なspriteID");
            return;
        } 
        ChangeSprite();
    }

    /// <summary>
    /// 指定したスプライトに切り替え
    /// </summary>
    /// <param name="texid"></param>
    public void SetSprite(EMicrowaveTimerSprite texid)
    {
        spriteID = texid;
        ChangeSprite();
    }

    /// <summary>
    /// スプライト切り替え
    /// </summary>
    private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = textures[(int)spriteID];

    private void SetPosition(Vector3 pPos)
    {
        Vector3 pos = pPos;
        pos.y += 1.5f;
        transform.position = pos;
    }
}
