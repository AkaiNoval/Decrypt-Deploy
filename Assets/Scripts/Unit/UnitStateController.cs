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
    [SerializeField] float stateSwitchDelay;
    Targeting targeting;
    UnitStats unitStats;
    // Start is called before the first frame update
    IState currentState;
    public LayerMask targetLayers;
    public CurrentState state;
    public Idle StateIdle = new Idle();
    public Moving StateMoving = new Moving();
    public MeleeAttack StateMeleeAttack = new MeleeAttack();
    public RangedAttack StateRangeAttack = new RangedAttack();
    public Support StateSupport = new Support();
    public UsingActiveAbility StateActiveAbility = new UsingActiveAbility();
    public UsingPassiveAbility StatePassiveAbility = new UsingPassiveAbility();
    [Header("Options")]
    public bool CanMultipleDamage;
    public GameObject bulletSpawnPoint;
    public GameObject bulletPrefab;
    public Targeting Targeting { get => targeting; set => targeting = value; }
    public UnitStats UnitStats { get => unitStats; set => unitStats = value; }

    private void Awake()
    {
        UnitStats = GetComponent<UnitStats>();
        Targeting = GetComponent<Targeting>();
        targetLayers = LayerMask.GetMask("Unit");
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
        //Manually Switch State
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
            StartCoroutine(DelayedStateSwitch(newState));
        }
    }
    IEnumerator DelayedStateSwitch(IState newState)
    {
        currentState.ExitState(this);

        yield return new WaitForSeconds(stateSwitchDelay);

        currentState = newState;
        currentState.EnterState(this); 
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

    public void TriggerMeleeAttack()
    {
        // Call the DealDamage method in the MeleeAttack state
        StateMeleeAttack.DealDamage(this);
    }
    public void Instantiate(GameObject bulletPrefab, GameObject bulletSpawnPoint, Quaternion rotation, float rangedDamage)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, rotation);
        bullet.GetComponent<Bullet>().IsEnemyBullet = GetComponent<Unit>().IsEnemy;
        bullet.GetComponent<Bullet>().BulletDamage = rangedDamage;
    }
    public bool CheckEnemyInCloseRange()
    {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, UnitStats.UnitCloseRange, targetLayers);
        // Check if any enemies are found within the specified range
        foreach (var enemyCollider in enemiesColliders)
        {
            Unit enemyUnit = enemyCollider.GetComponent<Unit>();
            // Check if the collider belongs to an enemy unit
            if (enemyUnit != null && enemyUnit.IsEnemy)
            {
                return true; // Return true if an enemy is found
            }
        }
        return false; // Return false if no enemies are found
    }
}
