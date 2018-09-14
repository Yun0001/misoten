using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクルの演出が終了したらオブジェクトを削除するコールバックスクリプト
/// </summary>
public class ParticleCallback : MonoBehaviour
{
	// メインのゲームオブジェクト
    private GameObject m_owner;

	// 指定されたオブジェクトを削除する為のもの
	[SerializeField] private GameObject m_setObj;

	/// <summary>
	/// 開始関数
	/// </summary>
	private void Start()
	{
		// 処理内容は特に無し
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	private void Update()
	{
		// オブジェクトが「NULL」では無い時のみ
		if (m_owner != null)
		{
            this.transform.position = m_owner.transform.position;
        }
    }

	/// <summary>
	/// オブジェクトがアクティブ時に呼ばれる
	/// </summary>
	private void OnEnable()
	{
		// パーティクルのコルーチン開始
		StartCoroutine(ParticleWorking());
	}

	/// <summary>
	/// パーティクルのコルーチンの処理
	/// </summary>
	/// <returns></returns>
	IEnumerator ParticleWorking()
	{
		// パーティクルのコンポーネント取得
		var particle = GetComponent<ParticleSystem>();

        // パーティクルの演出中であればここで処理を止める
        yield return new WaitWhile(() => particle.IsAlive());

        // 演出が終わったらパーティクルとゲームオブジェクトを削除
        Destroy(m_setObj);
		Destroy(gameObject);
    }

	/// <summary>
	/// オブジェクトの状態を取得
	/// </summary>
	/// <param name="gameObject">オブジェクト取得</param>
    public void SetOwner(GameObject gameObject)
	{
        this.m_owner = gameObject;
    }
}
