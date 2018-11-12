using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DastBox : MonoBehaviour {

    /*
     * プレイヤーと当たり判定があればキー入力可能UIを表示
     * Bボタン入力で味の素UIゲージ表示
     * Bボタンを入力し続けることでゲージを進める
     * Bボタンを離すとゲージを戻す
     * ゲージがMaxでイートイを捨てる
     * イートイのスプライトを縮小させながらゴミ箱に移動させる
     * 移動後ゴミ箱のアニメーション
     * アニメーション後プレイヤーの状態を通常状態に戻す
     * */

    [SerializeField]
    private bool isCollision = false;

    [SerializeField]
    private GameObject DastBoxUI;

    private int playerstatus;

	// Use this for initialization
	void Awake ()
    {	
	}

    public void Access(int status, Vector3 pos)
    {
        SetPlayerStatus(status);
        DastBoxUI.GetComponent<DastBoxGage>().Init(pos);
        DastBoxUI.SetActive(true);
    }

    public void Action()
    {
        DastBoxUI.GetComponent<DastBoxGage>().IncreaseGage();
    }

    public void Cutting()
    {
        DastBoxUI.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!isCollision)
        {
            if (other.tag == "Player")
            {
                isCollision = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isCollision = false;
        }
    }


    public bool IsCollision() => isCollision;

    public void SetPlayerStatus(int status)
    {
        if (status != (int)Player.PlayerStatus.Catering || status != (int)Player.PlayerStatus.CateringIceEatoy)
        {
            //Debug.LogError("不正な状態");
           // return;
        }
        playerstatus = status;
    }

    public int GetPlayerStatus() => playerstatus;

    public float GetGageAmount()
    {
        return DastBoxUI.GetComponent<DastBoxGage>().GetAmount();
    }
}
