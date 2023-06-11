using System.Collections.Generic;
using UnityEngine;

public enum TargetSeeker
{
    SeekClosestEnemy,
    SeekClosestAlly,
    SeekClosestAndLowestHealthAlly,
    SeekClosestAndLowestHealthEnemy,
    SeekClosestAttackerAlly,
    SeekClosestAttackerEnemy,
    SeekClosestAndHighestHealthAlly,
    SeekClosestAndHighestHealthEnemy
}

public class Targeting : MonoBehaviour
{
    UnitStats unitStats;
    [SerializeField] bool showGizmos;
    Unit _unit;  // Reference to the unit this script is attached to
    [SerializeField] TargetSeeker targetSeeker;  // The strategy for selecting the target
    [SerializeField] Unit target;// The current target
    [SerializeField] UnitObjective objectiveTarget;
    UnitObjective[] objTargets;
    [SerializeField] float range = 10f;  // The maximum range for selecting a target, if there are no targets in this radius => NULL
    [SerializeField] float distanceToTarget;
    [SerializeField] float distanceToObjective;
    [SerializeField] float distanceBetweenTargetAndObject;

    [SerializeField] bool wasEnemy; // Flag to store the previous value of the isEnemy flag

    public Unit Target { get { return target; } set { target = value; } }
    public UnitObjective Objective { get => objectiveTarget; set => objectiveTarget = value; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float DistanceToObj { get => distanceToObjective; set => distanceToObjective = value; }
    public float DistBetweenTargetAndObject { get => distanceBetweenTargetAndObject; set => distanceBetweenTargetAndObject = value; }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        unitStats = GetComponent<UnitStats>();
        objTargets = FindObjectsOfType<UnitObjective>();
        wasEnemy = _unit.IsEnemy;
        GetObjective();
        CalcDist();
    }
    private void Update()
    {
        // Update the current target by finding the closest unit within range
        Target = GetClosestUnit(_unit.GetPosition(), range); // Store the previous value of IsEnemy
        // Check if the value of IsEnemy has changed
        if (_unit.IsEnemy != wasEnemy)
        {
            GetObjective(); // Update the objective
        }
        CalcDist(); // Calculate distances
        RotateUnit();
    }


    public Unit GetClosestUnit(Vector3 position, float maxRange)
    {
        switch (targetSeeker)
        {
            case TargetSeeker.SeekClosestEnemy:
                return GetClosestEnemy(position, maxRange);
            case TargetSeeker.SeekClosestAlly:
                return GetClosestAlly(position, maxRange);
            case TargetSeeker.SeekClosestAndLowestHealthAlly:
                return GetClosestAndLowestHealthAlly(position, maxRange);
            case TargetSeeker.SeekClosestAndLowestHealthEnemy:
                return GetClosestAndLowestHealthEnemy(position, maxRange);
            case TargetSeeker.SeekClosestAttackerAlly:
                return GetClosestAttackerAlly(position, maxRange);
            case TargetSeeker.SeekClosestAttackerEnemy:
                return GetClosestAttackerEnemy(position, maxRange);
            case TargetSeeker.SeekClosestAndHighestHealthAlly:
                return GetClosestAndHighestHealthAlly(position, maxRange);
            case TargetSeeker.SeekClosestAndHighestHealthEnemy:
                return GetClosestAndHighestHealthEnemy(position, maxRange);
            default:
                Debug.LogError("Invalid TargetSeeker value");
                return null;
        }
    }
    private Unit GetClosestEnemy(Vector3 position, float maxRange)
    {
        List<Unit> enemyList = _unit.IsEnemy ? UnitManager.AllyList : UnitManager.EnemyList;
        return FindClosestUnit(position, maxRange, enemyList);
    }
    private Unit GetClosestAlly(Vector3 position, float maxRange)
    {
        List<Unit> allyList = _unit.IsEnemy ? UnitManager.EnemyList : UnitManager.AllyList;
        return FindClosestUnit(position, maxRange, allyList);
    }
    private Unit GetClosestAndLowestHealthAlly(Vector3 position, float maxRange)
    {
        List<Unit> allyList = _unit.IsEnemy ? UnitManager.EnemyList : UnitManager.AllyList;
        return FindClosestAndLowestHealthUnit(position, maxRange, allyList);
    }
    private Unit GetClosestAndLowestHealthEnemy(Vector3 position, float maxRange)
    {
        List<Unit> enemyList = _unit.IsEnemy ? UnitManager.AllyList : UnitManager.EnemyList;
        return FindClosestAndLowestHealthUnit(position, maxRange, enemyList);
    }
    private Unit GetClosestAndHighestHealthAlly(Vector3 position, float maxRange)
    {
        List<Unit> allyList = _unit.IsEnemy ? UnitManager.EnemyList : UnitManager.AllyList;
        return FindClosestAndHighestHealthUnit(position, maxRange, allyList);
    }
    private Unit GetClosestAndHighestHealthEnemy(Vector3 position, float maxRange)
    {
        List<Unit> enemyList = _unit.IsEnemy ? UnitManager.AllyList : UnitManager.EnemyList;
        return FindClosestAndHighestHealthUnit(position, maxRange, enemyList);
    }
    private Unit GetClosestAttackerAlly(Vector3 position, float maxRange)
    {
        List<Unit> allyList = _unit.IsEnemy ? UnitManager.EnemyList : UnitManager.AllyList;
        List<Unit> attackerList = allyList.FindAll(u => u.GetComponent<UnitStats>().UnitClass == Class.Attacker);
        return FindClosestUnit(position, maxRange, attackerList);
    }
    private Unit GetClosestAttackerEnemy(Vector3 position, float maxRange)
    {
        List<Unit> enemyList = _unit.IsEnemy ? UnitManager.AllyList : UnitManager.EnemyList;
        List<Unit> attackerList = enemyList.FindAll(u => u.GetComponent<UnitStats>().UnitClass == Class.Attacker);
        return FindClosestUnit(position, maxRange, attackerList);
    }
    private Unit FindClosestUnit(Vector3 position, float maxRange, List<Unit> unitList)
    {
        Unit closest = null;
        float closestDistance = float.MaxValue;

        foreach (Unit unit in unitList)
        {
            if (unit == _unit || unit.IsDead())
                continue;

            float distance = Vector3.Distance(position, unit.GetPosition());

            if (distance <= maxRange && (closest == null || distance < closestDistance))
            {
                closest = unit;
                closestDistance = distance;
            }
        }
        return closest;
    }
    private Unit FindClosestAndLowestHealthUnit(Vector3 position, float maxRange, List<Unit> unitList)
    {
        Unit closestWithLowestHealth = null;
        float lowestHealth = float.MaxValue;
        float closestDistance = float.MaxValue;

        foreach (Unit unit in unitList)
        {
            if (unit == _unit|| unit.IsDead())
                continue;

            float distance = Vector3.Distance(position, unit.GetPosition());

            if (distance <= maxRange)
            {
                UnitStats unitStats = unit.GetComponent<UnitStats>();

                if (unitStats != null && unitStats.UnitCurrentHealth < lowestHealth)
                {
                    lowestHealth = unitStats.UnitCurrentHealth;
                    closestWithLowestHealth = unit;
                    closestDistance = distance;
                }
                else if (unitStats != null && unitStats.UnitCurrentHealth == lowestHealth && distance < closestDistance)
                {
                    closestWithLowestHealth = unit;
                    closestDistance = distance;
                }
            }
        }
        return closestWithLowestHealth;
    }
    private Unit FindClosestAndHighestHealthUnit(Vector3 position, float maxRange, List<Unit> unitList)
    {
        Unit closestWithHighestHealth = null;
        float highestHealth = float.MinValue;
        float closestDistance = float.MaxValue;

        foreach (Unit unit in unitList)
        {
            // Skip the current unit or any dead units
            if (unit == _unit || unit.IsDead())
                continue;

            // Calculate the distance between the current unit and the target position
            float distance = Vector3.Distance(position, unit.GetPosition());

            // Check if the unit is within the maximum range
            if (distance <= maxRange)
            {
                UnitStats unitStats = unit.GetComponent<UnitStats>();

                // Check if the unit has higher health than the current highest health
                if (unitStats != null && unitStats.UnitCurrentHealth > highestHealth)
                {
                    highestHealth = unitStats.UnitCurrentHealth;
                    closestWithHighestHealth = unit;
                    closestDistance = distance;
                }
                // If the unit has the same health as the current highest health, check for closest distance
                else if (unitStats != null && unitStats.UnitCurrentHealth == highestHealth && distance < closestDistance)
                {
                    closestWithHighestHealth = unit;
                    closestDistance = distance;
                }
            }
        }

        return closestWithHighestHealth;
    }

    private void RotateUnit()
    {
        if (_unit.IsEnemy)
        {
            if (Target != null && Target.transform.position.x > transform.position.x)
            {
                // Target is on the right, rotate to face right
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // Target is on the left or no target, rotate to face left
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (Target != null && Target.transform.position.x < transform.position.x)
            {
                // Target is on the left, rotate to face left
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                // Target is on the right or no target, rotate to face right
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }
    public float DistanceBetweenUnitAndTarget()
    {
        if (Target == null) return 0f;
        else return DistanceToTarget = Vector3.Distance(this.transform.position, target.transform.position); 
    }
    public float DistanceBetweenUnitAndObject() => DistanceToObj = Vector3.Distance(this.transform.position, objectiveTarget.transform.position);
    public float DistanceBetweenObjectiveAndClosestTarget() => DistBetweenTargetAndObject = Vector3.Distance(target.transform.position, Objective.transform.position);

    public void GetObjective()
    {
        foreach (var unitObj in objTargets)
        {
            if ((!_unit.IsEnemy && unitObj.IsEnemy) || (_unit.IsEnemy && !unitObj.IsEnemy))
            {
                objectiveTarget = unitObj;
                break; // Exit the loop once a suitable unitObjective is found
            }
        }
        wasEnemy = _unit.IsEnemy;
    }
    private void CalcDist()
    {
        if (Objective == null) return;
        DistanceBetweenUnitAndObject();
        if (Target == null) return;
        DistanceBetweenUnitAndTarget();
        DistanceBetweenObjectiveAndClosestTarget();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (_unit==null) return;
        if (_unit.IsEnemy)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.cyan;
        }
        // Draw the first wire sphere using UnitFarRange
        Gizmos.DrawWireSphere(_unit.GetPosition(), unitStats.UnitFarRange);
        // Draw the second wire sphere using UnitCloseRange
        Gizmos.DrawWireSphere(_unit.GetPosition(), unitStats.UnitCloseRange);
        if (Target == null) return;
        if (_unit.IsEnemy)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.magenta;
        }
        Gizmos.DrawLine(_unit.GetPosition(), Target.GetPosition());
    }
}
