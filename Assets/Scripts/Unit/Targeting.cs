using System.Collections.Generic;
using UnityEngine;

public enum TargetSeeker
{
    SeekClosestEnemy,
    SeekClosestAlly,
    SeekClosestAndLowestHealthAlly,
    SeekClosestAndLowestHealthEnemy
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
    public UnitObjective ObjTarget { get => objectiveTarget; set => objectiveTarget = value; }
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

    private float DistanceBetweenUnitAndTarget() => DistanceToTarget = Vector3.Distance(transform.position, target.transform.position);
    private float DistanceBetweenUnitAndObject() => DistanceToObj = Vector3.Distance(transform.position, objectiveTarget.transform.position);
    private float DistanceBetweenObjectiveAndClosestTarget() => DistBetweenTargetAndObject = Vector3.Distance(target.transform.position, ObjTarget.transform.position);

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
        if (ObjTarget == null) return;
        DistanceBetweenUnitAndObject();
        if (Target == null) return;
        DistanceBetweenUnitAndTarget();
        DistanceBetweenObjectiveAndClosestTarget();
    }

    public bool GoToObjective() //Called by Moving State to get the Transform
    {
        switch (unitStats.UnitClass)
        {
            case Class.Attacker:
                // If the primary target is null or the distance to the objective is shorter than the distance to the primary target
                // Attacker units should go to the objective
                if (Target == null || DistanceToObj < DistanceToTarget)
                {
                    return true;
                }
                break;
            case Class.Supporter:
                // If the primary target is null
                // Supporter units should go to the objective
                if (Target == null)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        // If none of the conditions are met, return false
        return false;
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
