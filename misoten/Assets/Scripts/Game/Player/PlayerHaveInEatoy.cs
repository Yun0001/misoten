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
    private readonly Vector3 adjustmentPosition = new Vector3(0.3f, 0.1f, 0);

    public bool SetEatoy(GameObject eatoy)
    {
        // 持とうとしているものがnullなら抜ける
        if (eatoy == null) return false;

        haveInEatoy = eatoy;
        return true;
    }

    public void RevocationHaveInEatoy() => Destroy(haveInEatoy);


    /// <summary>
    /// 持っているEatoyを取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetHaveInEatoy() => haveInEatoy;


    public void SetHaveInEatoyPosition()
    {
        Vector3 pos = transform.position;
        // 前向きの時
        if (GetComponent<PlayerAnimCtrl>().IsFront())
        {
            haveInEatoy.SetActive(true);
            haveInEatoy.GetComponent<SpriteRenderer>().sortingOrder = 2;
            // 右向きの時
            if (GetComponent<SpriteRenderer>().flipX)
            {
                pos.x += adjustmentPosition.x;
            }
            // 左向きの時
            else
            {
                pos.x -= adjustmentPosition.x;
            }
            pos.y += adjustmentPosition.y;
        }
        // 後向きの時
        else
        {
            haveInEatoy.SetActive(false);
        }
        haveInEatoy.transform.position = pos;
    }
}
