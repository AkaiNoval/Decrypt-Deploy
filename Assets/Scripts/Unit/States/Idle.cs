using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Idle : IState
{
    UnitStats unitStats;
    Targeting targeting;
    //IF Attacker and No Target => State Moving
    public void EnterState(UnitStateController unitState)
    {
        unitStats = unitState.GetComponent<UnitStats>();
        targeting = unitState.GetComponent<Targeting>();
        unitState.state = CurrentState.Idle;
        Debug.Log("You are at the Idle State");
    }

    public void ExitState(UnitStateController unitState)
    {
        Debug.Log("You are exiting at the Idle State");
    }
    public void UpdateState(UnitStateController unitState)
    {
        unitState.CheckTargetToSwitchState();
    }
    public void PhysicsUpdateState(UnitStateController unitState)
    {
        Debug.Log("Physics updating at the Idle State");
    }
    public void OnTriggerEnter2DState(UnitStateController unitState)
    {
        Debug.Log("OnTriggerEnter at the Idle State");
    }

    void CheckAvailableTarget(UnitStateController unitState)
    {
        if(unitStats.UnitClass == Class.Attacker)
        {
            unitState.SwitchState(unitState.StateMoving);
        }
    }

}
