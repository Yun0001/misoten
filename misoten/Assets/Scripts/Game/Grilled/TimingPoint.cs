using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class TimingPoint : MonoBehaviour
{
    private GamePad.Index playerNumber;
    [SerializeField]
    private Grilled grilled_cs;

    private readonly Vector3 timingPointScale = new Vector3(0.2f, 0.4f, 1f);
    private readonly float timingPointPositionDifference = -0.4f;

    private void Awake()
    {
        transform.localScale = timingPointScale;
        Vector3 pos = transform.position;
        pos.x += timingPointPositionDifference;
        transform.position = pos;
    }

    // Use this for initialization
    public void Init(Grilled cs) => grilled_cs = cs;

    public void SetPlayerNumber(GamePad.Index pNumber) => playerNumber = pNumber;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GamePad.GetButtonDown(GamePad.Button.B, playerNumber))
        {
            if (collision.tag == "SuccessArea")
            {
                //ポイント加算
                grilled_cs.GetGrilledCuisine().GetComponent<Food>().AddQualityTaste(collision.GetComponent<GrilledPoint>().GetPoint());
                collision.gameObject.SetActive(false);
            }
            else if (collision.tag == "SuccessAreaChild")
            {
                grilled_cs.GetGrilledCuisine().GetComponent<Food>().AddQualityTaste(collision.GetComponent<GrilledPoint>().GetPoint());
                collision.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
