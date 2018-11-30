using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NabeMissEffect : MonoBehaviour
{
	// 失敗時のエフェクト設定
	[SerializeField]
	private ParticleSystem missParticle;

	// イライラオブジェクト設定
	[SerializeField]
	private GameObject irairaObj;

	// オブジェクト設定用
	private ParticleSystem setObj;

	private void Awake()
	{
		// エフェクト生成
		setObj = Instantiate(missParticle, new Vector3(0.0f, 1.0f, -4.5f), Quaternion.identity) as ParticleSystem;
		setObj.transform.SetParent(transform);
		setObj.GetComponent<ParticleSystem>().Stop();
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// エフェクトフラグ「ON」の時
		if(irairaObj.GetComponent<IraIraFrame>().GetEffectFlag())
		{
			// エフェクト再生
			setObj.GetComponent<ParticleSystem>().Play();
			irairaObj.GetComponent<IraIraFrame>().SetEffectFlag(false);
		}
		else { setObj.GetComponent<ParticleSystem>().Stop(); }
	}
}
