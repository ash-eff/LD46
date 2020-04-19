using UnityEngine;

public class WCSpawnState : State<WaveController>
{
    #region setup
    private static WCSpawnState _instance;

    private WCSpawnState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<WaveController> createInstance() { return Instance; }

    public static WCSpawnState Instance
    {
        get { if (_instance == null) new WCSpawnState(); return _instance; }
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
