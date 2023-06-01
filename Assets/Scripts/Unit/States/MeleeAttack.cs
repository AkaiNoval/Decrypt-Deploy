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
        DealDamageToTarget(unitState);

        DealDamageToObjective(unitState);
    }

    void SwitchState(UnitStateController unitState)
    {
        var targeting = unitState.Targeting;
        var distanceToTarget = targeting.DistanceToTarget;
        var distanceToObj = unitState.Targeting.DistanceToObj;
        switch (unitState.UnitStats.UnitClass)
        {
            case Class.Attacker:
                // If there is a target but it is farther than the objective and the objective is in the close range, keep the melee state
                if (targeting.Target != null && distanceToTarget > distanceToObj && distanceToObj <= unitState.UnitStats.UnitCloseRange)
                    return;
                // If there is a target but it is closer to the objective and the objective is out of the close range, switch to the moving state
                if (targeting.Target != null && distanceToTarget < distanceToObj && distanceToObj > unitState.UnitStats.UnitCloseRange && distanceToTarget > unitState.UnitStats.UnitCloseRange)
                {
                    unitState.SwitchState(unitState.StateMoving);
                    return;
                }
                // If there is no target and the objective is in the close range, keep the melee state
                if (targeting.Target == null && distanceToObj <= unitState.UnitStats.UnitCloseRange)
                    return;
                // If there is no target and the objective is outside of the close range, switch to the moving state
                if (targeting.Target == null && distanceToObj > unitState.UnitStats.UnitCloseRange)
                    unitState.SwitchState(unitState.StateMoving);
                break;
            case Class.Supporter:
                // If there are no enemies nearby and no target
                if (!unitState.CheckEnemyForSupporter() && targeting.Target == null)
                    unitState.SwitchState(unitState.StateIdle);
                // If there is no target and there is an enemy in close range, melee attacking.
                if (targeting.Target == null && unitState.CheckEnemyForSupporter())
                    return;
                // If the target is within the close range but outside the far range
                if (distanceToTarget <= unitState.UnitStats.UnitCloseRange && distanceToTarget > unitState.UnitStats.UnitFarRange)
                    unitState.SwitchState(unitState.StateSupport);
                break;
        }
    }
    void DealDamageToTarget(UnitStateController unitState)
    {
        // Check if there is no target or if the target is not within the close range
        if (unitState.Targeting.Target == null || unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
        {
            return; // Exit the method if the conditions are not met
        }
        UnitStats targetStats = unitState.Targeting.Target.GetComponent<UnitStats>();

        // Calculate the base damage
        float baseDamage = unitState.UnitStats.UnitMeleeDamage;

        // Check for critical hit
        bool isCriticalHit = UnityEngine.Random.value <= unitState.UnitStats.UnitCriticalChance / 100f;

        // Apply critical damage multiplier if it's a critical hit
        float damageMultiplier = isCriticalHit ? (1f + unitState.UnitStats.UnitCriticalDamage / 100f) : 1f;

        // Calculate the final damage
        float reducedDamage = targetStats.CalculateReducedDamage(baseDamage * damageMultiplier, isCriticalHit);

        // Reduce the target's health by the damage amount
        targetStats.UnitCurrentHealth -= reducedDamage;
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
