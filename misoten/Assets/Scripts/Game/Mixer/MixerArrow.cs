using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerArrow : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed;

    
    private void Awake()
    {
        rotationSpeed *= -1;
    }


    public void ReverseDirection()
    {
        rotationSpeed *= -1;
        Vector3 scale = new Vector3(-1.5f, 1.5f, 1.5f);
        transform.localScale = scale;
    }
    public void Rotation()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}