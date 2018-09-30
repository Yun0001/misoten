using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 食材クラス
/// </summary>
public class Food : MonoBehaviour {

    /// <summary>
    /// この食材の所有プレイヤーID
    /// </summary>
    private int ownershipPlayerID;

	// Use this for initialization
	void Start () {
        //　テスト用
        ownershipPlayerID = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOwnershipPlayerID(int ID)
    {
        ownershipPlayerID = ID;
    }

    public int GetOwnershipPlayerID()
    {
        return ownershipPlayerID;
    }
}
