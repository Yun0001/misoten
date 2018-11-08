using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : KitchenwareBase
{
    private GameObject frame;


    private void Awake()
    {
        InstanceMiniGameUI();
    }

    protected override void InstanceMiniGameUI()
    {
        miniGameUI = Instantiate(Resources.Load("Prefabs/PotMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = canvasCamera.GetComponent<Camera>();
        frame = miniGameUI.transform.Find("Canvas/JoyStick/SecondHandFrame").gameObject;
        miniGameUI.SetActive(false);
    }

    protected override void ResetMiniGameUI()
    {
        miniGameUI.SetActive(false);
    }


    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        miniGameUI.transform.Find("Canvas/JoyStick").GetComponent<Joystick>().Init(0);
    }

    protected override bool Cooking()
    {
        return frame.GetComponent<IraIraFrame>().GetOneRotationFlag();
    }

    public override void CookingInterruption()
    {
        ResetMiniGameUI();
        SetIsCooking(false);
        CuisineManager.GetInstance().GetPotController().OfferCuisine(cuisine.GetComponent<Food>().GetFoodID());
        cuisine = null;
    }

    protected override GameObject SetCuisine() => CuisineManager.GetInstance().GetPotController().OutputCuisine();
}
