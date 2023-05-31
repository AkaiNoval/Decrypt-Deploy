using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Idle : IState
{
    LayerMask targetLayers;
    //IF Attacker and No Target => State Moving
    public void EnterState(UnitStateController unitState)
    {
        unitState.state = CurrentState.Idle;
        targetLayers = LayerMask.GetMask("Unit");
    }


    public void UpdateState(UnitStateController unitState)
    {
        CheckAvailableTarget(unitState);
    }
    void CheckAvailableTarget(UnitStateController unitState)
    {
        
        switch (unitState.UnitStats.UnitClass)
        {
            case Class.Attacker:
                // Check if the target is null, if so, switch to moving state
                if (unitState.Targeting.Target == null)
                {
                    unitState.SwitchState(unitState.StateMoving); return;
                }
                float distanceToTarget = unitState.Targeting.DistanceToTarget;
                float closeRange = unitState.UnitStats.UnitCloseRange;
                float farRange = unitState.UnitStats.UnitFarRange;

                // If the target is beyond the unit's far range or close range, switch to moving state
                if (distanceToTarget >= farRange)
                {
                    unitState.SwitchState(unitState.StateMoving); return;
                }
                // If the target is within the far range, switch to the range attack state
                if (distanceToTarget <= farRange && distanceToTarget >= closeRange)
                {
                    unitState.SwitchState(unitState.StateRangeAttack); return;
                }
                // If the target is within the close range, switch to the melee attack state
                if (distanceToTarget <= closeRange)
                {
                    unitState.SwitchState(unitState.StateMeleeAttack); return;
                }
                break;
            case Class.Supporter:
                // If no target is available
                if (unitState.Targeting == null)
                {
                    // Check if there are enemies nearby to switch to the melee attack state
                    if (CheckEnemyForSupporter(unitState))
                    {
                        unitState.SwitchState(unitState.StateMeleeAttack);
                    }
                    else
                    {
                        // If no enemies are nearby, switch to idle state
                        unitState.SwitchState(unitState.StateIdle);
                    }
                }
                else
                {
                    // If the target is beyond the close range, switch to moving state
                    if (unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
                    {
                        unitState.SwitchState(unitState.StateMoving);
                    }
                    else
                    {
                        // If the target is within the close range, switch to support state
                        unitState.SwitchState(unitState.StateSupport);
                    }
                }
                break;

            default:
                break;
        }
    }
    bool CheckEnemyForSupporter(UnitStateController unitState)
    {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(unitState.transform.position, unitState.UnitStats.UnitCloseRange, targetLayers);
        // Check if any enemies are found within the specified range
        foreach (var enemyCollider in enemiesColliders)
        {
            Unit enemyUnit = enemyCollider.GetComponent<Unit>();
            // Check if the collider belongs to an enemy unit
            if (enemyUnit != null && enemyUnit.IsEnemy)
            {
                return true; // Return true if an enemy is found
            }
        }
        return false; // Return false if no enemies are found
    }

    #region Nothing here

    public void ExitState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { } 
    #endregion






}
