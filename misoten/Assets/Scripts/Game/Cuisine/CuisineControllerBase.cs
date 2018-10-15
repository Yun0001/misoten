using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 料理コントローラー親クラス（各料理コントローラーはこれを継承）
/// </summary>
public class CuisineControllerBase : MonoBehaviour
{
    /// <summary>
    /// 各料理の最大数
    /// </summary>
    private readonly static int maxCuisine = 4;

    /// <summary>
    /// 料理プレハブ
    /// </summary>
    protected GameObject cuisinePrefab;

    /// <summary>
    /// 料理
    /// </summary>
    protected GameObject[] cuisine = new GameObject[maxCuisine];

    /// <summary>
    /// 料理が配膳中か
    /// </summary>
    protected bool[] isCateringCuisine = new bool[maxCuisine];

    protected void Init(Food.Category category)
    {
        for (int i = 0; i < cuisine.Length; i++)
        {
            cuisine[i] = Instantiate(cuisinePrefab, transform.position, Quaternion.identity);
            cuisine[i].GetComponent<Food>().SetFoodID(i);
            cuisine[i].GetComponent<Food>().SetCategory(category);
            isCateringCuisine[i] = false;
        }
    }

    /// <summary>
    /// 調理終了時にプレイヤーに料理を渡す
    /// </summary>
    /// <returns>料理</returns>
    public GameObject OutputCuisine()
    {
        for (int i = 0; i < isCateringCuisine.Length; i++)
        {
            if (!isCateringCuisine[i])
            {
                // 料理を返却
                isCateringCuisine[i] = true;
                return cuisine[i];
            }
        }
        // 料理がすでに４つ配膳中であらばnullを返却
        Debug.Log("これ以上料理を出せません");
        return null;
    }

    /// <summary>
    /// 配膳終了時に新たに料理を作成できるようにする
    /// </summary>
    /// <param name="ID">料理のID</param>
    public void OfferCuisine(int id)
    {
        if (id < 0 || id > 3)
        {
            Debug.Log("不正な料理ID");
            return;
        }

        isCateringCuisine[id] = false;
        cuisine[id].GetComponent<Food>().Init();
    }
}
