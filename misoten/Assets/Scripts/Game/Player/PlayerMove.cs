using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum EDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private Player player_cs;
    private Vector2 move;
    private Rigidbody2D rb;
    [SerializeField]    [Range(0.0f, 30.0f)]
    private float speed;

    [SerializeField]    [Range(0.0f, 1.0f)]
    private float CateringCoefficient;

    [SerializeField]    [Range(0.0f, 1.0f)]
    private float TasteChargeCoefficient;

    public void Init()
    {
        player_cs = GetComponent<Player>();
        rb = player_cs.gameObject.GetComponent<Rigidbody2D>();
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
        if (pStatus == Player.PlayerStatus.Explosion) return;


        // 配膳中または味の素チャージ中ならば移動量減少
        switch (pStatus)
        {
            case Player.PlayerStatus.Catering:
                move *= CateringCoefficient;
                break;

            case Player.PlayerStatus.TasteCharge:
                move *= TasteChargeCoefficient;
                break;

            case Player.PlayerStatus.Hindrance:
                move = new Vector2(0f,0f);
                break;
        }

        rb.velocity = move * speed;

        // コントローラー４つの場合は不要
        move = new Vector2(0f, 0f);
        //
    }

    public void SetMove(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Up:
                move.y = speed;
                break;

            case EDirection.Down:
                move.y = -speed;
                break;

            case EDirection.Right:
                move.x = speed;
                break;

            case EDirection.Left:
                move.x = -speed;
                break;
        }
    }

    public void SetMove(Vector2 vec)
    {
        move = vec;
    }

    private void Clamp()
    {
        float width = 8f;
        Vector2 min = new Vector2(-width, -4.5f);
        Vector2 max = new Vector2(width, 1.5f);

        Vector2 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}
