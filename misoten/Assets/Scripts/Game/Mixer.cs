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
    private float animCount = 0;

    // 蓋が開いてるフレーム
    [SerializeField]
    private int openFrame;

    [SerializeField]
    private int accessNum = 0;
    private bool efectFlg;
    private bool seFlg;
    private bool uiFlg;
    private bool rotationFlg;

    [SerializeField]
    private Status status = Status.Stand;


    // Use this for initialization
    void Awake () {
		
	}

    private void Update()
    {
        if (!IsUpdate()) return;

        switch (status)
        {
            case Status.AccessTwo:

                if (accessNum == (int)Status.AccessTwo)
                {
                    // 調理開始
                    status = Status.Open;
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
                }
                break;

            case Status.Open:
                animCount++;
                if (animCount >= openFrame)
                {
                    status = Status.Play;
                    transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(false);
                }
                break;
        }



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
        if (status == Status.AccessThree)
        {
            accessNum = 3;
            status = Status.Open;
            transform.Find("mixer").GetComponent<mixerAnimCtrl>().SetIsOpen(true);
        }
        return true;
    }

    public bool DecisionAccessPoint(Vector3 accesspos)
    {
        float border = transform.position.z - 0.5f;

        if (accesspos.z < border)
        {
            ChangeBoxColliderEnable(0);
            return true;
        }
        else if (accesspos.x > transform.position.x)
        {
            ChangeBoxColliderEnable(1);
            return true;
        }
        else if (accesspos.x < transform.position.x)
        {
            ChangeBoxColliderEnable(2);
            return true;
        }
        return false;
    }

    private void ChangeBoxColliderEnable(int element)
    {
        BoxCollider[] bc = GetComponents<BoxCollider>();
        bc[element].enabled = !bc[element].enabled;
    }

    public void ReturnStatus() => status--;

    public void AddAccessNum() => accessNum++;

    public void SubAccessNum() => accessNum--;

    public Status GetStatus() => status;

    private bool IsUpdate()
    {
        return (status == Status.AccessTwo) || (status == Status.Open);
    }
}
