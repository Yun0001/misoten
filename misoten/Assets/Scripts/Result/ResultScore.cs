using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScore : MonoBehaviour
{
	private Sprite[] sprits;

	[SerializeField]
	private GameObject[] spriteObj;

	void Awake()
	{
		sprits = Resources.LoadAll<Sprite>("Textures/UI_Digital2");
		for (int i = 0; i < spriteObj.Length; i++)
		{
			spriteObj[i].GetComponent<SpriteRenderer>().sprite = sprits[0];
		}

	}

	void Update()
	{
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		int suu = ScoreCount.GetScore();
		int rement;
		for (int i = 0; i < spriteObj.Length; i++)
		{
			rement = suu % 10;
			spriteObj[i].GetComponent<SpriteRenderer>().sprite = sprits[rement];
			suu = suu / 10;
		}
	}
}
