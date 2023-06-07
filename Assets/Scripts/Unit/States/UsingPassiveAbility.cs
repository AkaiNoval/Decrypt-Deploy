using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UsingPassiveAbility : IState
{
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.UsingPassiveAbility;
    }
    public void UpdateState(UnitStateController unitState)
    {
        //Run passivingAbility animation, that has an event keyframe in unitState 
        
    }
    #region Nothing here
    public void ExitState(UnitStateController unitState) { }
    public void OnTriggerEnter2DState(UnitStateController unitState) { }
    public void PhysicsUpdateState(UnitStateController unitState) { } 
    #endregion
}
