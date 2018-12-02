using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : KitchenwareBase
{
    [SerializeField]
    private int basePoint;

    [SerializeField]
    private int missCount;

    [SerializeField]
    private GameObject stickUI;

    [SerializeField]
    private GameObject secondHandFrame;


    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject pointText;


    private void Awake()
    {
        InstanceMiniGameUI();
    }

    protected override void InstanceMiniGameUI()
    {
        miniGameUI.SetActive(false);
    }

    protected override void ResetMiniGameUI()
    {
        miniGameUI.SetActive(false);
        stickUI.SetActive(false);
    }


    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        basePoint = 50;
        missCount = -1;
        stickUI.SetActive(true);
        stickUI.GetComponent<Animator>().Play("rSpin");
        stickUI.GetComponent<Animator>().speed = 0.15f;
    }

    protected override bool Cooking()
    {
        bool result = secondHandFrame.GetComponent<IraIraFrame>().GetOneRotationFlag();
        if (result)
        {
            stickUI.SetActive(false);
        }
        return result;
    }

    public override void CookingInterruption()
    {
        ResetMiniGameUI();
        SetIsCooking(false);
        cuisine = null;
    }

    protected override GameObject SetCuisine() => CuisineManager.GetInstance().GetPotController().OutputCuisine();

    protected override int CalcEatoyPoint()
    {
        double missPoint =1;
        for (int i = 0; i < missCount; i++)
        {
            missPoint *= 0.9;
        }
        int point = (int)(basePoint * missPoint);
        pointText.GetComponent<PointAnnouce>().DisplayText(point);
        return point;
    }

    public void AddMissCount() => missCount++;

    public void JoystickInit(int Id) => joystick.GetComponent<Joystick>().Init(Id);

    public void SetIrairaFrameInFlag(bool b) => secondHandFrame.GetComponent<IraIraFrame>().SetInFlag(b);
}
