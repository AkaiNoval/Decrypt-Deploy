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
    [Range(0f, 1f)]
    [SerializeField] float stateSwitchDelay;
    IState state;
    public LayerMask targetLayers;
    public CurrentState currentState;
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
    public Targeting Targeting { get ; set; }
    public UnitStats UnitStats { get; set; }

    public bool IsCoroutineRunning { get; set; }
    public float StateSwitchDelay { get => stateSwitchDelay; set => stateSwitchDelay = Mathf.Clamp(value,0,1); }

    private void Awake()
    {
        UnitStats = GetComponent<UnitStats>();
        Targeting = GetComponent<Targeting>();
        targetLayers = LayerMask.GetMask("Unit");
    }               
    void Start()
    {
        IsCoroutineRunning = false;
        //starting state for the state machine
        state = StateIdle;
        // "this" is a reference to the context(THIS script)
        state.EnterState(this);
    }
    void Update()
    {
        SwitchState();
        if (currentState != CurrentState.UsingPassiveAbility)
        {
            UnitStats.PassiveAbility.StartTimer(this);
        }
        state.UpdateState(this);

    }
    void FixedUpdate()
    {
        state.PhysicsUpdateState(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        state.OnTriggerEnter2DState(this);
    }
    public void SwitchState(IState newState)
    {
        //Avoid going to another state when chaning to  USINGPASSIVE or USINGACTIVE STATE.
        if (currentState == CurrentState.UsingPassiveAbility)
        {
            newState = StatePassiveAbility;
        }
        if(currentState == CurrentState.UsingActiveAbility)
        {
            newState = StateActiveAbility;
        }
        if (state == newState) return; 
        StartCoroutine(DelayedStateSwitch(newState));
    }
    IEnumerator DelayedStateSwitch(IState newState)
    {
        float localStateSwitchDelay = StateSwitchDelay;
        if(newState == StatePassiveAbility || newState == StateActiveAbility)
        {
            localStateSwitchDelay = 0f;
        }
        state.ExitState(this);

        yield return new WaitForSeconds(localStateSwitchDelay);

        state = newState;
        state.EnterState(this); 
    }
    void SwitchState()
    {
        switch (currentState)
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

    #region Button
    public void ChangeToActiveAbilityState()
    {
        currentState = CurrentState.UsingActiveAbility;
    }

    public void ActiveActiveAbility()
    {
        UnitStats.ActiveAbility.ApplyActiveAbility(this);
    }
    #endregion

    #region AnimationEvent keyframe
    public void TriggerActiveAbility()
    {
        Debug.Log("Active Ability");
    }

    //Based on Animation Keyframe
    public void TriggerPassiveAbility()
    {
        if (UnitStats.PassiveAbility == null) return;
        UnitStats.PassiveAbility.ApplyPassiveAbility(this);
        IsCoroutineRunning = false;
        currentState = CurrentState.Idle;
    }

    //Based on Animation Keyframe 
    public void TriggerSupport()
    {
        if (UnitStats.SupportType == null) return;
        UnitStats.SupportType.ApplySupport(this);
    }
    //Based on Animation Keyframe 
    public void TriggerMeleeAttack()
    {
        // Call the DealDamage method in the MeleeAttack state
        StateMeleeAttack.DealDamage(this);
    }
    //Based on Animation Keyframe 
    public void TriggerRangeAttack(GameObject bulletPrefab, GameObject bulletSpawnPoint, Quaternion rotation, float rangedDamage)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, rotation);
        bullet.GetComponent<Bullet>().IsEnemyBullet = GetComponent<Unit>().IsEnemy;
        bullet.GetComponent<Bullet>().BulletDamage = rangedDamage;
    } 
    #endregion

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
