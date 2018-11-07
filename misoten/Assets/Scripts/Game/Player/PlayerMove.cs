using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum EDirection
    {
        Down,
        Up,
        Right,
        Left
    }

    private Player player_cs;
    private PlayerAnimation playerAnimation_cs;
    private Vector3 move;
    private Rigidbody rb;
    [SerializeField]    [Range(0.0f, 30.0f)]
    private float speed;

    [SerializeField]    [Range(0.0f, 1.0f)]
    private float CateringCoefficient;

    [SerializeField]    [Range(0.0f, 1.0f)]
    private float TasteChargeCoefficient;

    [SerializeField, Range(1, 50)]
    private int adjustment;



    public void Init()
    {
        player_cs = GetComponent<Player>();
        playerAnimation_cs = GetComponent<PlayerAnimation>();
        rb = player_cs.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Clamp();
    }

    public void Move()
    {
        Player.PlayerStatus pStatus = player_cs.GetPlayerStatus();
        if (pStatus == Player.PlayerStatus.Microwave) return;
        if (pStatus == Player.PlayerStatus.Pot) return;
        if (pStatus == Player.PlayerStatus.GrilledTable) return;
        if (pStatus == Player.PlayerStatus.Hindrance) return;


        // 配膳中または味の素チャージ中ならば移動量減少
        switch (pStatus)
        {
            case Player.PlayerStatus.Catering:
                move *= CateringCoefficient;
                break;

            case Player.PlayerStatus.TasteCharge:
                move *= TasteChargeCoefficient;
                break;
        }

        rb.velocity = move * speed;

        // コントローラー４つの場合は不要
        move = Vector3.zero;
        //
    }

    public void SetMove(EDirection direction)
    {
        playerAnimation_cs.SetPlayerStatus(1);
        switch (direction)
        {
            case EDirection.Up:
                move.z = speed/ adjustment;
                playerAnimation_cs.SetPlayerUDDirection(EDirection.Up);
                break;

            case EDirection.Down:
                move.z = -speed/ adjustment;
                playerAnimation_cs.SetPlayerUDDirection(EDirection.Down);
                break;

            case EDirection.Right:
                move.x = speed/ adjustment;
                playerAnimation_cs.SetPlayerRLDirection(EDirection.Left);
                break;

            case EDirection.Left:
                move.x = -speed/ adjustment;
                playerAnimation_cs.SetPlayerRLDirection(EDirection.Right);
                break;
        }
    }

    public void SetMove(Vector3 vec)
    {
        if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Normal || player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering)
        {
            move = vec;
            if (move.x != 0 || move.z != 0) playerAnimation_cs.SetPlayerStatus(1);
            else playerAnimation_cs.SetPlayerStatus(0);

            if (move.x < 0) playerAnimation_cs.SetPlayerRLDirection(EDirection.Right);
            else if (move.x > 0) playerAnimation_cs.SetPlayerRLDirection(EDirection.Left);

            if (move.z < 0) playerAnimation_cs.SetPlayerUDDirection(EDirection.Down);
            else if (move.z > 0) playerAnimation_cs.SetPlayerUDDirection(EDirection.Up);
        }

    }

    private void Clamp()
    {
        float width = 4.0f;
        Vector3 min = new Vector3(-width, 0, -4.6f);
        Vector3 max = new Vector3(width, 0,2.0f);

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.z= Mathf.Clamp(pos.z, min.z, max.z);

        transform.position = pos;
    }

    public void VelocityReset() => rb.velocity = Vector3.zero;
}
