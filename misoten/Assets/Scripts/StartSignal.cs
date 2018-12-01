using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSignal : MonoBehaviour {

    private float StartTimeframe = 10f;
    private bool one = true;
    [SerializeField]
    private GameObject Start1;
    [SerializeField]
    private GameObject Start2;
    [SerializeField]
    private GameObject Start3;
    [SerializeField]
    private GameObject GameStart;
	[SerializeField]
	private GameObject pauseObj;

	private void Awake()
	{
		pauseObj.GetComponent<Pause>().pausing = true;
	}

	// Use this for initialization
	void Start () {
        Start1.SetActive(false);
        Start2.SetActive(false);
        Start3.SetActive(false);
        GameStart.SetActive(false);

        Sound.LoadSe("StartCount", "GamePlay/Countdown");
        Sound.LoadSe("Start", "GamePlay/Endannouncement");
	}
	
	// Update is called once per frame
	void Update () {


        // スタートカウントダウン開始
        if (one)
        {
            StartCoroutine(CountdownCoroutine());
            one = false;
        }
    
	}


    IEnumerator CountdownCoroutine()
    {

        Start3.SetActive(true);
        Sound.PlaySe("StartCount", 1);
        yield return new WaitForSeconds(1.0f);

        Start3.SetActive(false);
        Start2.SetActive(true);
        Sound.PlaySe("StartCount", 1);
        yield return new WaitForSeconds(1.0f);

        Start2.SetActive(false);
        Start1.SetActive(true);
        Sound.PlaySe("StartCount", 1);
        yield return new WaitForSeconds(1.0f);

        Start1.SetActive(false);
        GameStart.SetActive(true);
        Sound.PlaySe("Start", 1);
        yield return new WaitForSeconds(1.0f);

        GameStart.SetActive(false);
		pauseObj.GetComponent<Pause>().pausing = false;
	}


}
