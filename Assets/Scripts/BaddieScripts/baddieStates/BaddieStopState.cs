using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieStopState : State<BaddieController>
{
    public override void EnterState(BaddieController baddie)
    {
        baddie.gameObject.SetActive(false);
    }

    public override void ExitState(BaddieController baddie)
    {
    }

    public override void UpdateState(BaddieController baddie)
    {
    }

    public override void FixedUpdateState(BaddieController baddie)
    {
    }
}
