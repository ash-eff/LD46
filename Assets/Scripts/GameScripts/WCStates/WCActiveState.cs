using UnityEngine;

public class WCActiveState : State<WaveController>
{
    #region setup
    private static WCActiveState _instance;

    private WCActiveState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<WaveController> createInstance() { return Instance; }

    public static WCActiveState Instance
    {
        get { if (_instance == null) new WCActiveState(); return _instance; }
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
