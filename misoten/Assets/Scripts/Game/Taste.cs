using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//旨味成分クラス
public class Taste : MonoBehaviour
{

    [SerializeField]
    private float Duration; // 持続時間

    public int playerID { get; set; }

	// Update is called once per frame
	void Update ()
    {
        //持続時間を減らす
        Duration -= Time.deltaTime;

        // 持続時間が０以下になれば破壊
        if (Duration <= 0) Destroy(gameObject);
	}
}
