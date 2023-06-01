using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.state = CurrentState.CloseAttack;
    }
    public void UpdateState(UnitStateController unitState)
    {

    }
    // Bug: Instead of calling this method only when the Target is in close range, it always gets called when there is a Target.
    // Solution 1: Only play the animation that triggers the event keyframe in this state.
    // Solution 2: Add an additional condition to this method, allowing it to be called only if the Target is in close range.
    // Fixed by using Solution 2 => Waiting for Solution 1
    public void DealDamage(UnitStateController unitState)
    {
        // Check if there is no target or if the target is not within the close range
        if (unitState.Targeting.Target == null || unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
        {
            return; // Exit the method if the conditions are not met
        }
        UnitStats targetStats = unitState.Targeting.Target.GetComponent<UnitStats>();
        float reducedDamage = targetStats.CalculateReducedDamage(unitState.UnitStats.UnitMeleeDamage);
        targetStats.UnitCurrentHealth -= reducedDamage;
    }

    void SwitchState(UnitStateController unitState)
    {
        switch (unitState.UnitStats.UnitClass)
        {
            case Class.Attacker:
                break;
            case Class.Supporter:

                if(unitState.Targeting.Target == null) return;
                if(unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitFarRange)
                {
                    unitState.SwitchState(unitState.StateSupport);
                }
                    break;
            default:
                break;
        }
    }
    #region Nothing here
    public void ExitState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { } 
    #endregion
}
