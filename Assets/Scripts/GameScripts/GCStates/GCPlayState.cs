using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPlayState : State<GameController>
{
    #region setup
    private static GCPlayState _instance;
    
    private GCPlayState()
    {
        if (_instance != null) return;
        _instance = this;
    }
    
    public override State<GameController> createInstance() { return Instance; }
    
    public static GCPlayState Instance
    {
        get { if (_instance == null) new GCPlayState(); return _instance; }
    }
    #endregion
    
    public override void EnterState(GameController controller)
    {
        controller.TurnUpMusic(true);
        Cursor.visible = false;
        controller.plantUI.GetComponent<CanvasGroup>().alpha = 1;
        controller.waterUI.GetComponent<CanvasGroup>().alpha = 1;
        controller.player.stateMachine.ChangeState(PlayerBaseState.Instance);
    }
    
    public override void ExitState(GameController controller)
    {
    }
    
    public override void UpdateState(GameController controller)
    { 
        if (Input.GetKeyDown(KeyCode.P))
        {
            controller.stateMachine.ChangeState(GCPauseState.Instance);
        }
    }
    
    public override void FixedUpdateState(GameController controller)
    {
    }
}
