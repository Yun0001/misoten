using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFragment : MonoBehaviour
{
	[SerializeField]
	private int id;

	private GameObject obj;

	// Use this for initialization
	void Start ()
	{
		if(id == 0) { obj = GameObject.Find("IceFragment_1").gameObject; }
		else { obj = GameObject.Find("GameSceneManager/PauseManager/Stage/cookwares/iceboxes/icebox2/IceBoxMiniGame/IceFragment_2").gameObject; }
	}

	/// <summary>
	/// アイスピックのエフェクト再生
	/// </summary>
	public void EffectPlay()
	{
		obj.GetComponent<ParticleSystem>().Play();
	}

	/// <summary>
	/// アイスピックのエフェクト停止
	/// </summary>
	public void EffectStop()
	{
		obj.GetComponent<ParticleSystem>().Stop();
	}
}
