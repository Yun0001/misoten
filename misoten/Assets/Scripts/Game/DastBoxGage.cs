using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DastBoxGage : MonoBehaviour
{
    [SerializeField]
    private float meterIncreaseSpeed;

    private Image gage;

    private void Awake()
    {
        gage = GetComponent<Image>();
    }

    // Use this for initialization
    public void Init(Vector3 pos)
    {
        gage.fillAmount = 0;
        pos.y += 1;
        transform.parent.transform.position = pos;
    }


    /// <summary>
    /// ゲージ増加
    /// </summary>
    public void IncreaseGage()
    {
        // ゲージ増加
        gage.fillAmount += meterIncreaseSpeed / 60;
        if (gage.fillAmount >= 1.0f) gage.fillAmount = 1.0f;
    }

    public float GetAmount() => gage.fillAmount;
}
