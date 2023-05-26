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
        unitState.CheckTargetToAttack();
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
        if(targeting==null)
        {
            Debug.Log("targeting is null");
        }
        if (targeting.GoToObjective())
        {
            destinationSetter.target = targeting.ObjTarget.transform;
            
        }
        else if(!targeting.GoToObjective())
        {
            destinationSetter.target = targeting.Target.transform;
        }
        
    }

}
