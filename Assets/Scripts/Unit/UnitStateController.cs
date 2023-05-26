using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    Targeting targeting;
    UnitStats unitStats;
    // Start is called before the first frame update
    IState currentState;
    public CurrentState state;
    public Idle StateIdle = new Idle();
    public Moving StateMoving = new Moving();
    public MeleeAttack StateMeleeAttack = new MeleeAttack();
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        targeting = GetComponent<Targeting>();
    }               
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
        SwitchState();
    }
    void FixedUpdate()
    {
        currentState.PhysicsUpdateState(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2DState(this);
    }
    public void SwitchState(IState newState)
    {
        if (currentState != newState)
        {
            currentState.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }
    }
    public void CheckTargetToAttack()
    {
        if(targeting.Target != null)
        {
            if (targeting.DistanceToTarget <= unitStats.UnitFarRange || targeting.DistBetweenTargetAndObject <= unitStats.UnitFarRange)
            {
                //Switch To Range Combat
            }
            if (targeting.DistanceToTarget <= unitStats.UnitCloseRange /*|| targeting.ObjDistTarget <= unitStats.UnitCloseRange*/)
            {
                Debug.Log("targeting.Distance: " + targeting.DistanceToTarget);
                Debug.Log("unitStats.UnitCloseRange: " + unitStats.UnitCloseRange);
                SwitchState(StateMeleeAttack);
            }
        }
        else if(targeting.ObjTarget != null)
        {
            if (targeting.DistanceToObj <= unitStats.UnitFarRange)
            {
                //Switch To Range Combat
            }
            if (targeting.DistanceToObj <= unitStats.UnitCloseRange /*|| targeting.ObjDistTarget <= unitStats.UnitCloseRange*/)
            {
                Debug.Log("targeting.DistanceToObj: " + targeting.DistanceToObj);
                Debug.Log("unitStats.DistanceToObj: " + unitStats.UnitCloseRange);
                SwitchState(StateMeleeAttack);
            }
        }

    }
    void SwitchState()
    {
        switch (state)
        {
            case CurrentState.Idle:
                SwitchState(StateIdle);
                break;
            case CurrentState.Moving:
                SwitchState(StateMoving);
                break;
            case CurrentState.RangedAttack:
                break;
            case CurrentState.CloseAttack:
                SwitchState(StateMeleeAttack);
                break;
            case CurrentState.Support:
                break;
            case CurrentState.UsingInteractableAbility:
                break;
            case CurrentState.UsingPassiveAbility:
                break;
            default:
                break;
        }
    }
}
