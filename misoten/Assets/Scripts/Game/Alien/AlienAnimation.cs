using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimation : MonoBehaviour {


    private const int ANIMATION_STATUS_PATTERN = 2;// アニメーションが必要な状態の数
    private const int ANIMATION_NUM = 4;// アニメーションの数


    [SerializeField, Range(0.0f, 100.0f)]
    private float oneAnimPatternSwitchTime;

    private float oneAnimPatternTime;

    private int countAnimTime = 0;

    private string[] folderPass = { "Textures/Alien/Wait/", "Textures/Alien/Work/" };

    private string[] waitTextureName = { "1", "2", "1", "2" };

    private string[] workTextureName = { "1", "2", "3", "4" };



    // スプライト
    public Sprite[,] sprite = new Sprite[ANIMATION_STATUS_PATTERN, ANIMATION_NUM];

    private int alienStatus = 0;
    private PlayerMove.EDirection playerUDDirection = PlayerMove.EDirection.Down;
    private int isCatering = 0;

    [SerializeField]
    private int animID = 0;

    // Use this for initialization
    void Awake()
    {
        oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

        // 待機画像ロード
        WaiitAnimationSpriteLoad();

        // 歩行画像ロード
        WorkAnimationSpriteLoad();
    }

    private void WaiitAnimationSpriteLoad()
    {
        for (int k = 0; k < 4; k++)
            sprite[0, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName[k]);
    }

    private void WorkAnimationSpriteLoad()
    {
        for (int k = 0; k < 4; k++)
            sprite[1, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName[k]);
    }

    private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[alienStatus, animID];

    public void SetAlienStatus(int alienstatus) => alienStatus = alienstatus;

    public void SetPlayerRLDirection(int direction)
    {
        if (direction == 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 0.3f;
            transform.localScale = scale;
        }
        else if (direction == 1)
        {
            Vector3 scale = transform.localScale;
            scale.x = -0.3f;
            transform.localScale = scale;
        }
    }


    void Update()
    {
        countAnimTime++;
        if (countAnimTime > oneAnimPatternTime)
        {
            countAnimTime = 0;
            animID++;
            Debug.Log(animID);
            if (animID > ANIMATION_NUM - 1) animID = 0;
            ChangeSprite();
        }
    }
}
