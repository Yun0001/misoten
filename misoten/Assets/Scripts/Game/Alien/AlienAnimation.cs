using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンアニメーションスクリプト
/// </summary>
public class AlienAnimation : MonoBehaviour
{
	//private const int ANIMATION_STATUS_PATTERN = 2;// アニメーションが必要な状態の数
	private const int IS_CATERING = 2;// 通常状態と配膳中
	private const int ANIMATION_NUM = 4;// アニメーションの数


	[SerializeField, Range(0.0f, 100.0f)]
	private float oneAnimPatternSwitchTime;

	private float oneAnimPatternTime;

	private int countAnimTime = 0;

	private string[] folderPass = { "Textures/Alien/Wait/", "Textures/Alien/Work/" };

	private string[] waitTextureName = { "1", "2", "1", "2" };

	private string[] workTextureName = { "kod218vgo1", "kod218vgo2", "kod218vgo3", "kod218vgo4" };

	// スプライト
	public Sprite[,,] sprite = new Sprite[IS_CATERING, 2, ANIMATION_NUM];

	private AlienMove.EDirection alienUDDirection = AlienMove.EDirection.Right;
	private int isCatering = 0;

	[SerializeField]
	private int animID = 0;

	// Use this for initialization
	void Awake()
	{
		oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

		// 待機画像ロード
		WaiitAnimationSpriteLoad();

		// 歩行画像ロード
		WorkAnimationSpriteLoad();
	}

	private void WaiitAnimationSpriteLoad()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, i, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName[k]);
			}
		}
	}

	private void WorkAnimationSpriteLoad()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, i, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName[k]);
			}
		}
	}

	private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[isCatering, (int)alienUDDirection, animID];

	public void SetAlienUDDirection(AlienMove.EDirection direction) => alienUDDirection = direction;

	public void SetAlienRLDirection(AlienMove.EDirection direction)
	{
		//playerRLDirection = direction;
		if (direction == AlienMove.EDirection.Right)
		{
			Vector3 scale = transform.localScale;
			scale.x = 0.3f;
			transform.localScale = scale;
		}
		else if (direction == AlienMove.EDirection.Left)
		{
			Vector3 scale = transform.localScale;
			scale.x = -0.3f;
			transform.localScale = scale;
		}
	}


	public void SetIsCatering(bool catering)
	{
		if (catering) isCatering = 1;
		else isCatering = 0;
	}

	void Update()
	{
		countAnimTime++;
		if (countAnimTime > oneAnimPatternTime)
		{
			countAnimTime = 0;
			animID++;
			if (animID > ANIMATION_NUM - 1) animID = 0;
			ChangeSprite();
		}
	}
}
