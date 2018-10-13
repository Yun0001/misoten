using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilledFoodController : MonoBehaviour {

    private GameObject[] grilledtFood = new GameObject[CuisineManager.MaxCuisine];
    private GameObject grilledFoodPrefab;
    private bool[] OutputFlg = new bool[CuisineManager.MaxCuisine];

    // Use this for initialization
    void Awake()
    {
        grilledFoodPrefab = (GameObject)Resources.Load("Prefabs/PotFood");
        for (int i = 0; i < grilledtFood.Length; i++)
        {
            grilledtFood[i] = Instantiate(grilledFoodPrefab, transform.position, Quaternion.identity);
            grilledtFood[i].GetComponent<Food>().SetFoodID(i);
            grilledtFood[i].GetComponent<Food>().SetCategory(Food.Category.Grilled);
            OutputFlg[i] = false;
        }
    }

    public GameObject OutputCuisine()
    {
        for (int i = 0; i < OutputFlg.Length; i++)
        {
            if (!OutputFlg[i])
            {
                OutputFlg[i] = true;
                return grilledtFood[i];
            }
        }
        return null;
    }

    public void OfferCuisine(int ID)
    {
        OutputFlg[ID] = false;
        grilledtFood[ID].GetComponent<Food>().Init();
    }
}
