using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class Csvloader : MonoBehaviour {

    // ランキングデータ
    private List<string[]> rankDatas = new List<string[]>();

    private int test;
    private int ranking;
    private int rankCount;
    private int scoreData;

    private TotalSales TotalScore;
    private GameObject scoreManager;

    private GameObject rankIN;
    private GameObject rank1;
    private GameObject rank2;
    private GameObject rank3;
    private GameObject rank4;
    private GameObject rank5;
    private GameObject rank6;
    private GameObject rank7;
    private GameObject rank8;
    private GameObject rank9;
    private GameObject rank10;
    

    // Use this for initialization
    void Start () {
        rankCount = 1;

        // csvファイル名
        var fileName = "rank";
        // Resourcesのcsvフォルダ内のcsvファイルをTextAssetとして取得
        var csvFile = Resources.Load("csv/" + fileName) as TextAsset;
        // csvファイルの内容をStringReaderに変換
        var reader = new StringReader(csvFile.text);
 
        // csvファイルの内容を一行ずつ末尾まで取得しリストを作成
        while(reader.Peek() > -1) {
            // 一行読み込む
            var lineData = reader.ReadLine();
            // カンマ(,)区切りのデータを文字列の配列に変換
            var address = lineData.Split (',');
            // リストに追加
            rankDatas.Add(address);
            // 末尾まで繰り返し...
        }


        // 現在のフォルダにsaveData.csvを出力する(決まった場所に出力したい場合は絶対パスを指定してください)
        // 引数説明：第1引数→ファイル出力先, 第2引数→ファイルに追記(true)or上書き(false), 第3引数→エンコード
        //StreamWriter sw = new StreamWriter("Assets/Resources/csv/rankData.csv", false, Encoding.GetEncoding("Shift_JIS"));
        //for (int i = 0; i < 3; i++)
        //{
        //    //if(i!=1)continue;
        //    string[] str = { "tatsu", "" + (i + 1) };
        //    string str2 = string.Join(",", str);
        //    sw.WriteLine(str2);
        //    Debug.Log("DATA:" + str2);
        //}
        //// StreamWriterを閉じる
        //sw.Close();

        scoreManager = GameObject.Find("CanvasResult/totalchip");
        TotalScore = scoreManager.GetComponent<TotalSales>();
        scoreData = TotalScore.GetMaxScore();   //現在のスコア
        // ログに読み込んだデータを表示する
        foreach(var data in rankDatas) {
            //Debug.Log("DATA:" + data[0] + " / " + data[1] + " / " + data[2]);
            //Debug.Log("DATA:" + data[2]);


            ranking = int.Parse(data[0]);  
            test = int.Parse(data[2]);           
            //data[2] = int.Parse("123");
            if (test >= scoreData)
            {
                //Debug.Log("DATA:" + test);
                rankCount++;
            }
            DebugText(ranking);
        }
        
        //Top10ランクイン
        if (rankCount < 10)
        {
            Debug.Log(rankCount+"ランクイン"+scoreData);
            rankIN.GetComponent<Text>().text = "ランクイン";
             
        }
        else
        {
            Debug.Log("ランク外");
            rankIN.GetComponent<Text>().text = "ランク外";
             
        }
    }


    private void Awake()
    {
        rankIN = GameObject.Find("rankINText");
        rank1 = GameObject.Find("rank1");
        rank2 = GameObject.Find("rank2");
        rank3 = GameObject.Find("rank3");
        rank4 = GameObject.Find("rank4");
        rank5 = GameObject.Find("rank5");
        rank6 = GameObject.Find("rank6");
        rank7 = GameObject.Find("rank7");
        rank8 = GameObject.Find("rank8");
        rank9 = GameObject.Find("rank9");
        rank10 = GameObject.Find("rank10");

    }

	// Update is called once per frame
	void Update () {
		
	}


    private void DebugText(int rank)
    {
        switch(rank){
            case 1:
                rank1.GetComponent<Text>().text = rank + "rank :" + test.ToString();
                break;
            case 2:
                rank2.GetComponent<Text>().text = rank + "rank :" + test.ToString();
               break;
            case 3:
                rank3.GetComponent<Text>().text = rank + "rank :" + test.ToString();
                break;
            case 4:
                rank4.GetComponent<Text>().text = rank + "rank :" + test.ToString();
               break;
            case 5:
                rank5.GetComponent<Text>().text = rank + "rank :" + test.ToString();
               break;
            case 6:
                rank6.GetComponent<Text>().text = rank + "rank :" + test.ToString();
               break;
            case 7:
                rank7.GetComponent<Text>().text = rank + "rank :" + test.ToString();
                break;
            case 8:
                rank8.GetComponent<Text>().text = rank + "rank :" + test.ToString();
              break;
            case 9:
                rank9.GetComponent<Text>().text = rank + "rank :" + test.ToString();
                 break;
            case 10:
                rank10.GetComponent<Text>().text = rank + "rank :" + test.ToString();
            break;
            default:
            Debug.Log("rank例外処理が呼ばれました");
            break;

        }
     }



}
