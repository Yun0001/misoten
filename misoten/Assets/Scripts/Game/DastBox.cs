using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DastBox : MonoBehaviour {

    [SerializeField]
    private bool isCollision = false;

    [SerializeField]
    private GameObject DastBoxUI;

    private int playerstatus;


    /// <summary>
    /// プレイヤーアクセス
    /// </summary>
    /// <param name="status"></param>
    /// <param name="pos"></param>
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
