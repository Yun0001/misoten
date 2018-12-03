using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CoinFO : MonoBehaviour {

    private GameTimeManager _gameTimeManager;
    private GameObject[] _gameUIs;

    [SerializeField] private bool isStartingCoinFO = false;
    [SerializeField, Range(1.0f, 5.0f)] private float fadeSpeed = 3.0f;

    private GameObject _coin;

    // Use this for initialization
    void Start()
    {
        _gameUIs = GameObject.FindGameObjectsWithTag("GameUI");

        _gameTimeManager = GameObject.Find("TimeManager").gameObject.GetComponent<GameTimeManager>();
        _coin = this.transform.GetChild(0).gameObject;
        _coin.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (_gameTimeManager.IsTimeUp() && _gameTimeManager.GetIsTimeUp())
        {
            isStartingCoinFO = true;
            foreach (GameObject _gameUI in _gameUIs)
            {
                _gameUI.SetActive(false);
            }
        }

        if (isStartingCoinFO)
        {
            //this.transform.GetChild(0).gameObject.SetActive(true);
            _coin.SetActive(true);
            _coin.transform.position -= transform.right * fadeSpeed * Time.deltaTime;

            if (_coin.transform.position.x<=-18.5f)
            {
                SceneManagerScript.LoadNextScene();
                //SceneManager.LoadScene("Scenes/Result");
                //SceneManagerScript.GetInstance().LoadNextScene();
            }
        }

    }

	public bool GetIsStartingCoinFO() => isStartingCoinFO;
}
