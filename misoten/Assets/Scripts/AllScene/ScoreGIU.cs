
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;

//public class ScoreGUI : MonoBehaviour
//{

//    //public GameObject player;
//    public Text scoreText;
//    private int score = 0;
//    //********** 開始 **********//
//    private int scoreUpPos = 3;
//    //********** 終了 **********//
//    private Transform playerTrans;

//    void Start()
//    {
//        //playerTrans = player.GetComponent<Transform>();
//        scoreText.text = "Score: 0";
//    }

//    void Update()
//    {
//        //float playerHeight = playerTrans.position.y;
//        //float currentCameraHeight = transform.position.y;
//        //float newHeight = Mathf.Lerp(currentCameraHeight, playerHeight, 0.5f);
//        //if (playerHeight > currentCameraHeight)
//        //{
//        //    transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
//        //}
//        //********** 開始 **********//
//        //scoreUpPosを超えた時
//        //if (playerTrans.position.y >= scoreUpPos)
//        //{
//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            //scoreUpPos += 3; //scoreUpPosを高くする
//            score += 10; //scoreに加算
//                         //スコアを更新して表示
//            scoreText.text = "Score: " + score.ToString();
//        }
//    }
//    //********** 終了 **********//
//}
