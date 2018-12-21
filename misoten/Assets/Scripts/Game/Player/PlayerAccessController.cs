using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessController : MonoBehaviour
{

    public enum AccessObjectName
    {
        Microwave, //レンジ
        Pot,//鍋
        Flyingpan,//焼き台
        Mixer,
        IceBox,
        DastBox,
        Alien,
        None
    }

    private Player player_cs;

    private AccessObjectName[] accessObjName = {
        AccessObjectName .Microwave,
        AccessObjectName.Pot,
        AccessObjectName .Flyingpan,
        AccessObjectName.Mixer,
        AccessObjectName.IceBox,
        AccessObjectName.DastBox,
        AccessObjectName.Alien
    };

    private void Awake()
    {
        player_cs = GetComponent<Player>();
    }

    public bool IsAccessPossible(AccessObjectName acccessObjectName)
    {
        switch (acccessObjectName)
        {
            case AccessObjectName.Microwave:    return IsMicrowaveAccessPossible();
            case AccessObjectName.Pot:               return IsPotAccessPossible();
            case AccessObjectName.Flyingpan:      return IsFlyingpanAccessPossible();
            case AccessObjectName.Mixer:            return IsMixerAccessPossible();
            case AccessObjectName.IceBox:          return IsIceBoxAccessPossible();
            case AccessObjectName.DastBox:       return IsDastBoxAccessPossible();
            case AccessObjectName.Alien:            return IsAlienAccessPossible();
            default:                                             return false;
        }
    }

    /// <summary>
    /// 電子レンジアクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsMicrowaveAccessPossible()
    {
        // 電子レンジに当たっていなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Microwave) == null) return false;

        // アイスイートイを持っていなければfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.CateringIceEatoy) return false;

        return true;
    }

    /// <summary>
    /// 鍋アクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsPotAccessPossible()
    {
        // 鍋に当たっていなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Pot) == null) return false;

        // アイスイートイを持っていなければfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.CateringIceEatoy) return false;

        return true;
    }

    /// <summary>
    /// フライパンアクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsFlyingpanAccessPossible()
    {
        // フライパンに当たっていなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.GrilledTable) == null) return false;

        // アイスイートイを持っていなければfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.CateringIceEatoy) return false;

        return true;
    }

    /// <summary>
    /// ミキサーアクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsMixerAccessPossible()
    {
        // ミキサーに当たっていなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer) == null) return false;

        // イートイを持っていなければfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.Catering) return false;

        // チェンジイートイを持っている場合はfalse
        int colorID = (int)GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy().GetComponent<Eatoy>().GetEatoyColor();
        if (colorID != 0 && colorID % 2 == 0) return false;

        // ミキサーの状態が3人アクセスより進んでいればfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Mixer).GetComponent<Mixer>().GetStatus() >= Mixer.Status.AccessThree) return false;

        return true;
    }

    /// <summary>
    /// アイスボックスアクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsIceBoxAccessPossible()
    {
        // 通常状態以外ならfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.Normal) return false;

        // 冷蔵庫に当たっていなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.IceBox) == null) return false;

        return true;
    }

    /// <summary>
    /// ダストボックスアクセス判定
    /// </summary>
    /// <returns></returns>
    private bool IsDastBoxAccessPossible()
    {
        // アイスイートイまたはイートイを持っていなければfalse
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.Catering && 
            player_cs.GetPlayerStatus() != Player.PlayerStatus.CateringIceEatoy) return false;

        //ダストボックスとの当たり判定がなければfalse
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.DastBox) == null) return false;

        return true;
    }

    private bool IsAlienAccessPossible()
    {
        if (GetComponent<PlayerHaveInEatoy>().GetHaveInEatoy() == null) return false;                             // 料理を持っていないならreturn
        if (player_cs.GetPlayerStatus() != Player.PlayerStatus.Catering) return false;          // 配膳状態でないならreturn
        if (player_cs.IsObjectCollision(PlayerCollision.hitObjName.Alien) == null) return false;    // 宇宙人との当たり判定がなければreturn

        return true;
    }

    public AccessObjectName[] GetAccessObjectNameArray() => accessObjName;
}
