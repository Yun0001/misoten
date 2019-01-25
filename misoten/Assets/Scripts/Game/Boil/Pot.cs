using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : KitchenwareBase
{
    [SerializeField]
    private GameObject stickUI;

    [SerializeField]
    private GameObject secondHandFrame;


    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject pointText;

    [SerializeField]
    private int POINT;

    private int point;


    private void Awake()
    {
        InstanceMiniGameUI();
        point = 0;
        SetEventManager();
    }

    protected override void InstanceMiniGameUI()
    {
        miniGameUI.SetActive(false);
    }

    protected override void ResetMiniGameUI()
    {
        miniGameUI.SetActive(false);
        stickUI.SetActive(false);
        secondHandFrame.GetComponent<IraIraFrame>().SetStartFlag(false);
        secondHandFrame.GetComponent<IraIraFrame>().InitRotation();
        point = 0;
    }


    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        stickUI.SetActive(true);
        stickUI.GetComponent<Animator>().Play("rSpin");
        stickUI.GetComponent<Animator>().speed = 0.15f;
        point = 0;
    }

    protected override bool Cooking()
    {
        bool result = secondHandFrame.GetComponent<IraIraFrame>().GetOneRotationFlag();
        if (result)
        {
            stickUI.SetActive(false);
            secondHandFrame.GetComponent<IraIraFrame>().SetStartFlag(false);
            secondHandFrame.GetComponent<IraIraFrame>().InitRotation();
        }
        return result;
    }

    public override void CookingInterruption()
    {
        ResetMiniGameUI();
        SetIsCooking(false);
        cuisine = null;
    }

    protected override GameObject SetCuisine() => CuisineManager.Instance.GetPotController().OutputCuisine();

    protected override int CalcEatoyPoint()
    {
        // チュートリアル用
        if (eventManager == null)
        {
            pointText.GetComponent<PointAnnouce>().DisplayText(point.ToString());
        }
        else
        {
            if (eventManager_cs.GetNowPattern() == global::EventManager.FeverPattern.Cooking)
            {
                pointText.GetComponent<PointAnnouce>().DisplayText(point.ToString() + "×1.5倍");
            }
            else
            {
                pointText.GetComponent<PointAnnouce>().DisplayText(point.ToString());
            }
        }
        return point;
    }


    public void JoystickInit(int Id) => joystick.GetComponent<Joystick>().Init(Id);

    public void SetIrairaFrameInFlag(bool b) => secondHandFrame.GetComponent<IraIraFrame>().SetInFlag(b);

    public void AddPoint()
    {
        point += POINT;
        // チュートリアル用
        if (eventManager == null)
        {
            pointText.GetComponent<PointAnnouce>().DisplayText(POINT.ToString());
        }
        else
        {
            if (eventManager_cs.GetNowPattern() == global::EventManager.FeverPattern.Cooking)
            {
                pointText.GetComponent<PointAnnouce>().DisplayText(POINT.ToString() + "×1.5倍");
            }
            else
            {
                pointText.GetComponent<PointAnnouce>().DisplayText(POINT.ToString());
            }
        }
    }

    public void FrameMoveStart()
    {
        secondHandFrame.GetComponent<IraIraFrame>().SetStartFlag(true);
    }
}
