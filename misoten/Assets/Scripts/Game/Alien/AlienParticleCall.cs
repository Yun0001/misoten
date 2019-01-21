using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エイリアン関係のエフェクトの呼び出し
/// </summary>
public class AlienParticleCall : MonoBehaviour
{
	// エイリアン管理用列挙型
	// ---------------------------------------------

	/// <summary>
	/// エイリアンが出すエフェクトの種類
	/// </summary>
	private enum EParticlePattern
	{
		MOVE = 0,		// 移動時
		SATISFACTION,	// 満足時
		CLAIM,			// クレーム時
		EAT,			// イート時
		CHIPHANDOVER,	// チップを渡した時
		AURA,			// オーラ
		MAX				// 最大
	}

	// ---------------------------------------------

	// インスペクター上で設定可能
	// ---------------------------------------------

	// 呼び出すエフェクトの設定
	[SerializeField]
	private ParticleSystem[] prefab = new ParticleSystem[(int)EParticlePattern.MAX];

	// 呼び出したエフェクトの確認用
	[SerializeField]
	private ParticleSystem[] particleSystems = new ParticleSystem[(int)EParticlePattern.MAX];

	// ---------------------------------------------

	// ローカル変数
	// ---------------------------------------------

	// エフェクトフラグ
	private bool[] effectFlag = new bool[(int)EParticlePattern.MAX];

	// ---------------------------------------------

	/// <summary>
	/// 開始関数
	/// </summary>
	void Start()
	{
		// 各エフェクトの初期化
		for(int i = 0; i < (int)EParticlePattern.MAX ; i++) { effectFlag[i] = false; }
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	void Update ()
	{
		// 移動時エフェクト
		MoveEffectCall();

		// 満足時エフェクト
		SatisfactionEffectCall();

		// 怒り時(クレーム)時エフェクト
		AngerEffectCall();

		// イート時のエフェクト
		EatEffectCall();

		// チップを渡した時のエフェクト
		ChipHandOverEffectCall();

		// チェンジイートイを注文しているエイリアンのエフェクト
		AuraEffectCall();

        //Todo
        if( (Input.GetKeyDown("i")))
        {
            Debug.Log("De");
            effectFlag[(int)EParticlePattern.SATISFACTION] =false;
        }
	}

	/// <summary>
	/// 移動時のエフェクト呼び出し関数
	/// </summary>
	void MoveEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.MOVE])
		{
			// 移動状態の時
			if (GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() || GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// パーティクル生成
				particleSystems[(int)EParticlePattern.MOVE] = Instantiate(prefab[(int)EParticlePattern.MOVE], new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity) as ParticleSystem;
				particleSystems[(int)EParticlePattern.MOVE].transform.SetParent(transform);

				// 拡縮設定
				ScaleConfiguration(EParticlePattern.MOVE, new Vector3(1.0f, 1.0f, 1.0f));

				// パーティクル再生
				particleSystems[(int)EParticlePattern.MOVE].Play();

				// 一度しか通らないようにする
				effectFlag[(int)EParticlePattern.MOVE] = true;
			}
		}
		else
		{
			// 移動状態ではない時
			if (!GetComponent<AlienMove>().GetWhenEnteringStoreMoveFlag() && !GetComponent<AlienMove>().GetWhenLeavingStoreFlag())
			{
				// パーティクル停止
				particleSystems[(int)EParticlePattern.MOVE].Stop();

				// パーティクル削除
				Destroy(particleSystems[(int)EParticlePattern.MOVE]);

				// 退店時の時用に
				effectFlag[(int)EParticlePattern.MOVE] = false;
			}
		}
	}

	/// <summary>
	/// 満足時のエフェクト呼び出し関数
	/// </summary>
	void SatisfactionEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.SATISFACTION])
		{
			// 満足状態の時
			if(GetComponent<AlienSatisfaction>().GetSatisfactionFlag())
			{
				// 状態移行フラグが「ON」の時
				if (GetComponent<AlienOrder>().GetStatusMoveFlag())
				{
					// 満足時のSE
					//Sound.PlaySe(GameSceneManager.seKey[4]);
					Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Offer_succes), 2);

					// パーティクル生成
					particleSystems[(int)EParticlePattern.SATISFACTION] = Instantiate(prefab[(int)EParticlePattern.SATISFACTION], new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity) as ParticleSystem;
					particleSystems[(int)EParticlePattern.SATISFACTION].transform.SetParent(transform);

					// 拡縮設定
					ScaleConfiguration(EParticlePattern.SATISFACTION, new Vector3(1.0f, 1.0f, 1.0f));

					// パーティクル再生
					particleSystems[(int)EParticlePattern.SATISFACTION].Play();

					// 一度しか通らないようにする
					effectFlag[(int)EParticlePattern.SATISFACTION] = true;
                    //ToDo　エフェクト　boss 注文時false にする
                    // 満足「ON」もfalse
					AlienStatus.SetCounterStatusChangeFlag(false, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.SATISFACTION);
                    //GetComponent<BoxCollider>().enabled = true;
     
				}
                Debug.Log("EF");
			}
          
		}

 

	}

	/// <summary>
	/// 怒り(クレーム)時のエフェクト呼び出し関数
	/// </summary>
	void AngerEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.CLAIM])
		{
			// クレーム状態の時
			if (GetComponent<AlienClaim>().GetIsClaim())
			{
				// 状態移行フラグが「ON」の時
				if (GetComponent<AlienOrder>().GetStatusMoveFlag())
				{
					// SEを鳴らす
					//Sound.PlaySe(GameSceneManager.seKey[2]);
					Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Getangry), 3);

					// パーティクル生成
					particleSystems[(int)EParticlePattern.CLAIM] = Instantiate(prefab[(int)EParticlePattern.CLAIM], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as ParticleSystem;
					particleSystems[(int)EParticlePattern.CLAIM].transform.SetParent(transform);

					// 拡縮設定
					ScaleConfiguration(EParticlePattern.CLAIM, new Vector3(1.0f, 1.0f, 1.0f));

					// パーティクル再生
					particleSystems[(int)EParticlePattern.CLAIM].Play();

					// 一度しか通らないようにする
					effectFlag[(int)EParticlePattern.CLAIM] =true;
                    //ToDo　エフェクト　boss 注文時false にする
                    // 満足「ON」もfalse
					AlienStatus.SetCounterStatusChangeFlag(false, GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.SATISFACTION);
                    //GetComponent<BoxCollider>().enabled = true;
                    
				}
			}
		}

	}

	/// <summary>
	/// イート時のエフェクト呼び出し関数
	/// </summary>
	void EatEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.EAT])
		{
			// イート状態の時
			if (AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT))
			{
				// SEを鳴らす
				//Sound.PlaySe(GameSceneManager.seKey[2]);
				Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Eat_success), true, 2);
				Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Eat_success), 2);

				// パーティクル生成
				particleSystems[(int)EParticlePattern.EAT] = Instantiate(prefab[(int)EParticlePattern.EAT], new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z - 0.4f), Quaternion.identity) as ParticleSystem;
				particleSystems[(int)EParticlePattern.EAT].transform.SetParent(transform);

				// 拡縮設定
				ScaleConfiguration(EParticlePattern.EAT, new Vector3(1.0f, 1.0f, 1.0f));

				// パーティクル再生
				particleSystems[(int)EParticlePattern.EAT].Play();

				// 一度しか通らないようにする
				effectFlag[(int)EParticlePattern.EAT] = true;
			}
		}
		else
		{
			// イート状態ではない時
			if (!AlienStatus.GetCounterStatusChangeFlag(GetComponent<AlienOrder>().GetSetId(), (int)AlienStatus.EStatus.EAT))
			{
				Sound.SetLoopFlgSe(SoundController.GetGameSEName(SoundController.GameSE.Eat_success), false, 2);

				// パーティクル停止
				particleSystems[(int)EParticlePattern.EAT].Stop();
			}
		}
	}

	/// <summary>
	/// チップを渡した時のエフェクト呼び出し関数
	/// </summary>
	void ChipHandOverEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.CHIPHANDOVER])
		{
			// 満足状態の時
			if (GetComponent<AlienSatisfaction>().GetSatisfactionFlag())
			{
				// 状態移行フラグが「ON」の時
				if (GetComponent<AlienOrder>().GetStatusMoveFlag())
				{
					// パーティクル生成
					particleSystems[(int)EParticlePattern.CHIPHANDOVER] = Instantiate(prefab[(int)EParticlePattern.CHIPHANDOVER], transform.position, Quaternion.identity) as ParticleSystem;
					particleSystems[(int)EParticlePattern.CHIPHANDOVER].transform.SetParent(transform);

					// 拡縮設定
					ScaleConfiguration(EParticlePattern.CHIPHANDOVER, new Vector3(1.0f, 1.0f, 1.0f));

					// パーティクル再生
					particleSystems[(int)EParticlePattern.CHIPHANDOVER].Play();

					// 一度しか通らないようにする
					effectFlag[(int)EParticlePattern.CHIPHANDOVER] = true;
				}
			}
		}
	}

	/// <summary>
	/// イベントエイリアンのエフェクト呼び出し関数
	/// </summary>
	void AuraEffectCall()
	{
		// 一度しか通らない
		if (!effectFlag[(int)EParticlePattern.AURA])
		{
			// イベントエイリアンが出すエフェクト
			if (AlienCall.GetAuraFlag(GetComponent<AlienOrder>().GetSetId()))
			{
				AlienCall.SetAuraFlag(false, GetComponent<AlienOrder>().GetSetId());

				// パーティクル生成
				particleSystems[(int)EParticlePattern.AURA] = Instantiate(prefab[(int)EParticlePattern.AURA], new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), Quaternion.identity) as ParticleSystem;
				particleSystems[(int)EParticlePattern.AURA].transform.SetParent(transform);

				// 拡縮設定
				ScaleConfiguration(EParticlePattern.AURA, new Vector3(1.0f, 1.0f, 1.0f));

				// パーティクル再生
				particleSystems[(int)EParticlePattern.AURA].Play();

				// 一度しか通らないようにする
				effectFlag[(int)EParticlePattern.AURA] = true;
			}
		}
	}

	/// <summary>
	/// パーティクルの位置設定関数
	/// </summary>
	/// <param name="pattern"></param>
	/// <param name="position"></param>
	void PositionConfiguration(EParticlePattern pattern, Vector3 position) => particleSystems[(int)pattern].transform.position = position;

	/// <summary>
	/// パーティクルの拡縮設定関数
	/// </summary>
	void ScaleConfiguration(EParticlePattern pattern, Vector3 scale) => particleSystems[(int)pattern].transform.localScale = scale;
}