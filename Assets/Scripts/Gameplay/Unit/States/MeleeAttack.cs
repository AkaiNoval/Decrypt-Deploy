using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeAttack : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.CloseAttack;
    }
    public void UpdateState(UnitStateController unitState)
    {
        SwitchState(unitState);
    }
    // Bug: Instead of calling this method only when the Target is in close range, it always gets called when there is a Target.
    // Solution 1: Only play the animation that triggers the event keyframe in this state.
    // Solution 2: Add an additional condition to this method, allowing it to be called only if the Target is in close range.
    // Fixed by using Solution 2 => Waiting for Solution 1
    public void DealDamage(UnitStateController unitState, bool canMultiple)
    {
        DealDamageToTarget(unitState, canMultiple);

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
                if (!unitState.CheckEnemyInCloseRange() && targeting.Target == null)
                    unitState.SwitchState(unitState.StateIdle);
                // If there is no target and there is an enemy in close range, melee attacking.
                if (targeting.Target == null && unitState.CheckEnemyInCloseRange())
                    return;
                // If the target is within the close range but outside the far range
                if (distanceToTarget <= unitState.UnitStats.UnitCloseRange && distanceToTarget > unitState.UnitStats.UnitFarRange)
                    unitState.SwitchState(unitState.StateSupport);
                break;
        }
    }
    void DealDamageToTarget(UnitStateController unitState,bool canMultiple)
    {
        // Check if there is no target or if the target is not within the close range
        if (unitState.Targeting.Target == null || unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange) return;

        UnitStats targetStats = unitState.Targeting.Target.GetComponent<UnitStats>();

        // Calculate the base damage
        float baseDamage = unitState.UnitStats.UnitMeleeDamage;

        // Check for critical hit
        bool isCriticalHit = UnityEngine.Random.value <= unitState.UnitStats.UnitCriticalChance / 100f;

        // Apply critical damage multiplier if it's a critical hit
        float damageMultiplier = isCriticalHit ? (1f + unitState.UnitStats.UnitCriticalDamage / 100f) : 1f;

        // Calculate the final damage
        float reducedDamage = targetStats.CalculateReducedDamage(baseDamage * damageMultiplier, targetStats.UnitMeleeResistance, isCriticalHit);
        if(targetStats.UnitCurrentHealth <= reducedDamage)
        {
            AddDeadEnemyToCounter(unitState, targetStats.SoStats);
        }
        // Reduce the target's health by the damage amount
        targetStats.UnitCurrentHealth -= reducedDamage;
        unitState.KillCounter.DamageDealt += reducedDamage;
        // Check if multiple targets are allowed
        if (!canMultiple) return;
        // Find all enemies in close range
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(unitState.transform.position, unitState.UnitStats.UnitCloseRange, unitState.targetLayers);
        
        // Apply damage to each enemy in range
        foreach (var enemyCollider in enemiesColliders)
        {
            Unit enemyUnit = enemyCollider.GetComponent<Unit>();
            // Check if the collider belongs to an enemy unit
            if (enemyUnit != null && enemyUnit.IsEnemy != unitState.unit.IsEnemy)
            {
                // Exclude the initially targeted enemy (already dealt damage above)
                if (enemyUnit != unitState.Targeting.Target)
                {
                    UnitStats enemyStats = enemyUnit.GetComponent<UnitStats>();
        
                    // Calculate and apply the reduced damage to the enemy
                    float enemyReducedDamage = enemyStats.CalculateReducedDamage(baseDamage * damageMultiplier, targetStats.UnitMeleeResistance, isCriticalHit);
                    if (enemyStats.UnitCurrentHealth <= enemyReducedDamage)
                    {
                        AddDeadEnemyToCounter(unitState, enemyStats.SoStats);
                    }
                    enemyStats.UnitCurrentHealth -= enemyReducedDamage;
                    unitState.KillCounter.DamageDealt += enemyReducedDamage;
                }
            }
        }
        
    }

    void AddDeadEnemyToCounter(UnitStateController unitState,SOUnitStats enemy)
    {
        Debug.Log("I Killed someone");
        unitState.KillCounter.KillsList.Add(enemy);
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
