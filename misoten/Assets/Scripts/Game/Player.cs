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
    private string layerName;
    private string InputXAxisName;
    private string InputYAxisName;
    GamepadState padState;
    GamePad.Index PlayerNumber;

    // Use this for initialization
    void Start () {
        speed = 3f;
        rb = GetComponent<Rigidbody>();
        layerName = LayerMask.LayerToName(gameObject.layer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!PlayerNumberDecision())    return;

        moveX = 0;
        moveZ = 0;
        padState = GamepadInput.GamePad.GetState(PlayerNumber);
        switch (layerName)
        {
            case "Player1":


                break;
            case "Player2":
                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -speed;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveX = speed;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    moveZ = speed;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveZ = -speed;
                }
                break;
            case "Player3":
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    moveX = -speed;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveX = speed;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    moveZ = speed;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    moveZ = -speed;
                }
                break;
            case "Player4":

                break;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveX*speed, 0, moveZ * speed);
    }

    private bool PlayerNumberDecision()
    {
        switch (layerName)
        {
            case "Player1":
                PlayerNumber = GamePad.Index.One;
                InputXAxisName = "L_XAxis_1";
                InputYAxisName = "L_YAxis_1";
                return true;
            case "Player2":
                PlayerNumber = GamePad.Index.Two;
                InputXAxisName = "L_XAxis_2";
                InputYAxisName = "L_YAxis_2";
                return true;
            case "Player3":
                PlayerNumber = GamePad.Index.Three;
                InputXAxisName = "L_XAxis_3";
                InputYAxisName = "L_YAxis_3";
                return true;
            case "Player4":
                PlayerNumber = GamePad.Index.Four;
                InputXAxisName = "L_XAxis_4";
                InputYAxisName = "L_YAxis_4";
                return true;
        }
        return false;
    }
}
