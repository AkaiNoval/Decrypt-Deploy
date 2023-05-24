using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState(UnitStateController unitState);
    void UpdateState(UnitStateController unitState);
    void PhysicsUpdateState(UnitStateController unitState);
    void ExitState(UnitStateController unitState);
    void OnTriggerEnter2DState(UnitStateController unitState);
}
public enum CurrentState
{
    Idle,
    Moving,
    RangedAttack,
    CloseAttack,
    Support,
    UsingInteractableAbility,
    UsingPassiveAbility
}
public class UnitStateController : MonoBehaviour
{
    // Start is called before the first frame update
    IState currentState;
    public CurrentState state;
    public Idle StateIdle = new Idle();
    public Moving StateMoving = new Moving();
    void Start()
    {
        if(currentState == null)
        {
            currentState = StateIdle;
        }
        //starting state for the state machine
        currentState = StateIdle;
        // "this" is a reference to the context(THIS script)
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //will call any logic in Update State
        currentState.UpdateState(this);
    }
    void FixedUpdate()
    {
        currentState.PhysicsUpdateState(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2DState(this);
    }
    public void SwitchState(IState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
