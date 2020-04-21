using UnityEngine;

public class PlayerBaseState : State<PlayerController>
{
    #region setup
    private static PlayerBaseState _instance;

    private PlayerBaseState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static PlayerBaseState Instance
    {
        get { if (_instance == null) new PlayerBaseState(); return _instance; }
    }
    #endregion

    public override void EnterState(PlayerController player)
    {
        player.SetCursorActive(true);
    }

    public override void ExitState(PlayerController player)
    {
    }

    public override void UpdateState(PlayerController player)
    {
        player.UpdateMoney();
        player.PlayerInput();
        player.CursorPosition();
        player.RotateWeapons();
        player.CheckAnimation();
        player.SetPlayerVelocity(10f, true);
        player.SetSpriteDirection();
        player.ClampPlayerToMap();
    }

    public override void FixedUpdateState(PlayerController player)
    {
    }
}
