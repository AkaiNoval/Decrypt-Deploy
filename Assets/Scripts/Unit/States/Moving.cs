using Pathfinding;
using UnityEngine;

public class Moving : IState
{
    AILerp path;
    AIDestinationSetter destinationSetter;
    float previousSpeed;
    public void EnterState(UnitStateController unitState)
    {
        destinationSetter = unitState.GetComponent<AIDestinationSetter>();
        path = unitState.GetComponent<AILerp>();
        Debug.Log("You are at the Moving State");
        unitState.state = CurrentState.Moving;
        path.canMove = true;
        previousSpeed = unitState.UnitStats.UnitSpeed;
        path.speed = unitState.UnitStats.UnitSpeed;
    }
    public void ExitState(UnitStateController unitState)
    {
        Debug.Log("You are exiting at the Moving State");
        path.canMove = false;
    }
    public void UpdateState(UnitStateController unitState)
    {
        SpeedUpdate(unitState);
        SwitchState(unitState);
        SetAndMoveToTarget(unitState);
    }
    void SpeedUpdate(UnitStateController unitState)
    {
        if (previousSpeed != unitState.UnitStats.UnitSpeed)
        {
            path.speed = unitState.UnitStats.UnitSpeed;
            previousSpeed = unitState.UnitStats.UnitSpeed;
        }
    }
    void SwitchState(UnitStateController unitState)
    {
        #region Supporter
        if (unitState.UnitStats.UnitClass == Class.Supporter)
        {
            if (unitState.Targeting.Target == null)
            {
                // If the unit class is Supporter and the target is null, switch to the Idle state
                // TODO: Implement random movement in allowed area for supporters
                unitState.SwitchState(unitState.StateIdle);
                return;
            }

            if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitFarRange)
            {
                // If the target is within the medium range, switch to the Support state
                unitState.SwitchState(unitState.StateSupport);
                return;
            }

        }
        #endregion

        #region Attacker
        else if (unitState.UnitStats.UnitClass == Class.Attacker)
        {
            #region Objective
            if (unitState.Targeting.Objective != null && unitState.Targeting.DistanceToObj <= unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToObj > unitState.UnitStats.UnitCloseRange)
            {
                // If the objective is within the long range, switch to the RangeAttack state
                unitState.SwitchState(unitState.StateRangeAttack);
                return;
            }
            else if (unitState.Targeting.DistanceToObj <= unitState.UnitStats.UnitCloseRange)
            {
                // If the objective is within the close range, switch to the MeleeAttack state
                unitState.SwitchState(unitState.StateMeleeAttack);
                return;
            }
            #endregion

            #region Target
            if (unitState.Targeting.Target == null)
            {
                return;
            }
            if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
            {
                // If the target is within the long range, switch to the RangeAttack state
                unitState.SwitchState(unitState.StateRangeAttack);
                return;
            }
            else if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange)
            {
                // If the target is within the close range, switch to the MeleeAttack state
                unitState.SwitchState(unitState.StateMeleeAttack);
                return;
            } 
            #endregion
        } 
        #endregion
    }
    void SetAndMoveToTarget(UnitStateController unitState)
    {
        Transform targetTransform = null;
        // Check if the objective should be the target
        if (GoToObjective(unitState))
        {
            // If the objective target is not null, set it as the target transform
            targetTransform = unitState.Targeting.Objective != null ? unitState.Targeting.Objective.transform : null;
        }
        else
        {
            if (unitState.UnitStats.UnitClass == Class.Attacker)
            {
                // If the unit class is Attacker, set the primary target as the target transform
                targetTransform = unitState.Targeting.Target != null ? unitState.Targeting.Target.transform : null;
            }
            else if (unitState.UnitStats.UnitClass == Class.Supporter)
            {
                if (unitState.Targeting.Target == null)
                {
                    // If the unit class is Supporter and the target is null, switch to the Idle state
                    unitState.SwitchState(unitState.StateIdle);
                    return;
                }
                // If the target is not null, set it as the target transform
                targetTransform = unitState.Targeting.Target != null ? unitState.Targeting.Target.transform : null;
            }
        }
        // Set the target transform in the destination setter
        destinationSetter.target = targetTransform;
    }
    bool GoToObjective(UnitStateController unitState)
    {
        //Only Attacker can go to the ojbective
        return unitState.UnitStats.UnitClass switch
        {
            // If the unit class is Attacker and either the primary target is null or the distance to the objective is shorter than the distance to the primary target, return true.
            Class.Attacker when (unitState.Targeting.Target == null || unitState.Targeting.DistanceToObj < unitState.Targeting.DistanceToTarget) => true,

            // If the unit class is Supporter and the primary target is null, return false.
            Class.Supporter when (unitState.Targeting.Target == null) => false,

            // For any other unit class, return false.
            _ => false
        };
    }

    #region Nothing here
    public void PhysicsUpdateState(UnitStateController unitState)
    {
        Debug.Log("Physics updating at the Moving State");
    }
    public void OnTriggerEnter2DState(UnitStateController unitState)
    {
        Debug.Log("OnTriggerEnter at the Moving State");
    }
    #endregion
}
