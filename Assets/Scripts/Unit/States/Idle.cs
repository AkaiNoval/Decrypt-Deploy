using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Idle : IState
{
    
    //IF Attacker and No Target => State Moving
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.Idle;
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
                // Retrieve distance variables and target from unitState
                float distanceToTarget = unitState.Targeting.DistanceToTarget;
                float distanceToObj = unitState.Targeting.DistanceToObj;
                float closeRange = unitState.UnitStats.UnitCloseRange;
                float farRange = unitState.UnitStats.UnitFarRange;
                Unit Target = unitState.Targeting.Target;

                // Check the conditions and switch states accordingly
                if (Target == null)
                {
                    if (distanceToObj > closeRange && distanceToObj > farRange)
                    {
                        // If there is no target and the distance to object is beyond both ranges, switch to moving state
                        unitState.SwitchState(unitState.StateMoving);
                    }
                    else if (distanceToObj > closeRange && distanceToObj <= farRange)
                    {
                        // If there is no target and the distance to object is within the far range, switch to range attack state
                        unitState.SwitchState(unitState.StateRangeAttack);
                    }
                    else if (distanceToObj <= closeRange)
                    {
                        // If there is no target and the distance to object is within the close range, switch to melee attack state
                        unitState.SwitchState(unitState.StateMeleeAttack);
                    }
                    // No target, return
                    return;
                }
                // Check the conditions based on the distance to the target and switch states accordingly
                if (distanceToTarget <= farRange && distanceToTarget >= closeRange)
                {
                    // If the target is within the far range, switch to range attack state
                    unitState.SwitchState(unitState.StateRangeAttack);
                }
                else if (distanceToTarget <= closeRange)
                {
                    // If the target is within the close range, switch to melee attack state
                    unitState.SwitchState(unitState.StateMeleeAttack);
                }
                else if (distanceToTarget >= farRange)
                {
                    // If the target is beyond the unit's far range, switch to moving state
                    unitState.SwitchState(unitState.StateMoving);
                }
                // Check if the target is null, if so, switch to moving state
                break;

            case Class.Supporter:
                // If no target is available
                if (unitState.Targeting == null)
                {
                    // Check if there are enemies nearby to switch to the melee attack state
                    if (unitState.CheckEnemyInCloseRange())
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


    #region Nothing here

    public void ExitState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { } 
    #endregion
}
