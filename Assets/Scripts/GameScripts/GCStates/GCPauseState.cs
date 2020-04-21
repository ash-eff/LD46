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
        controller.TurnUpMusic(false);
        Cursor.visible = true;
        controller.plantUI.GetComponent<CanvasGroup>().alpha = 0;
        controller.waterUI.GetComponent<CanvasGroup>().alpha = 0;
        Time.timeScale = 0;
        controller.pauseMenu.SetActive(true);
        controller.player.stateMachine.ChangeState(PlayerWaitState.Instance);
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
    }

    public override void FixedUpdateState(GameController controller)
    {
    }
}
