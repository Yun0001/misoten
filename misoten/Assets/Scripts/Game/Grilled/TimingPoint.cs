using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;

public class TimingPoint : MonoBehaviour
{
    private readonly Vector3 timingPointScale = new Vector3(0.2f, 0.4f, 1f);
    private readonly float timingPointPositionDifference = -0.2f;

    private bool[] isHit = Enumerable.Repeat(false, 4).ToArray();
    private GameObject[] isHitObj = Enumerable.Repeat<GameObject>(null, 4).ToArray();


    private void Awake()
    {
        transform.localScale = timingPointScale;
        Vector3 pos = transform.position;
        pos.x += timingPointPositionDifference;
        pos.y += 0.08f;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.tag)
        {
            case "GrilledSuccessAreaNormal1":
                isHit[0] = true;
                isHitObj[0] = collision.gameObject;
                break;

            case "GrilledSuccessAreaNormal2":
                isHit[1] = true;
                isHitObj[1] = collision.gameObject;
                break;

            case "GrilledSuccessAreaHard":
                isHit[2] = true;
                isHitObj[2] = collision.gameObject;
                break;

            case "GrilledSuccessAreaHell":
                isHit[3] = true;
                isHitObj[3] = collision.gameObject;
                break;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        switch (collision.tag)
        {
            case "GrilledSuccessAreaNormal1":
                isHit[0] = false;
                isHitObj[0] = null;
                break;

            case "GrilledSuccessAreaNormal2":
                isHit[1] = false;
                isHitObj[1] = null;
                break;

            case "GrilledSuccessAreaHard":
                isHit[2] = false;
                isHitObj[2] = null;
                break;

            case "GrilledSuccessAreaHell":
                isHit[3] = false;
                isHitObj[3] = null;
                break;
        }
    }

    public void SetIsHit(int i, bool flg) => isHit[i] = flg;

    public void ResetHitObj(int i) => isHitObj[i] = null;

    public bool IsHit(int i) => isHit[i];

    public GameObject GetHitObj(int i) => isHitObj[i];
}
