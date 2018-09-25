using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class UnityChanMove : MonoBehaviour {

    enum status
    {
        wait,
        walk,
        jump
    }

    [SerializeField]
    private Animator animator { get; set; }

    [SerializeField]
    private float speed { get; set; }

    private const float rotationSpeed = 3.0f;

    [SerializeField]
    private status state { get; set; }


    // 初期処理
    void Start() {
        // Animator
        animator = GetComponent<Animator>();
        state = status.wait;
        speed = 0.05f;
    }

    // 更新処理
    void Update()
    {
        GamepadState padState = GamepadInput.GamePad.GetState(GamePad.Index.One);

        if (GamepadInput.GamePad.GetButton(GamePad.Button.A, GamePad.Index.One))
        {
            transform.Rotate(0, rotationSpeed, 0);
        }

        if (!animator.GetBool("isJump"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += transform.forward * speed;
                animator.SetBool("isRunnig", true);
            }
            else
            {
                animator.SetBool("isRunnig", false);
            }
        }
   

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -rotationSpeed, 0);
        }

        if (Input.GetKey(KeyCode.C))
        {
            animator.SetBool("isJump", true);
        }

        Debug.Log(Input.GetAxis("L_XAxis_1"));

 


    }
}
