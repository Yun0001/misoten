using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアンアニメーションスクリプト
/// </summary>
public class AlienAnimation : MonoBehaviour
{
	private enum EAlienPattern
	{
		MARTIAN = 0,
		MERCURY,
		VENUSIAN,
        BOSS,
		MAX
	}

	public enum EAlienAnimation
	{
		WAIT = 0,		// 待機
		WORK,			// 歩く
		ANGER,			// 怒り
		SATISFACTION,	// 満足
		EAT,			// 食事
		MAX
	}

	[SerializeField]
	private EAlienPattern alienPattern;

	//private const int ANIMATION_STATUS_PATTERN = 2;// アニメーションが必要な状態の数
	private const int IS_CATERING = 5;// 待機、歩き、怒り、満足、食事
	private const int ANIMATION_NUM = 4;// アニメーションの数


	[SerializeField, Range(1.0f, 100.0f)]
	private float oneAnimPatternSwitchTime;

	private float oneAnimPatternTime;

	private float countAnimTime = 0;

	private string[] folderPass = { "Textures/Alien/Wait/", "Textures/Alien/Work/", "Textures/Alien/Anger/", "Textures/Alien/Satisfaction/", "Textures/Alien/Eat/" };

	private string[] waitTextureName1 = { "Martian/1", "Martian/2", "Martian/1", "Martian/2" };
	private string[] waitTextureName2 = { "Mercury/1", "Mercury/2", "Mercury/1", "Mercury/2" };
	private string[] waitTextureName3 = { "Venusian/1", "Venusian/2", "Venusian/1", "Venusian/2" };
    private string[] waitTextureName4 = { "Boss/1", "Boss/2", "Boss/1", "Boss/2" };

	private string[] workTextureName1 = { "Martian/1", "Martian/2", "Martian/3", "Martian/4" };
	private string[] workTextureName2 = { "Mercury/1", "Mercury/2", "Mercury/3", "Mercury/4" };
	private string[] workTextureName3 = { "Venusian/1", "Venusian/2", "Venusian/3", "Venusian/4" };
    private string[] workTextureName4 = { "Boss/1", "Boss/2", "Boss/1", "Boss/2" };

	private string[] angerTextureName1 = { "Martian/1", "Martian/2", "Martian/1", "Martian/2" };
	private string[] angerTextureName2 = { "Mercury/1", "Mercury/2", "Mercury/1", "Mercury/2" };
	private string[] angerTextureName3 = { "Venusian/1", "Venusian/2", "Venusian/1", "Venusian/2" };
	private string[] angerTextureName4 = { "Boss/1", "Boss/1", "Boss/1", "Boss/1" };

	private string[] satisfactionTextureName1 = { "Martian/1", "Martian/1", "Martian/1", "Martian/1" };
	private string[] satisfactionTextureName2 = { "Mercury/1", "Mercury/1", "Mercury/1", "Mercury/1" };
	private string[] satisfactionTextureName3 = { "Venusian/1", "Venusian/1", "Venusian/1", "Venusian/1" };
    private string[] satisfactionTextureName4 = { "Boss/1", "Boss/1", "Boss/1", "Boss/1" };

	private string[] eatTextureName1 = { "Martian/1", "Martian/2", "Martian/1", "Martian/2" };
	private string[] eatTextureName2 = { "Mercury/1", "Mercury/2", "Mercury/1", "Mercury/2" };
	private string[] eatTextureName3 = { "Venusian/1", "Venusian/2", "Venusian/1", "Venusian/2" };
    private string[] eatTextureName4 = { "Boss/1", "Boss/2", "Boss/1", "Boss/2" };

	// スプライト
	public Sprite[,,,] sprite = new Sprite[(int)EAlienPattern.MAX, IS_CATERING, 2, ANIMATION_NUM];

	private AlienMove.EDirection alienUDDirection = AlienMove.EDirection.Right;
	private int isCatering = 0;

	[SerializeField]
	private int animID = 0;

	// Use this for initialization
	void Awake()
	{
		oneAnimPatternTime = oneAnimPatternSwitchTime / ANIMATION_NUM;

		// 待機・歩き・怒り・満足画像ロード
		switch (alienPattern)
		{
			case EAlienPattern.MARTIAN:
				WaitAnimationSpriteLoad1();
				WorkAnimationSpriteLoad1();
				AngerAnimationSpriteLoad1();
				SatisfactionAnimationSpriteLoad1();
				EatAnimationSpriteLoad1();
				break;
			case EAlienPattern.MERCURY:
				WaitAnimationSpriteLoad2();
				WorkAnimationSpriteLoad2();
				AngerAnimationSpriteLoad2();
				SatisfactionAnimationSpriteLoad2();
				EatAnimationSpriteLoad2();
				break;
			case EAlienPattern.VENUSIAN:
				WaitAnimationSpriteLoad3();
				WorkAnimationSpriteLoad3();
				AngerAnimationSpriteLoad3();
				SatisfactionAnimationSpriteLoad3();
				EatAnimationSpriteLoad3();
				break;
                //Todo:ボスアニメーション追加予定
            case EAlienPattern.BOSS:
				WaitAnimationSpriteLoad4();
				WorkAnimationSpriteLoad4();
				AngerAnimationSpriteLoad4();
				SatisfactionAnimationSpriteLoad4();
				EatAnimationSpriteLoad4();
				break;
			default: break;
		}
	}

	private void WaitAnimationSpriteLoad1()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, 0, i, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName1[k]);
			}
		}
	}

	private void WorkAnimationSpriteLoad1()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, 1, i, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName1[k]);
			}
		}
	}

	private void AngerAnimationSpriteLoad1()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, 2, i, k] = Resources.Load<Sprite>(folderPass[2] + angerTextureName1[k]);
			}
		}
	}

	private void SatisfactionAnimationSpriteLoad1()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, 3, i, k] = Resources.Load<Sprite>(folderPass[3] + satisfactionTextureName1[k]);
			}
		}
	}

	private void EatAnimationSpriteLoad1()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[0, 4, i, k] = Resources.Load<Sprite>(folderPass[4] + eatTextureName1[k]);
			}
		}
	}

	private void WaitAnimationSpriteLoad2()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, 0, i, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName2[k]);
			}
		}
	}

	private void WorkAnimationSpriteLoad2()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, 1, i, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName2[k]);
			}
		}
	}

	private void AngerAnimationSpriteLoad2()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, 2, i, k] = Resources.Load<Sprite>(folderPass[2] + angerTextureName2[k]);
			}
		}
	}

	private void SatisfactionAnimationSpriteLoad2()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, 3, i, k] = Resources.Load<Sprite>(folderPass[3] + satisfactionTextureName2[k]);
			}
		}
	}

	private void EatAnimationSpriteLoad2()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[1, 4, i, k] = Resources.Load<Sprite>(folderPass[4] + eatTextureName2[k]);
			}
		}
	}

	private void WaitAnimationSpriteLoad3()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[2, 0, i, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName3[k]);
			}
		}
	}

	private void WorkAnimationSpriteLoad3()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[2, 1, i, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName3[k]);
			}
		}
	}

	private void AngerAnimationSpriteLoad3()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[2, 2, i, k] = Resources.Load<Sprite>(folderPass[2] + angerTextureName3[k]);
			}
		}
	}

	private void SatisfactionAnimationSpriteLoad3()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[2, 3, i, k] = Resources.Load<Sprite>(folderPass[3] + satisfactionTextureName3[k]);
			}
		}
	}

	private void EatAnimationSpriteLoad3()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[2, 4, i, k] = Resources.Load<Sprite>(folderPass[4] + eatTextureName3[k]);
			}
		}
	}

    //boss
    private void WaitAnimationSpriteLoad4()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[3, 0, i, k] = Resources.Load<Sprite>(folderPass[0] + waitTextureName4[k]);
			}
		}
	}

	private void WorkAnimationSpriteLoad4()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[3, 1, i, k] = Resources.Load<Sprite>(folderPass[1] + workTextureName4[k]);
			}
		}
	}

	private void AngerAnimationSpriteLoad4()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[3, 2, i, k] = Resources.Load<Sprite>(folderPass[2] + angerTextureName4[k]);
			}
		}
	}

	private void SatisfactionAnimationSpriteLoad4()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[3, 3, i, k] = Resources.Load<Sprite>(folderPass[3] + satisfactionTextureName4[k]);
			}
		}
	}

	private void EatAnimationSpriteLoad4()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int k = 0; k < 4; k++)
			{
				sprite[3, 4, i, k] = Resources.Load<Sprite>(folderPass[4] + eatTextureName4[k]);
			}
		}
	}

	private void ChangeSprite() => GetComponent<SpriteRenderer>().sprite = sprite[(int)alienPattern, isCatering, (int)alienUDDirection, animID];

	public void SetAlienUDDirection(AlienMove.EDirection direction) => alienUDDirection = direction;

	public void SetAlienRLDirection(AlienMove.EDirection direction)
	{
        //TODO ボスサイズ変更
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

	public void SetIsCatering(int catering) => isCatering = catering;

	void Update()
	{
		if (AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT))
		{
			countAnimTime += 2.0f;
		}
		else { countAnimTime += 1.0f; }
		if (countAnimTime > oneAnimPatternTime)
		{
			countAnimTime = 0;
			animID++;
			if (animID > ANIMATION_NUM - 1) animID = 0;
			ChangeSprite();
		}
	}
}
