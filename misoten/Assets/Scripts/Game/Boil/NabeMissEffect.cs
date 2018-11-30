using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NabeMissEffect : MonoBehaviour
{
	// インスペクター上で設定可能
	// ---------------------------------------------

	// 失敗時のエフェクト設定
	[SerializeField]
	private ParticleSystem missParticle;

	// イライラオブジェクト設定
	[SerializeField]
	private GameObject irairaObj;

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// オブジェクト設定用
	private ParticleSystem setObj;

	// ---------------------------------------------

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
		if(CookingPot.inFlag)
		{
			// エフェクトフラグ「ON」の時
			if (irairaObj.GetComponent<IraIraFrame>().GetEffectFlag())
			{
				// エフェクト再生
				setObj.GetComponent<ParticleSystem>().Play();
				irairaObj.GetComponent<IraIraFrame>().SetEffectFlag(false);
			}
			else { setObj.GetComponent<ParticleSystem>().Stop(); }
		}
		else
		{
			CookingPot.inFlag = true;
			irairaObj.GetComponent<IraIraFrame>().SetEffectFlag(false);
			setObj.GetComponent<ParticleSystem>().Stop();
		}
	}
}
