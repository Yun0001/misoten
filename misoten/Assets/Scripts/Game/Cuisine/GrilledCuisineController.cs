using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 焼き料理コントローラー
/// </summary>
public class GrilledCuisineController : CuisineControllerBase
{
    // 初期処理
    void Awake()
    {
        cuisinePrefab = Resources.Load("Prefabs/GrilledFood") as GameObject;
        Init(Food.Category.Grilled);
    }
}
