using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CoinFO : MonoBehaviour {

    [SerializeField] private bool isStartingCoinFO = false;
    [SerializeField, Range(1.0f, 5.0f)] private float fadeSpeed = 3.0f;

    private GameObject _coin;

	// Use this for initialization
	void Start () {
        _coin = this.transform.GetChild(0).gameObject;// GameObject.Find("coinFO");
        _coin.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (isStartingCoinFO)
        {
            //this.transform.GetChild(0).gameObject.SetActive(true);
            _coin.SetActive(true);
            _coin.transform.position -= transform.right * fadeSpeed * Time.deltaTime;

            if (_coin.transform.position.x<=-18.5f)
            {
                SceneManager.LoadScene("Scenes/Result");
                //SceneManagerScript.GetInstance().LoadNextScene();
            }
        }

    }
}
