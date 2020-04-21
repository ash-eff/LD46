using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieFrozenState : State<BaddieController>
{
    public override void EnterState(BaddieController baddie)
    {
    }

    public override void ExitState(BaddieController baddie)
    {
    }

    public override void UpdateState(BaddieController baddie)
    {
        baddie.IsGameOver();
        baddie.CheckScreenPos();
    }

    public override void FixedUpdateState(BaddieController baddie)
    {
    }
}
