using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : IState
{
    Targeting targeting;
    UnitStats unitStats;
    Unit unit;
    AIPath path;

    AIDestinationSetter destinationSetter;
    UnitObjective[] unitObjectives;
    UnitObjective unitObjective;
    public void EnterState(UnitStateController unitState)
    {
        unitStats = unitState.GetComponent<UnitStats>();
        targeting = unitState.GetComponent<Targeting>();
        destinationSetter = unitState.GetComponent<AIDestinationSetter>();
        path = unitState.GetComponent<AIPath>();
        unit = unitState.GetComponent<Unit>();
        unitState.state = CurrentState.Moving;
        unitObjectives = Object.FindObjectsOfType<UnitObjective>();
        BoxCollider2D boxCollider = unitState.GetComponent<BoxCollider2D>();
        GetObjective(unitState);
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

        if (targeting.Target == null)
        {
            
            destinationSetter.target = unitObjective.transform;
            path.canMove = true;
        }
        else 
        {
            destinationSetter.target = targeting.Target.transform;
            path.canMove = true;
        }


    }
    void GetObjective(UnitStateController unitState)
    {
        foreach (var unitObj in unitObjectives)
        {
            if(unit.IsEnemy && unitObj.IsEnemy)
            {
                unitObjective = unitObj;
                break;
            }
            else if(!unit.IsEnemy && unitObj.IsEnemy)
            {
                unitObjective = unitObj;
                break;
            }
        }
    }
}
