using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMeteor : MonoBehaviour {
    [SerializeField, Range(1.0f, 2.0f)] float meteorSpeed = 1.5f;
    private Vector3 startCoord;
    private Vector3 targetCoord = new Vector3(0, 0, 0);
   
    void Start () {
        startCoord = this.transform.localPosition;
    }

    void Update()
    {

        if (this.transform.localPosition.x <= 2.0f)
        {
            this.transform.localPosition = startCoord;
        }
        else
        {
            this.transform.Rotate(new Vector3(0, 0, 10));
            float t = Time.deltaTime * meteorSpeed;
            this.transform.localPosition = Vector3.Slerp(this.transform.localPosition, targetCoord, t);
        }

    }

}
