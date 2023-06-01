using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeAttack : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.state = CurrentState.CloseAttack;
    }
    public void UpdateState(UnitStateController unitState)
    {
        SwitchState(unitState);
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

        DealDamageToObjective(unitState);
    }

    void SwitchState(UnitStateController unitState)
    {
        var targeting = unitState.Targeting;
        var distanceToTarget = targeting.DistanceToTarget;

        switch (unitState.UnitStats.UnitClass)
        {
            case Class.Attacker:
                // If the target is within the close range of the unit or the objective is within the close range
                //if (distanceToTarget <= unitState.UnitStats.UnitCloseRange || targeting.DistanceToObj <= unitState.UnitStats.UnitCloseRange)
                //{
                //    Debug.Log("Can't Switch");
                //    return;
                //}
                // If the target is within the range for ranged attack
                if (distanceToTarget > unitState.UnitStats.UnitCloseRange && distanceToTarget <= unitState.UnitStats.UnitFarRange)
                    unitState.SwitchState(unitState.StateRangeAttack);
                // If the target is outside the close range and far range, or there is no target
                else if (distanceToTarget > unitState.UnitStats.UnitCloseRange && distanceToTarget > unitState.UnitStats.UnitFarRange || targeting.Target == null)
                    unitState.SwitchState(unitState.StateMoving);
                break;
            case Class.Supporter:
                // If there are no enemies nearby and no target
                if (!unitState.CheckEnemyForSupporter() && targeting.Target == null)
                    unitState.SwitchState(unitState.StateIdle);

                // If there is no target, return without switching state
                if (targeting.Target == null)
                    return;

                // If the target is within the close range but outside the far range
                if (distanceToTarget <= unitState.UnitStats.UnitCloseRange && distanceToTarget > unitState.UnitStats.UnitFarRange)
                    unitState.SwitchState(unitState.StateSupport);
                break;
        }
    }
    void DealDamageToObjective(UnitStateController unitState)
    {
        if (unitState.Targeting.DistanceToObj > unitState.UnitStats.UnitCloseRange) return;
        unitState.Targeting.Objective.ObjectiveCurrentHealth -= unitState.UnitStats.UnitMeleeDamage;
    }
    #region Nothing here
    public void ExitState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { } 
    #endregion
}
