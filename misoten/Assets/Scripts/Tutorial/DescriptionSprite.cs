using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionSprite : MonoBehaviour {


    [SerializeField]
    private int tutorialFlowID;


    private Sprite[] dscriptionSprites;

    private string texFolderPass = "Textures/Tutorial/";

    [SerializeField]
    private int frameCount;

    [SerializeField]
    private int  spriteID;

    private void Awake()
    {
        dscriptionSprites = LoadSprites();
        frameCount = 0;
        spriteID = 0;
        GetComponent<SpriteRenderer>().sprite = dscriptionSprites[spriteID];
    }

    private void Update()
    {
        frameCount++;
        if (frameCount >= 120)
        {
            // 次の画像に進める
            spriteID++;
            // IDが画像数を超えたら０に戻る
            if (spriteID > dscriptionSprites.Length - 1)
            {
                spriteID = 0;
            }
            // スプライト設定
            GetComponent<SpriteRenderer>().sprite = dscriptionSprites[spriteID];

            // カウントをリセット
            frameCount = 0;
        }
    }

    private Sprite[] LoadSprites()
    {
        switch (tutorialFlowID)
        {
            case 0:
                return Resources.LoadAll<Sprite>(texFolderPass+"Freezer_Txt");
            case 1:
                return Resources.LoadAll<Sprite>(texFolderPass + "Range_Tex");
            case 2:
                return Resources.LoadAll<Sprite>(texFolderPass + "Boil_Txt");
            case 3:
                return Resources.LoadAll<Sprite>(texFolderPass + "Grill_Tex");
            case 4:
                return Resources.LoadAll<Sprite>(texFolderPass + "Mix_Tex");
            case 5:
                return Resources.LoadAll<Sprite>(texFolderPass + "Gomi");
        }
        Debug.LogError("不正なスプライトID");
        return null;
    }
}
