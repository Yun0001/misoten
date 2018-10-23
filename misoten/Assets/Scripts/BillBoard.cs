using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    void Update()
    {
        Vector3 camPosition = Camera.main.transform.position;
        camPosition.x = transform.position.x;
        transform.LookAt(camPosition);
    }

}