using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieAttackState : State<BaddieController>
{
    public override void EnterState(BaddieController baddie)
    {
        Debug.Log("Attack State");
        baddie.anim.SetBool("Running", false);
    }

    public override void ExitState(BaddieController baddie)
    {
    }

    public override void UpdateState(BaddieController baddie)
    {
        baddie.IsGameOver();
        baddie.CheckScreenPos();
        Attack(baddie);
    }

    public override void FixedUpdateState(BaddieController baddie)
    {
    }

    public void Attack(BaddieController baddie)
    {
        if (Time.time > baddie.RateOfAttack + baddie.LastAttack)
        {
            baddie.LastAttack = Time.time;
            baddie.plant.TakeDamage(baddie.DamageAmount);
        }
    }
}
