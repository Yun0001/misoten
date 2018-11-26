using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constants;

public class TutorialFrow : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tutorialFrow;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetTutorial(int num, bool b) => _tutorialFrow[num].SetActive(b);

}
