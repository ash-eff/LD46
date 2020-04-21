using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCWaitState : State<GameController>
{
    #region setup
    private static GCWaitState _instance;

    private GCWaitState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<GameController> createInstance() { return Instance; }

    public static GCWaitState Instance
    {
        get { if (_instance == null) new GCWaitState(); return _instance; }
    }
    #endregion

    public override void EnterState(GameController controller)
    {
        controller.TurnUpMusic(true);
    }

    public override void ExitState(GameController controller)
    {
    }

    public override void UpdateState(GameController controller)
    {
        controller.UpdateGUI();
    }

    public override void FixedUpdateState(GameController controller)
    {
    }
}
