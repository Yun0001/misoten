using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour {

    
    /// <summary>
    /// 移動スピード
    /// </summary>
    private float speed { get; set; }
    private float moveX = 0f;
    private float moveZ = 0f;
    private Rigidbody rb;
    string layerName;

    // Use this for initialization
    void Start () {
        speed = 20f;
        rb = GetComponent<Rigidbody>();
        layerName = LayerMask.LayerToName(gameObject.layer);
    }
	
	// Update is called once per frame
	void Update () {
        GamepadState padState = GamepadInput.GamePad.GetState(GamePad.Index.One);
        string InputXAxisName = "L_XAxis_1";
        string InputYAxisName = "L_XAxis_1";
        switch (layerName)
        {
            case "Player1":
                InputXAxisName = "L_XAxis_1";
                InputYAxisName = "L_YAxis_1";
                break;
            case "Player2":
                InputXAxisName = "L_XAxis_2";
                InputYAxisName = "L_YAxis_2";
                break;
            case "Player3":
                InputXAxisName = "L_XAxis_3";
                InputYAxisName = "L_YAxis_3";
                break;
            case "Player4":
                InputXAxisName = "L_XAxis_4";
                InputYAxisName = "L_YAxis_4";
                break;
        }
        moveX = Input.GetAxis(InputXAxisName);
        moveZ = -(Input.GetAxis(InputYAxisName));
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveX*speed, 0, moveZ * speed);
    }
}
