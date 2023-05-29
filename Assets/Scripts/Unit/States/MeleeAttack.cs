using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : IState
{
    Transform meleeTransform;
    UnitStats unitStats;
    public void EnterState(UnitStateController unitState)
    {
        unitState.state = CurrentState.CloseAttack;
        unitStats = unitState.GetComponent<UnitStats>();
        //meleeTransform = unitState.Fin
        Debug.Log("You are at the Melee State");
    }

    public void ExitState(UnitStateController unitState)
    {
        Debug.Log("You are out of the Melee State");

    }

    public void OnTriggerEnter2DState(UnitStateController unitState)
    {
        Debug.Log("You are triggering at the Melee State");
    }

    public void PhysicsUpdateState(UnitStateController unitState)
    {
        Debug.Log("You are physics updating at the Melee State");
    }

    public void UpdateState(UnitStateController unitState)
    {
        Debug.Log("You are LogicUpdating at the Melee State");
    }
}
