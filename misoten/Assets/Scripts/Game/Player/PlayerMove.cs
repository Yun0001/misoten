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

    [SerializeField]
    private ParticleSystem prefab = new ParticleSystem();

    // 呼び出したエフェクトの確認用
    [SerializeField]
    private ParticleSystem particleSystems = new ParticleSystem();

    private bool effectFlag = false;

    private Player player_cs;
    private Vector3 move;
    private Rigidbody rb;
    [SerializeField]    [Range(0.0f, 30.0f)]
    private float speed;

    [SerializeField]    [Range(0.0f, 1.0f)]
    private float CateringCoefficient;

    [SerializeField, Range(1, 50)]
    private int adjustment;

    [SerializeField]
    private GameObject timeManager;



    // 初期化
    public void Init()
    {
        player_cs = GetComponent<Player>();
        rb = player_cs.gameObject.GetComponent<Rigidbody>();

        // エフェクト生成
        InstanceMoveEffect();
    }

    // 更新処理
    private void Update()
    {
        // 移動範囲制限
        Clamp();

        // エフェクトストップ
        if (move == Vector3.zero) StopMoveEffect();
    }

    public void Move()
    {
        if (timeManager.GetComponent<GameTimeManager>().IsTimeUp()) return;
        // 移動可能状態でない場合抜ける
        if (!IsMoveStatus()) return;

        // 配膳中ならば移動量減少
        if (player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering) move *= CateringCoefficient;

        rb.velocity = move * speed;

        // 移動エフェクト
        MoveEffectCall();
    }


    public void SetMove(Vector3 vec)
    {
        if (!IsMoveStatus()) return;

        move = vec;

        // 歩行か待機か
        GetComponent<PlayerAnimCtrl>().SetWalking(IsWalking());

        // 前後設定
        if (move.z != 0) GetComponent<PlayerAnimCtrl>().SetFront(IsFront());

        //左右反転設定
        if (move.x != 0) GetComponent<SpriteRenderer>().flipX = IsLeftDirection();
    }

    /// <summary>
    /// 移動可能状態か判定
    /// </summary>
    /// <returns></returns>
    private bool IsMoveStatus()
    {
        //通常状態、配膳状態、氷配膳状態はtrue
        return 
            player_cs.GetPlayerStatus() == Player.PlayerStatus.Normal ||
            player_cs.GetPlayerStatus() == Player.PlayerStatus.Catering ||
            player_cs.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy;
    }

    private bool IsWalking() => ((move.x != 0) || (move.z != 0));

    /// <summary>
    /// スティックの左入力があるか
    /// </summary>
    /// <returns></returns>
    private bool IsLeftDirection() => move.x > 0;

    private bool IsFront()
    {
        return 
            ((move.z < 0) ||
            (GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.Catering) ||
            (GetComponent<Player>().GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy));
    }
    /// <summary>
    /// 移動範囲制限
    /// </summary>
    private void Clamp()
    {
        float width = 5.0f;
        Vector3 min = new Vector3(-width, 0, -3.45f);
        Vector3 max = new Vector3(width, 0,2.0f);

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.z= Mathf.Clamp(pos.z, min.z, max.z);

        transform.position = pos;
    }

    /// <summary>
    /// Velocityのリセット
    /// </summary>
    public void VelocityReset() => rb.velocity = Vector3.zero;

    /// <summary>
    /// 移動エフェクト発生
    /// </summary>
    void MoveEffectCall()
    {
        // 一度しか通らない
        if (!effectFlag)
        {
            // 拡縮設定

            // パーティクル再生
            particleSystems.Play();

            // 一度しか通らないようにする
            effectFlag = true;
        }
    }

    /// <summary>
    /// エフェクト生成
    /// </summary>
    private void InstanceMoveEffect()
    {
        particleSystems = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity) as ParticleSystem;
        particleSystems.transform.SetParent(transform);
    }

    /// <summary>
    /// エフェクトストップ
    /// </summary>
    private void StopMoveEffect()
    {
        particleSystems.Stop();

        effectFlag = false;
    }
}
