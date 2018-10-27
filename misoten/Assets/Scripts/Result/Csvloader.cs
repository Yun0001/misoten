using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Csvloader : MonoBehaviour {

    // ランキングデータ
    private List<string[]> rankDatas = new List<string[]>();
 
    // Use this for initialization
    void Start () {
     
        // csvファイル名
        var fileName = "rank";
        // Resourcesのcsvフォルダ内のcsvファイルをTextAssetとして取得
        var csvFile = Resources.Load("Csv/" + fileName) as TextAsset;
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
 
        // ログに読み込んだデータを表示する
        foreach(var data in rankDatas) {
            Debug.Log("DATA:" + data[0] + " / " + data[1] + " / " + data[2]);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
