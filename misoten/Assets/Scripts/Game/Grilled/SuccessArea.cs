using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessArea : MonoBehaviour
{
    [SerializeField]
    private Mesh myMesh;
    public float moveUV = 0;
    public float moveSpeed=0.01f;

    public Vector2[] uv = new Vector2[4];

    public bool isInGageFrame = false;
    public bool isOutGageFrame = false;

    private const float transparent_U = 0.5f;
    private const float color_U = 1 - transparent_U;

    // Use this for initialization
    void Awake () {
        myMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] nUV = { new Vector2(0.6f, 0.0f), new Vector2(0.9f, 0.0f), new Vector2(0.9f, 1.0f), new Vector2(0.6f, 1.0f) };
        uv = nUV;
        myMesh.uv = nUV;
        moveSpeed = 0.01f;
    }

    public void Init()
    {
        ResetMoveUV();
        Vector2[] nUV = { new Vector2(0.6f, 0.0f), new Vector2(0.9f, 0.0f), new Vector2(0.9f, 1.0f), new Vector2(0.6f, 1.0f) };
        uv = nUV;

        myMesh.uv = nUV;
    }

    public void SetMoveSpeed(float speed) => moveSpeed = speed;

    // Update is called once per frame
    void Update ()
    {
        // ゲージの中に入った時の処理
        if (isInGageFrame)
        {
            if (moveUV <= transparent_U)
            {
                moveUV += transparent_U / (transform.localScale.x / moveSpeed);
                Vector2[] nUV = myMesh.uv;
                for (int i = 0; i < myMesh.uv.Length; i++)
                {
                    nUV[i].x -= transparent_U / (transform.localScale.x / moveSpeed);
                }
                myMesh.uv = nUV;
                uv = nUV;

            }
        }
        // ゲージの外に出た時の処理
        else if (isOutGageFrame)
        {
            if (moveUV <= color_U)
            {
                moveUV += color_U / (transform.localScale.x / moveSpeed);
                Vector2[] nUV = myMesh.uv;
                for (int i = 0; i < myMesh.uv.Length; i++)
                {
                    nUV[i].x += color_U / (transform.localScale.x / moveSpeed);
                }
                myMesh.uv = nUV;
                uv = nUV;
            }
            if (moveUV >= color_U)
            {
                if (tag == "GrilledSuccessAreaNormal1"|| tag == "GrilledSuccessAreaNormal2")
                {
                    gameObject.transform.parent.gameObject.SetActive(false);
                    GameObject a= gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal1").gameObject;
                    //a.SetActive(false);
                    GameObject b = gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal2").gameObject;
                    //b.SetActive(false);
                    gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal1").gameObject.GetComponent<SuccessArea>().Init();
                    gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal2").gameObject.GetComponent<SuccessArea>().Init();
                    gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal1").gameObject.GetComponent<SuccessArea>().isOutGageFrame = false;
                    gameObject.transform.parent.gameObject.transform.Find("SuccessArea_Normal2").gameObject.GetComponent<SuccessArea>().isOutGageFrame = false;


                }
                else
                {
                    Init();
                    gameObject.SetActive(false);
                    isOutGageFrame = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゲージの枠と会当たった時の処理
        if (collision.tag == "GrilledGage")
        {
            isInGageFrame = true;
        }
        if (collision.tag == "OutsideGrilledGage")
        {
            isInGageFrame = false;
            isOutGageFrame = true;
            ChangeUV();
            ResetMoveUV();
        }
    }

    private void ChangeUV()
    {
        Vector2[] nUV = { new Vector2(0.4f, 0.0f), new Vector2(0.1f, 0.0f), new Vector2(0.1f, 1.0f), new Vector2(0.4f, 1.0f) };
        uv = nUV;

        myMesh.uv = nUV;
    }

    private void ResetMoveUV() => moveUV = 0;
}
