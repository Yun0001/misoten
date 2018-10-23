using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動の種類スクリプト
/// ※特定の動きだけをする物はここに処理を追加していく
/// </summary>
public class MoveType : MonoBehaviour
{
	/// <summary>
	/// 移動パターン
	/// </summary>
	[SerializeField] private enum EMoveType
	{
		REPETITION_X = 0,
		REPETITION_Y,
		REPETITION_Z,
		FOLLOWING,
		CIRCULAR_MOTION_XY,
		CIRCULAR_MOTION_YZ,
		CIRCULAR_MOTION_XZ
	}

	private Vector3 m_getPos;	// オブジェクト位置取得用
	private Vector3 m_velocity; // オブジェクト速度加算用

	[SerializeField] Transform m_target;							// ターゲット設定用
	[Range(0, 1.0f)] [SerializeField] private float m_attenuation;	// 減衰
	[Range(-100, 100.0f)] [SerializeField] private float m_speed;	// 移動速度
	[SerializeField] private EMoveType m_moveType;					// 移動の種類を格納する為の変数
	[SerializeField] private Vector3 m_moveRange;					// 移動範囲(幅、高さ、奥行き)

	/// <summary>
	/// 開始関数
	/// </summary>
	private void Start ()
	{
		// オブジェクトの位置取得
		m_getPos = GameObject.Find(transform.name).transform.position;
	}

	/// <summary>
	/// 更新関数
	/// </summary>
	private void Update ()
	{
		// 移動パターンの管理
		switch (m_moveType)
		{
			case EMoveType.REPETITION_X:
				// 指定した範囲(X軸)に繰り返し移動を行う
				transform.position = m_getPos + new Vector3(Mathf.Sin(Time.time * m_speed) * m_moveRange.x, 0.0f, 0.0f);
				break;
			case EMoveType.REPETITION_Y:
				// 指定した範囲(Y軸)に繰り返し移動を行う
				transform.position = m_getPos + new Vector3(0.0f, Mathf.Cos(Time.time * m_speed) * m_moveRange.y, 0.0f);
				break;
			case EMoveType.REPETITION_Z:
				// 指定した範囲(Z軸)に繰り返し移動を行う
				transform.position = m_getPos + new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time * m_speed) * m_moveRange.z);
				break;
			case EMoveType.FOLLOWING:
				// 指定したオブジェクトに追従を行う
				m_velocity += (m_target.position - transform.position) * m_speed;
				m_velocity *= m_attenuation;
				transform.position += m_velocity *= Time.deltaTime;
				break;
			case EMoveType.CIRCULAR_MOTION_XY:
				// 指定したオブジェクトが円運動(X,Y軸)に行う
				transform.position = m_getPos + new Vector3(
					Mathf.Cos(Time.time * m_speed) * m_moveRange.x,
					Mathf.Sin(Time.time * m_speed) * m_moveRange.y, 0.0f);
				break;
			case EMoveType.CIRCULAR_MOTION_YZ:
				// 指定したオブジェクトが円運動(Y,Z軸)に行う
				transform.position = m_getPos + new Vector3(
					0.0f, Mathf.Sin(Time.time * m_speed) * m_moveRange.y,
					Mathf.Cos(Time.time * m_speed) * m_moveRange.z);
				break;
			case EMoveType.CIRCULAR_MOTION_XZ:
				// 指定したオブジェクトが円運動(X,Z軸)に行う
				transform.position = m_getPos + new Vector3(
					Mathf.Cos(Time.time * m_speed) * m_moveRange.x,
					0.0f, Mathf.Sin(Time.time * m_speed) * m_moveRange.z);
				break;
			default:
				break;
		}
	}
}
