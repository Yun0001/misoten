using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerAccessPoint : MonoBehaviour
{
    private Mixer mixer;

    private void Awake()
    {
        mixer = GetComponent<Mixer>();
    }

    /// <summary>
    /// プレイヤーアクセス
    /// </summary>
    /// <param name="accessPos"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool Access(GameObject player)
    {
        // アクセスポイント判定
        if (!DecisionAccessPoint(player.transform.position))
        {
            Debug.LogError("アクセスポイントがおかしい");
            return false;
        }

        if (!mixer.Access(player)) return false;

        return true;
    }

    
    public bool AccessDiscconnection(Vector3 playerPos)
    {
        if (!DecisionAccessPoint(playerPos)) return false;

        mixer.ReturnStatus();
        return true;
    }
    

    /// <summary>
    /// アクセスポイント判定
    /// </summary>
    /// <param name="accesspos"></param>
    /// <returns></returns>
    public bool DecisionAccessPoint(Vector3 accesspos)
    {
        float border = transform.position.z - 0.5f;

        if (accesspos.z < border)
        {
            ChangeBoxColliderEnable(0);
            return true;
        }
        else if (accesspos.x > transform.position.x)
        {
            ChangeBoxColliderEnable(1);
            return true;
        }
        else if (accesspos.x < transform.position.x)
        {
            ChangeBoxColliderEnable(2);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 当たり判定のOnOffを切り替え
    /// </summary>
    /// <param name="element"></param>
    private void ChangeBoxColliderEnable(int element)
    {
        BoxCollider[] bc = GetComponents<BoxCollider>();
        bc[element].enabled = !bc[element].enabled;
    }

    /// <summary>
    /// 全てのアクセスポイントを復活
    /// </summary>
    public void RevivalAllAccessPoint()
    {
        BoxCollider[] bc = GetComponents<BoxCollider>();
        for (int i = 0; i < bc.Length; i++)
        {
            bc[i].enabled = true;
        }
    }
}
