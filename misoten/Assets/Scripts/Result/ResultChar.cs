using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChar : MonoBehaviour {

    private GameObject Chars;
    private Vector3 vec3Char;
    [SerializeField]
    private float posX =0.1f;

    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        Chars = GameObject.Find("chars");
        vec3Char = Chars.transform.position;
   
    }

    // Update is called once per frame
    void Update () {
        MoveChars();
    }

    private void MoveChars()
    {
        vec3Char.x -= posX;
        if (vec3Char.x >= 0)
        //if (Chars.GetComponent<Transform>().position.x < 0f)
        {
            Chars.GetComponent<Transform>().position = vec3Char;
        }
    }




}
