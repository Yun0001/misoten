using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveFoodController : MonoBehaviour
{
    private GameObject[] microwaveFood = new GameObject[CuisineManager.MaxCuisine];
    private GameObject microwaveFoodPrefab;
    [SerializeField]
    private bool[] OutputFlg = new bool[CuisineManager.MaxCuisine]; 

	// Use this for initialization
	void Awake ()
    {
        microwaveFoodPrefab = (GameObject)Resources.Load("Prefabs/MicrowaveFood");
        for (int i = 0; i < microwaveFood.Length; i++)
        {
            microwaveFood[i] = Instantiate(microwaveFoodPrefab, transform.position, Quaternion.identity);
            microwaveFood[i].GetComponent<Food>().SetFoodID(i);
            microwaveFood[i].GetComponent<Food>().SetCategory(Food.Category.Microwave);
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
                return microwaveFood[i];
            }
        }
        return null;
    }

    public void OfferCuisine(int ID)
    {
        OutputFlg[ID] = false;
        microwaveFood[ID].GetComponent<Food>().Init();
    }
}
