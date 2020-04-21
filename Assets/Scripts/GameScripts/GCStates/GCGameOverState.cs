using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCGameOverState : State<GameController>
{
    #region setup
    private static GCGameOverState _instance;

    private GCGameOverState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<GameController> createInstance() { return Instance; }

    public static GCGameOverState Instance
    {
        get { if (_instance == null) new GCGameOverState(); return _instance; }
    }
    #endregion

    public override void EnterState(GameController controller)
    {
        controller.TurnUpMusic(false);
        Cursor.visible = true;
        controller.plantUI.GetComponent<CanvasGroup>().alpha = 0;
        controller.waterUI.GetComponent<CanvasGroup>().alpha = 0;
        controller.gameOverMenu.SetActive(true);
        controller.player.stateMachine.ChangeState(PlayerWaitState.Instance);
    }

    public override void ExitState(GameController controller)
    {
        controller.gameOverMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public override void UpdateState(GameController controller)
    {
        Debug.Log(("game over"));
    }

    public override void FixedUpdateState(GameController controller)
    {
    }
}
