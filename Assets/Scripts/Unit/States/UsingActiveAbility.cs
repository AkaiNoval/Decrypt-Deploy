using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingActiveAbility : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.UsingActiveAbility;
    }

    public void UpdateState(UnitStateController unitState)
    {
        //play the animation that call animation event 
    }

    #region Nothing here
    public void ExitState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { } 
    #endregion
}
