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
       { {"前1" ,"前2","前1" ,"前2"}, {"後1" ,"後2","後1" ,"後2"} },
       { {"前_1" ,"前_2","前_1" ,"前_2"}, {"後_1" ,"後_2","後_1" ,"後_2"} }
    };

    private string[,,] workTextureName ={
       { { "前1","前2","前3","前4"}, { "後1","後2","後3","後4"} },
       {  { "前_1","前_2","前_3","前_4"}, { "後_1","後_2","後_3","後_4"} }
    };



    // スプライト
    public Sprite[,,,] sprite = new Sprite[ANIMATION_STATUS_PATTERN, IS_CATERING, PLAYER_DIRECTION_NUM, ANIMATION_NUM];

   // private Sprite[,,,] sprite;
    private int playerStatus = 0;
    private PlayerMove.EDirection playerUDDirection = PlayerMove.EDirection.Down;
    private PlayerMove.EDirection playerRLDirection = PlayerMove.EDirection.Right;
    private int isCatering = 0;

    [SerializeField]
    private int animID = 0;

	// Use this for initialization
	void Awake ()
    {
        oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

        // 待機画像ロード
        WaiitAnimationSpriteLoad();

        // 歩行画像ロード
        WorkAnimationSpriteLoad();
    }

    private void WaiitAnimationSpriteLoad()
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 4; k++)
                {
                    sprite[0, i, j, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName[i, j, k]);
                }
              
    }

    private void WorkAnimationSpriteLoad()
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 4; k++)
                    sprite[1, i, j, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName[i, j, k]);
    }

    private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[playerStatus, isCatering, (int)playerUDDirection, animID];

    public void SetPlayerStatus(int playerstatus) => playerStatus = playerstatus;

    public void SetPlayerUDDirection(PlayerMove.EDirection direction) => playerUDDirection = direction;

    public void SetPlayerRLDirection(PlayerMove.EDirection direction)
    {
        playerRLDirection = direction;
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
        if (catering) isCatering = 0;
        else isCatering = 1;
    }

    void Update ()
    {
        countAnimTime++;
        if (countAnimTime > oneAnimPatternTime)
        {
            countAnimTime = 0;
            animID++;
            Debug.Log(animID);
            if (animID > ANIMATION_NUM -1) animID = 0;
            ChangeSprite();
        }
	}
}
