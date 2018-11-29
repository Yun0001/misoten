using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System;

public class CookWareMw : KitchenwareBase
{
    [SerializeField]
    MicrowaveGage microwaveGage_cs;

    private int chain;

    private int[] eatoyPoint = new int[2];

    [SerializeField]
    private GameObject UIButton;

    [SerializeField]
    private float basePoint;

    // Use this for initialization
    void Awake()
    {
        InstanceMiniGameUI();
        microwaveGage_cs = miniGameUI.GetComponent<MicrowaveGage>();
        miniGameUI.GetComponent<MicrowaveGage>().SetCheckClockInMicrowave_cs(GetComponent<CookWareMw>());
        miniGameUI.SetActive(false);
    }

    protected override void InstanceMiniGameUI()
    {
       //' miniGameUI = Instantiate(Resources.Load("Prefabs/MicrowaveMiniGame") as GameObject, transform.position, Quaternion.identity);
        //miniGameUI.transform.Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = canvasCamera.GetComponent<Camera>();
    }

    protected override void InitMiniGameUI()
    {
        miniGameUI.SetActive(true);
        microwaveGage_cs.ResetMicrowaveGage();  // ゲージの状態をリセット
        ResetChain();
        for (int i = 0; i < eatoyPoint.Length; i++)
        {
            eatoyPoint[i] = 0;
        }

        UIButton.SetActive(true);
    }

    protected override void ResetMiniGameUI()
    {
        miniGameUI.SetActive(false);
    }


    protected override bool Cooking()
    {
        bool result = microwaveGage_cs.UpdateMicrowaveGage();
        if (result)
        {
            UIButton.SetActive(false);
        }
        return result;
    }


    protected override GameObject SetCuisine() => CuisineManager.GetInstance().GetMicrowaveController().OutputCuisine();

    public override void CookingInterruption()
    {
        ResetMiniGameUI();
        SetIsCooking(false);
        cuisine = null;
        microwaveGage_cs.StopStartSE();
        // レンジOpenSE
        Sound.PlaySe(GameSceneManager.seKey[16], 4);
        UIButton.SetActive(false);
    }

    protected override int CalcEatoyPoint()
    {
        return (int)((basePoint + chain * 0.25f) * eatoyPoint[0] + eatoyPoint[1]);
    }

    public void AddEatoyPoint(int e, int point) => eatoyPoint[e] += point;

    public void AddChain() => chain++;

    public void ResetChain() => chain = 1;

    public void DecisionCheckClockCollision()
    {
        microwaveGage_cs.DecisionCheckClockCollision();
    }
}
