using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFragment : MonoBehaviour
{
	[SerializeField]
	private int id;

	private IceBoxMiniGame iceBoxMiniGame;

	// Use this for initialization
	void Start ()
	{
		if(id == 0) { iceBoxMiniGame = GameObject.Find("GameSceneManager/PauseManager/Stage/cookwares/iceboxes/icebox1/IceBoxMiniGame").gameObject.GetComponent<IceBoxMiniGame>(); }
		else { iceBoxMiniGame = GameObject.Find("GameSceneManager/PauseManager/Stage/cookwares/iceboxes/icebox2/IceBoxMiniGame").gameObject.GetComponent<IceBoxMiniGame>(); }
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(iceBoxMiniGame.GetMoveflg())
		{
			GetComponent<ParticleSystem>().Play();
		}
		else
		{
			GetComponent<ParticleSystem>().Stop();
		}
	}
}
