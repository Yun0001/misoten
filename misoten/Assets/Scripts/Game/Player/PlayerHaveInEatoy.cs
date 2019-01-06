using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHaveInEatoy : MonoBehaviour
{
    /// <summary>
    /// イートイ
    /// </summary>
    [SerializeField]
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

        // イートイセット
        haveInEatoy = eatoy;

        // イートイの状態によってプレイヤーの状態を設定
        if (haveInEatoy.GetComponent<Eatoy>().IsIcing())
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.CateringIceEatoy);
        }
        else
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Catering);
        }
        GetComponent<Player>().ChangeAttachComponent((int)Player.PlayerStatus.Normal);
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

    public void OfferEatoy(Vector3 alienPos)
    {
        // イートイの座標をエイリアンの前に設定
        SetHaveInEatoyPosition(alienPos);

        // イートイ表示
        DisplayEatoy();

        // 手放す
        RevocationHaveInEatoy();
    }

    /// <summary>
    /// イートイの座標をエイリアンの前に設定
    /// </summary>
    /// <param name="alienPos"></param>
    private void SetHaveInEatoyPosition(Vector3 alienPos)
    {
        alienPos.y += 0.3f;
        alienPos.z -= 1f;
        haveInEatoy.transform.position = alienPos;
    }

    /// <summary>
    /// イートイ表示
    /// </summary>
    private void DisplayEatoy() => haveInEatoy.GetComponent<Eatoy>().DisplaySprite();
}
