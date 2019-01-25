using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンのリザルト用アニメーション
/// </summary>
public class AlienResultAnimation : MonoBehaviour
{
	private enum EAlienPattern
	{
		MARTIAN = 0,
		MERCURY,
		VENUSIAN,
        Boss,
		MAX
	}

	[SerializeField]
	private EAlienPattern alienPattern;

	private const int ANIMATION_NUM = 4;// アニメーションの数

	[SerializeField, Range(1.0f, 100.0f)]
	private float oneAnimPatternSwitchTime;

	private float oneAnimPatternTime;

	private float countAnimTime = 0;

	private string[] textureName1 = { "Textures/Alien/Wait/Martian/1", "Textures/Alien/Satisfaction/Martian/1", "Textures/Alien/Wait/Martian/1", "Textures/Alien/Satisfaction/Martian/1" };
	private string[] textureName2 = { "Textures/Alien/Wait/Mercury/1", "Textures/Alien/Satisfaction/Mercury/1", "Textures/Alien/Wait/Mercury/1", "Textures/Alien/Satisfaction/Mercury/1" };
	private string[] textureName3 = { "Textures/Alien/Wait/Venusian/1", "Textures/Alien/Satisfaction/Venusian/1", "Textures/Alien/Wait/Venusian/1", "Textures/Alien/Satisfaction/Venusian/1" };
    private string[] textureName4 = { "Textures/Alien/Wait/Boss/1", "Textures/Alien/Satisfaction/Boss/1", "Textures/Alien/Wait/Boss/1", "Textures/Alien/Satisfaction/Boss/1" };


	// スプライト
	public Sprite[,,] sprite = new Sprite[(int)EAlienPattern.MAX, 2, ANIMATION_NUM];

	private int animID = 0;

	void Awake()
	{
		oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

		// 待機・歩き・怒り・満足画像ロード
		switch (alienPattern)
		{
			case EAlienPattern.MARTIAN:
				AnimationSpriteLoad(0, textureName1);
				break;
			case EAlienPattern.MERCURY:
				AnimationSpriteLoad(1, textureName2);
				break;
			case EAlienPattern.VENUSIAN:
				AnimationSpriteLoad(2, textureName3);
				break;
            case EAlienPattern.Boss:
				AnimationSpriteLoad(3, textureName4);
				break;
			default: break;
		}
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		if (countAnimTime > oneAnimPatternTime)
		{
			countAnimTime = 0;
			animID++;
			if (animID > ANIMATION_NUM - 1) animID = 0;
			ChangeSprite();
		}
		countAnimTime += 1.0f;
	}

	private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[(int)alienPattern, 0, animID];

	private void AnimationSpriteLoad(int id, string[] name)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[id, i, k] = Resources.Load<Sprite>(name[k]);
			}
		}
	}

}
