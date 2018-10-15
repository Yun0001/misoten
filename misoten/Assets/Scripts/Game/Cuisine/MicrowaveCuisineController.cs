using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レンチン料理コントローラー
/// </summary>
public class MicrowaveCuisineController : CuisineControllerBase
{ 
	// 初期処理
	void Awake ()
    {
        cuisinePrefab = Resources.Load("Prefabs/MicrowaveFood") as GameObject;
        Init(Food.Category.Microwave);
    }
}
