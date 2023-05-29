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
    UsingActiveAbility,
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
    public RangedAttack StateRangeAttack = new RangedAttack();
    public Support StateSupport = new Support();
    public UsingActiveAbility StateActiveAbility = new UsingActiveAbility();
    public UsingPassiveAbility StatePassiveAbility = new UsingPassiveAbility();
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        targeting = GetComponent<Targeting>();
    }               
    void Start()
    {
        if(currentState == null)
        {
            currentState = StateMoving;
        }
        //starting state for the state machine
        currentState = StateMoving;
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
    public void CheckTargetToSwitchState()
    {
        bool hasTarget = targeting.Target != null;
        bool hasObjTarget = targeting.ObjTarget != null;
        bool isWithinFarRange = false;
        bool isWithinCloseRange = false;
        if (hasTarget)
        {
            isWithinFarRange = targeting.DistanceToTarget <= unitStats.UnitFarRange || targeting.DistBetweenTargetAndObject <= unitStats.UnitFarRange;
            isWithinCloseRange = targeting.DistanceToTarget <= unitStats.UnitCloseRange;
        }
        else if (hasObjTarget)
        {
            isWithinFarRange = targeting.DistanceToObj <= unitStats.UnitFarRange;
            isWithinCloseRange = targeting.DistanceToObj <= unitStats.UnitCloseRange;
        }
        if (isWithinFarRange)
        {
            // Switch to range combat
            // TODO: Add code to switch to range combat
        }
        if (isWithinCloseRange)
        {
            switch (unitStats.UnitClass)
            {
                case Class.Attacker:
                    SwitchState(StateMeleeAttack);
                    break;
                case Class.Supporter:
                    Debug.Log("SwitchState(StateSupport)");
                    break;
                default:
                    break;
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
                SwitchState(StateRangeAttack);
                break;
            case CurrentState.CloseAttack:
                SwitchState(StateMeleeAttack);
                break;
            case CurrentState.Support:
                SwitchState(StateSupport);
                break;
            case CurrentState.UsingActiveAbility:
                SwitchState(StateActiveAbility);
                break;
            case CurrentState.UsingPassiveAbility:
                SwitchState(StatePassiveAbility);
                break;
            default:
                break;
        }
    }
}
