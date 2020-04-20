using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitState : State<PlayerController>
{
    #region setup
    private static PlayerWaitState _instance;

    private PlayerWaitState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static PlayerWaitState Instance
    {
        get { if (_instance == null) new PlayerWaitState(); return _instance; }
    }
    #endregion

    public override void EnterState(PlayerController player)
    {
        player.SetPlayerVelocity(0, false);
        player.SetCursorActive(false);
        player.SetPlayerIdle();
        player.backScissors.SetActive(true);
        player.hose.gameObject.SetActive(true);
        player.cutters.gameObject.SetActive(false);
    }

    public override void ExitState(PlayerController player)
    {
    }

    public override void UpdateState(PlayerController player)
    {
        
    }

    public override void FixedUpdateState(PlayerController player)
    {
    }
}
