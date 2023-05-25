using Pathfinding;
using System.Collections;
using System.Collections.Generic;
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

    }
    public void ExitState(UnitStateController unitState)
    {
        Debug.Log("You are exiting at the Moving State");
    }
    public void UpdateState(UnitStateController unitState)
    {
        Debug.Log("You are updating at the Moving State");
        SetAndMoveToTarget();
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
        path.canMove = false;
        if (targeting.GoForObjective())
        {
            destinationSetter.target = targeting.ObjTarget.transform;
            path.canMove = true;
        }
        if(!targeting.GoForObjective())
        {
            destinationSetter.target = targeting.Target.transform;
            path.canMove = true;
        }        
    }
}