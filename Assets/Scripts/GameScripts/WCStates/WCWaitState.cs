using UnityEngine;

public class WCWaitState : State<WaveController>
{
    #region setup
    private static WCWaitState _instance;

    private WCWaitState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<WaveController> createInstance() { return Instance; }

    public static WCWaitState Instance
    {
        get { if (_instance == null) new WCWaitState(); return _instance; }
    }
    #endregion

    public override void EnterState(WaveController controller)
    {
    }

    public override void ExitState(WaveController controller)
    {
    }

    public override void UpdateState(WaveController controller)
    {
    }

    public override void FixedUpdateState(WaveController controller)
    {
    }
}
