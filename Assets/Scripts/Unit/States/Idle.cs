using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    Rigidbody2D rigidbody;
    Targeting targeting;

    public void EnterState(UnitStateController unitState)
    {
        rigidbody = unitState.GetComponent<Rigidbody2D>();
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
        if (rigidbody.velocity.magnitude <= 0)
        {
            Debug.Log("isStanding");
        }
        //if (targeting.Target == null) return;
        if (rigidbody.velocity.magnitude > 0|| targeting.Target != null)
        {
            Debug.Log("isMoving");
            unitState.SwitchState(unitState.StateMoving);
        }
        unitState.CheckTargetToAttack();
    }
    public void PhysicsUpdateState(UnitStateController unitState)
    {
        Debug.Log("Physics updating at the Idle State");
    }
    public void OnTriggerEnter2DState(UnitStateController unitState)
    {
        Debug.Log("OnTriggerEnter at the Idle State");
    }



}
