using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System;

public class Microwave : KitchenwareBase
{
    MicrowaveGage microwaveGage_cs;

	// Use this for initialization
	void Awake () {
        InstanceMiniGameUI();
        microwaveGage_cs = miniGameUI.GetComponent<MicrowaveGage>();
        miniGameUI.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = canvasCamera.GetComponent<Camera>();
    }

    protected override void InstanceMiniGameUI()
    {
        miniGameUI = Instantiate(Resources.Load("Prefabs/MicrowaveMiniGame") as GameObject, transform.position, Quaternion.identity);
        miniGameUI.SetActive(false);
    }

    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        microwaveGage_cs.ResetMicrowaveGage();  // ゲージの状態をリセット
    }

    protected override void ResetMiniGameUI()
    {
        miniGameUI.SetActive(false);
    }


    protected override bool Cooking()
    {
        return microwaveGage_cs.UpdateMicrowaveGage();
    }


    protected override GameObject SetCuisine() => CuisineManager.GetInstance().GetMicrowaveController().OutputCuisine();

    public override void CookingInterruption()
    {
        ResetMiniGameUI();

        CuisineManager.GetInstance().GetGrilledController().OfferCuisine(cuisine.GetComponent<Food>().GetFoodID());
        cuisine = null;
    }

    public void AddCuisinePoint(int point) => cuisine.GetComponent<Food>().AddQualityTaste(point);

    public void DecisionCheckClockCollision()
    {
        microwaveGage_cs.DecisionCheckClockCollision();
    }
}
