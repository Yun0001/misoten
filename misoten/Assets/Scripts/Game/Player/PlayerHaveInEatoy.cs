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
    /// 調整座標
    /// </summary>
    private readonly Vector3 adjustmentPosition = new Vector3(0.3f, 0.5f, -0.65f);

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
        }
        else
        {
            GetComponent<Player>().SetPlayerStatus(Player.PlayerStatus.Catering);
        }
        GetComponent<PlayerAnimCtrl>().SetServing(true);
        return true;
    }

    /// <summary>
    /// イートイ破棄
    /// </summary>
    public void RevocationHaveInEatoy() => haveInEatoy = null;


    /// <summary>
    /// 持っているEatoyを取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetHaveInEatoy() => haveInEatoy;


    /// <summary>
    /// イートイ座標更新
    /// </summary>
    public void UpdateHaveInEatoyPosition()
    {
        Vector3 pos = transform.position;
        
        // 前向きの時
        if (GetComponent<PlayerAnimCtrl>().IsFront())
        {
            haveInEatoy.SetActive(true);

            if (GetComponent<SpriteRenderer>().flipX) pos.x += adjustmentPosition.x;            // 右向きの時
            else pos.x -= adjustmentPosition.x;             // 左向きの時

            //pos.y += adjustmentPosition.y;                  // y座標調整
            pos.z += adjustmentPosition.z;                  // z座標調整
        }
        else
        {
            // 後向きの時は非表示
            haveInEatoy.SetActive(false);
        }
        haveInEatoy.transform.position = pos;
    }

    public void SetHaveInEatoyPosition(Vector3 alienPos)
    {
        alienPos.y += 0.3f;
        alienPos.z -= 1f;
        haveInEatoy.transform.position = alienPos;
        
    }
}
