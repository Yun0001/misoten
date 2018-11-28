using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : KitchenwareBase
{
    private GameObject frame;

    private int basePoint;

    private int missCount;

    [SerializeField]
    private GameObject stickUI;


    private void Awake()
    {
        InstanceMiniGameUI();
    }

    protected override void InstanceMiniGameUI()
    {
       // miniGameUI = Instantiate(Resources.Load("Prefabs/PotMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = canvasCamera.GetComponent<Camera>();
        frame = miniGameUI.transform.Find("Canvas/JoyStick/SecondHandFrame").gameObject;
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
        bool result = frame.GetComponent<IraIraFrame>().GetOneRotationFlag();
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

    public void JoystickInit(int Id)
    {
        miniGameUI.transform.Find("Canvas/JoyStick").GetComponent<Joystick>().Init(Id);

    }
}
