using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.Support;
    }

    public void UpdateState(UnitStateController unitState)
    {    
        SwitchState(unitState);
        //Run Support animation, that has an event keyframe in unitState 
    }

    void SwitchState(UnitStateController unitState)
    {
        // Check if the target is within the close range
        if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange)
        {
            // The target is within the close range, no need to switch state
            return;
        }
        // Check if the target is outside the close range
        if (unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
        {
            // Switch to the moving state
            unitState.SwitchState(unitState.StateMoving);
            return;
        }
        // Check if there is no target and enemies are within the close range
        if (unitState.Targeting == null && unitState.CheckEnemyInCloseRange())
        {
            // Switch to the melee attack state
            unitState.SwitchState(unitState.StateMeleeAttack);
            return;
        }
        // No target and no enemies in close range, switch to the idle state
        unitState.SwitchState(unitState.StateIdle);
    }

    #region Nothing here
    public void ExitState(UnitStateController unitState)
    {

    }

    public void OnTriggerEnter2DState(UnitStateController unitState)
    {

    }

    public void PhysicsUpdateState(UnitStateController unitState)
    {

    } 
    #endregion
}
