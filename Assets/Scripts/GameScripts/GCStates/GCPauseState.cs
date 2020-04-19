using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPauseState : State<GameController>
{
    #region setup
    private static GCPauseState _instance;

    private GCPauseState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<GameController> createInstance() { return Instance; }

    public static GCPauseState Instance
    {
        get { if (_instance == null) new GCPauseState(); return _instance; }
    }
    #endregion

    public override void EnterState(GameController controller)
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        controller.pauseMenu.SetActive(true);
    }

    public override void ExitState(GameController controller)
    {
        Time.timeScale = 1;
        controller.pauseMenu.SetActive(false);
    }

    public override void UpdateState(GameController controller)
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            controller.stateMachine.ChangeState(GCPlayState.Instance);
        }
        controller.AdjustVolume();
    }

    public override void FixedUpdateState(GameController controller)
    {
    }
}
