using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCFanFareState : State<GameController>
{
    #region setup
    private static GCFanFareState _instance;

    private GCFanFareState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<GameController> createInstance() { return Instance; }

    public static GCFanFareState Instance
    {
        get { if (_instance == null) new GCFanFareState(); return _instance; }
    }
    #endregion

    public override void EnterState(GameController controller)
    {
        Cursor.visible = true;
        controller.player.stateMachine.ChangeState(PlayerWaitState.Instance);
    }

    public override void ExitState(GameController controller)
    {
    }

    public override void UpdateState(GameController controller)
    {
    }

    public override void FixedUpdateState(GameController controller)
    {
    }
}
