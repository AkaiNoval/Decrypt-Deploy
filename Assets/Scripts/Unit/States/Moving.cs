using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Moving : IState
{
    Targeting targeting;
    UnitStats unitStats;
    AIPath path;
    AIDestinationSetter destinationSetter;
    public void EnterState(UnitStateController unitState)
    {
        unitStats = unitState.GetComponent<UnitStats>();
        targeting = unitState.GetComponent<Targeting>();
        destinationSetter = unitState.GetComponent<AIDestinationSetter>();
        path = unitState.GetComponent<AIPath>();
        unitState.state = CurrentState.Moving;
        Debug.Log("You are at the Moving State");
        path.canMove = true;
        path.maxSpeed = unitStats.UnitSpeed;

    }
    public void ExitState(UnitStateController unitState)
    {
        Debug.Log("You are exiting at the Moving State");
        path.canMove = false;
    }
    public void UpdateState(UnitStateController unitState)
    {
        Debug.Log("You are updating at the Moving State");
        SetAndMoveToTarget();
        unitState.CheckTargetToSwitchState();
    }
    public void PhysicsUpdateState(UnitStateController unitState)
    {
        Debug.Log("Physics updating at the Moving State");
    }
    public void OnTriggerEnter2DState(UnitStateController unitState)
    {
        Debug.Log("OnTriggerEnter at the Moving State");
    }
    void SetAndMoveToTarget()
    {
        Transform targetTransform = null;
        // Check if the objective should be the target
        if (targeting.GoToObjective())
        {
            // If the objective target is not null, set it as the target transform
            targetTransform = targeting.Objective != null ? targeting.Objective.transform : null;
        }
        else
        {
            // If the objective target is not selected, set the primary target as the target transform
            targetTransform = targeting.Target != null ? targeting.Target.transform : null;
        }
        // Set the target transform in the destination setter
        destinationSetter.target = targetTransform;
    }


}
