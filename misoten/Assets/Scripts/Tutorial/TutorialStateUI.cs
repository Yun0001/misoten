using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStateUI : MonoBehaviour
{
    private GameObject[] _players;
    [SerializeField] private Sprite _skip;
    [SerializeField] private Sprite _complete;

    private MenuAnimCtrl _menuAnimCtrl;

    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");

        _menuAnimCtrl = GameObject.Find("OBJ_menu").GetComponent<MenuAnimCtrl>();
    }

    void Update()
    {
        foreach (GameObject player in _players)
        {
            int i = player.GetComponent<Player>().GetPlayerID();

            bool isSkip = player.GetComponent<TutorialPlayer>().IsSkip();
            if (isSkip)
            {
                this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = _skip;
            }

            bool isComplete = player.GetComponent<TutorialPlayer>().IsComplete();
            if (isComplete)
            {
                this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = _complete;
            }

            if ((!isSkip)&&(!isComplete))
            {
                this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        // 非表示時
        if (_menuAnimCtrl.IsOverPage())
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            }
        }

    }

}