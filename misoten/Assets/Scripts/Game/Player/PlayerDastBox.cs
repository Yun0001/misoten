using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDastBox : MonoBehaviour {

    private Player player_cs;
    private GameObject dastBoxGage;

    private void Awake()
    {
        player_cs = GetComponent<Player>();
        dastBoxGage = Instantiate(Resources.Load("Prefabs/DastBoxUI") as GameObject, transform.position, Quaternion.identity, transform);
        dastBoxGage.SetActive(false);
    }

    public void AccessDastBox()
    {
        dastBoxGage.SetActive(true);
        dastBoxGage.GetComponent<DastBox>().Access((int)player_cs.GetPlayerStatus(), transform.position);
        player_cs.SetPlayerStatus(Player.PlayerStatus.DastBox);
    }

    public void UpdateDastBox()
    {
        // ゴミ箱状態の時の入力
        player_cs.GetPlayerInput().InputDastBox();

        //　ゴミ箱ゲージがMaxの時
        if (dastBoxGage.GetComponent<DastBox>().GetGageAmount() >= 1.0f)
        {
            player_cs.IsObjectCollision(PlayerCollision.hitObjName.DastBox).transform.Find("box").GetComponent<mwAnimCtrl>().SetIsOpen(true);
            player_cs.SetPlayerStatus(Player.PlayerStatus.Normal);
            player_cs.RevocationHaveInEatoy(true);
            GetComponent<PlayerAnimCtrl>().SetServing(false);
            GetDastBoxUI().SetActive(false);
            Sound.SetVolumeSe(SoundController.GetGameSEName(SoundController.GameSE.Dustshoot), 0.3f, 8);
            Sound.PlaySe(SoundController.GetGameSEName(SoundController.GameSE.Dustshoot), 8);

        }
    }

    public GameObject GetDastBoxUI() => dastBoxGage;
}
