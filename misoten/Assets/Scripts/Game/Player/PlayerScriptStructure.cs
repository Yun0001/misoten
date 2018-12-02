using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptStructure
{
    private PlayerStateBase state_cs;
    private PlayerMove move_cs;
    private PlayerCollision collision_cs;
    private PlayerAccessController accessController_cs;
    private PlayerAccessPossiblAnnounce accessPosssibleAnnounce_cs;
    private PlayerHaveInEatoy haveInEatoy_cs;
    private HaveEatoyCtrl haveEatoyCtrl_cs;

    public PlayerScriptStructure(GameObject player, PlayerStateBase statebase, PlayerMove move, PlayerCollision collision,
        PlayerAccessController accesscontroller, PlayerAccessPossiblAnnounce possibleannounce,
        PlayerHaveInEatoy haveineatoy, HaveEatoyCtrl haveeatoyctrl)
    {
        state_cs = statebase;
        move_cs = move;
        collision_cs = collision;
        accessController_cs = accesscontroller;
        accessPosssibleAnnounce_cs = possibleannounce;
        haveInEatoy_cs = haveineatoy;
        haveEatoyCtrl_cs = haveeatoyctrl;

        move_cs.Init(player);
    }

    public void SetState(PlayerStateBase state) => state_cs = state;

    public PlayerStateBase GetState() => state_cs;

    public PlayerMove GetMove() => move_cs;

    public PlayerCollision GetCollision() => collision_cs;

    public PlayerAccessController GetAccessController() => accessController_cs;

    public PlayerAccessPossiblAnnounce GetAccessPossibleAnnounce() => accessPosssibleAnnounce_cs;

    public PlayerHaveInEatoy GetHaveInEatoy() => haveInEatoy_cs;

    public HaveEatoyCtrl GetHaveEatoyCtrl() => haveEatoyCtrl_cs;
}
