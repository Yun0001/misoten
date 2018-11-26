using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerArrow : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed;

    private float rSpeed;

    

    public void Init()
    {
        rSpeed = -rotationSpeed;
        Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.localScale = scale;
    }


    public void ReverseDirection()
    {
        rSpeed = rotationSpeed;
        Vector3 scale = new Vector3(-1.5f, 1.5f, 1.5f);
        transform.localScale = scale;
    }
    public void Rotation()
    {
        transform.Rotate(new Vector3(0, 0, rSpeed));
    }
}