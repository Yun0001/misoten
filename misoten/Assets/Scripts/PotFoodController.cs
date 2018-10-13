using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotFoodController : MonoBehaviour
{
    private GameObject[] potFood = new GameObject[CuisineManager.MaxCuisine];
    private GameObject potFoodPrefab;
    private bool[] OutputFlg = new bool[CuisineManager.MaxCuisine];

    // Use this for initialization
    void Awake()
    {
        potFoodPrefab = (GameObject)Resources.Load("Prefabs/PotFood");
        for (int i = 0; i < potFood.Length; i++)
        {
            potFood[i] = Instantiate(potFoodPrefab, transform.position, Quaternion.identity);
            potFood[i].GetComponent<Food>().SetFoodID(i);
            potFood[i].GetComponent<Food>().SetCategory(Food.Category.Microwave);
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
                return potFood[i];
            }
        }
        return null;
    }

    public void OfferCuisine(int ID)
    {
        OutputFlg[ID] = false;
        potFood[ID].GetComponent<Food>().Init();
    }
}
