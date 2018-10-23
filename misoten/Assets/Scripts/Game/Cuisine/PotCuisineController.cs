using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鍋料理コントローラー
/// </summary>
public class PotCuisineController : CuisineControllerBase
{
    // 初期処理
    void Awake()
    {
        cuisinePrefab = Resources.Load("Prefabs/PotFood") as GameObject;
        Init(Food.Category.Pot);
    }
}
