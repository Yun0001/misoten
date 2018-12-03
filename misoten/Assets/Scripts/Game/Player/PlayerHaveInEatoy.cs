using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHaveInEatoy : MonoBehaviour
{
    /// <summary>
    /// イートイ
    /// </summary>
    GameObject haveInEatoy;

    /// <summary>
    /// イートイセット
    /// </summary>
    /// <param name="eatoy"></param>
    /// <returns></returns>
    public bool SetEatoy(GameObject eatoy)
    {
        // 持とうとしているものがnullなら抜ける
        if (eatoy == null) return false;

        haveInEatoy = eatoy;
        if (haveInEatoy.GetComponent<Eatoy>().IsIcing())
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
            GetComponent<Player>().ChangeAttachComponent((int)Player.PlayerStatus.Normal);
        }
        else
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Catering);
            GetComponent<Player>().ChangeAttachComponent((int)Player.PlayerStatus.Normal);
        }
        GetComponent<PlayerAnimCtrl>().SetServing(true);
        return true;
    }

    /// <summary>
    /// イートイ破棄
    /// </summary>
    public void RevocationHaveInEatoy(bool b =false)
    {
        if (b)
        {
            Destroy(haveInEatoy);
        }
        else
        {
            haveInEatoy = null;
        }
    }


    /// <summary>
    /// 持っているEatoyを取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetHaveInEatoy() => haveInEatoy;

    public void SetHaveInEatoyPosition(Vector3 alienPos)
    {
        alienPos.y += 0.3f;
        alienPos.z -= 1f;
        haveInEatoy.transform.position = alienPos;
    }

    public void DisplayEatoy()
    {
        haveInEatoy.GetComponent<Eatoy>().DisplaySprite();
    }
}
