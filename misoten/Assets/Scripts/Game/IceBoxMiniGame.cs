using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoxMiniGame : MonoBehaviour {

    [SerializeField]
    private int takeCount;

    [SerializeField]
    private int addBarragePoint;

    [SerializeField]
    private float playerBarrage = 0;

    [SerializeField]
    private float freesResist = 0f;

    [SerializeField]
    private GameObject IceBox;

    [SerializeField]
    private bool isStart = false;

	// Use this for initialization
	void Start () {
		
	}

    public void Init()
    {
        isStart = false;
        playerBarrage = 0;
        freesResist = 0f;
    }

    public void flgOn() => isStart = true;

	// Update is called once per frame
	void Update () {
        if(isStart)
        {
            freesResist += 0.1f;
        }
	}

    public bool AddPlayerBarrage()
    {
        playerBarrage += addBarragePoint;
        return IsOverTakeCount();
    }

    private bool IsOverTakeCount() => (playerBarrage - freesResist) >= takeCount;
}
