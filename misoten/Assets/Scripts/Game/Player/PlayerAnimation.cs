using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private const int ANIMATION_STATUS_PATTERN = 2;// アニメーションが必要な状態の数
    private const int IS_CATERING = 2;// 通常状態と配膳中
    private const int PLAYER_DIRECTION_NUM = 2;  // プレイヤーの向きの数
    private const int ANIMATION_NUM = 4;// アニメーションの数


    [SerializeField,Range(0.0f,100.0f)]
    private float oneAnimPatternSwitchTime;

    private float oneAnimPatternTime;

    private int countAnimTime = 0;

    private string[] folderPass = { "Textures/Player/Wait/", "Textures/Player/Work/" };

    private string[,,] waitTextureName = {
       { {"front1" ,"front2","front1" ,"front2"}, {"behind1" ,"behind2","behind1" ,"behind2"} },
       { {"h_front1" ,"h_front2","h_front1" ,"h_front2"}, {"h_behind1" ,"h_behind2","h_behind1" ,"h_behind2"} }
    };

    private string[,,] workTextureName ={
       { { "front1","front2","front3","front4"}, { "behind1","behind2","behind3","behind4"} },
       {  { "h_front1","h_front2","h_front3","h_front4"}, { "h_behind1","h_behind2","h_behind3","h_behind4"} }
    };



    // スプライト
    public Sprite[,,,] sprite = new Sprite[ANIMATION_STATUS_PATTERN, IS_CATERING, PLAYER_DIRECTION_NUM, ANIMATION_NUM];

    private int playerStatus = 0;
    private PlayerMove.EDirection playerUDDirection = PlayerMove.EDirection.Down;
    private int isCatering = 0;

    [SerializeField]
    private int animID = 0;

	// Use this for initialization
	void Awake ()
    {
        oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

        // 待機画像ロード
        WaitAnimationSpriteLoad();

        // 歩行画像ロード
        WorkAnimationSpriteLoad();
    }

    /// <summary>
    /// 待機画像ロード
    /// </summary>
    private void WaitAnimationSpriteLoad()
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 4; k++)
                    sprite[0, i, j, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName[i, j, k]);
    }

    /// <summary>
    /// 歩行画像ロード
    /// </summary>
    private void WorkAnimationSpriteLoad()
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 4; k++)
                    sprite[1, i, j, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName[i, j, k]);
    }

    private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[playerStatus, isCatering, (int)playerUDDirection, animID];

    public void SetPlayerStatus(int playerstatus)
    {
        if (playerStatus != playerstatus)
        {
            playerStatus = playerstatus;
            countAnimTime = 0;
            animID = 0;
            ChangeSprite();
        }
    }

    public void SetPlayerUDDirection(PlayerMove.EDirection direction)
    {
        if (playerUDDirection != direction)
        {
            playerUDDirection = direction;
            countAnimTime = 0;
            animID = 0;
            ChangeSprite();
        }
    } 

    public void SetPlayerRLDirection(PlayerMove.EDirection direction)
    {
        if (direction == PlayerMove.EDirection.Right)
        {
            Vector3 scale = transform.localScale;
            scale.x = 0.3f;
            transform.localScale = scale;
        }
        else if (direction == PlayerMove.EDirection.Left)
        {
            Vector3 scale = transform.localScale;
            scale.x = -0.3f;
            transform.localScale = scale;
        }
    } 


    public void SetIsCatering(bool catering)
    {
        if (catering) isCatering = 1;
        else isCatering = 0;
    }

    void Update ()
    {
        countAnimTime++;
        if (countAnimTime > oneAnimPatternTime)
        {
            countAnimTime = 0;
            animID++;
            if (animID > ANIMATION_NUM -1) animID = 0;
            ChangeSprite();
        }
	}
}
