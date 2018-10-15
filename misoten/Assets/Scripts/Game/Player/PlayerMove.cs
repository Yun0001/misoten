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
        if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering) move /= 2;// 配膳中ならば移動量半減

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
        float width = 7f;
        Vector2 min = new Vector2(-width, -4.5f);
        Vector2 max = new Vector2(width, 0.9f);

        Vector2 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}
