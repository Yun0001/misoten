using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCollision : MonoBehaviour
{

    public enum hitObjName
    {
        Microwave, //レンジ
        Pot,//鍋
        GrilledTable,//焼き台
        Mixer,
        IceBox,
        DastBox,
        Alien,//宇宙人
        HitObjMax
    }

    private Player player_cs; 

    [SerializeField]
    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 7).ToArray();// 現在プレイヤーと当たっているオブジェクト

    private void Awake()
    {
        player_cs = GetComponent<Player>();
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            // レンジ
            case "Microwave":
                hitObj[(int)hitObjName.Microwave] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.Microwave);
                }
                break;
            // 鍋
            case "Pot":
                hitObj[(int)hitObjName.Pot] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.Pot);
                }
                break;
            // 焼き台
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.GrilledTable);
                }
                break;
            // ミキサー
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.Mixer);
                }
                break;

            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Normal)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.IceBox);
                }
                break;

            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = collision.gameObject;
                if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering ||
                    player_cs.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
                {
                    player_cs.SetAnnounceSprite((int)hitObjName.DastBox);
                }
                break;
            case "Alien":
                hitObj[(int)hitObjName.Alien] = collision.gameObject;
                break;
        }
    }

    /// <summary>
    /// 当たり判定がなくなるとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Microwave":
                hitObj[(int)hitObjName.Microwave] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "Pot":
                hitObj[(int)hitObjName.Pot] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "Fryingpan":
                hitObj[(int)hitObjName.GrilledTable] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "Mixer":
                hitObj[(int)hitObjName.Mixer] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "IceBox":
                hitObj[(int)hitObjName.IceBox] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "DastBox":
                hitObj[(int)hitObjName.DastBox] = null;
                player_cs.HiddenAnnounceSprite();
                break;
            case "Alien":
                hitObj[(int)hitObjName.Alien] = null;
                break;
        }
    }

    public GameObject GetHitObj(hitObjName HitObjID)
    {
        if (hitObj[(int)HitObjID] == null) return null;

        return hitObj[(int)HitObjID].gameObject;
    }
}
