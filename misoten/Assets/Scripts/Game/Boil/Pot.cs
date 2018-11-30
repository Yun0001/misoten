using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : KitchenwareBase
{
    private int basePoint;

    private int missCount;

    [SerializeField]
    private GameObject stickUI;

    [SerializeField]
    private GameObject secondHandFrame;

    [SerializeField]
    private GameObject joystick;


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
        missCount = 0;
        stickUI.SetActive(true);
        stickUI.GetComponent<Animator>().Play("rSpin");
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
        double missPoint = 0.9;
        for (int i = 0; i < missCount; i++)
        {
            missPoint *= 0.9;
        }
        return (int)(basePoint* missPoint);
    }

    public void AddMissCount() => missCount++;

    public void JoystickInit(int Id) => joystick.GetComponent<Joystick>().Init(Id);

}
