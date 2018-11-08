using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : KitchenwareBase {

    public enum Status
    {
        Stand,
        AccessOne,
        AccessTwo,
        AccessThree,
        Start,
        Open,
        Play,
        End
    }

    private float time;
    private float count;
    private float power;
    private float animCount;

    private bool accessFlg;
    private bool efectFlg;
    private bool seFlg;
    private bool uiFlg;
    private bool rotationFlg;

    [SerializeField]
    private Status status = Status.Stand;


    // Use this for initialization
    void Awake () {
		
	}

    protected override void InstanceMiniGameUI()
    {
    }

    protected override void InitMiniGameUI()
    {
    }

    protected override void ResetMiniGameUI()
    {
    }

    protected override GameObject SetCuisine()
    {
        return null;
    }

    protected override bool Cooking()
    {
        return false;
    }

    public override void CookingInterruption()
    {
    }

    /// <summary>
    /// 調理準備
    /// </summary>
    public bool Access(Vector3 accesspos)
    {
        if (!DecisionAccessPoint(accesspos))
        {
            Debug.LogError("アクセスポイントがおかしい");
            return false;
        }

        if (status >= Status.AccessThree)
        {
            Debug.LogError("これ以上アクセスできません！");
            return false;
        }

        status++;
        return true;
    }

    private bool DecisionAccessPoint(Vector3 accesspos)
    {
        float border = transform.position.z - 0.5f;
        BoxCollider[] bc = GetComponents<BoxCollider>();
        if (accesspos.z < border)
        {
            bc[0].enabled = false;
            return true;
        }
        else if (accesspos.x > transform.position.x)
        {
            bc[1].enabled = false;
            return true;
        }
        else if (accesspos.x < transform.position.x)
        {
            bc[2].enabled = false;
            return true;
        }
        return false;
    }

    public Status GetStatus() => status;
}
