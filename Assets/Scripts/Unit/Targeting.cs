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
    [SerializeField] UnitObjective objTarget;
    UnitObjective[] objTargets;
    [SerializeField] float range = 10f;  // The maximum range for selecting a target
    [SerializeField] float distanceToTarget;
    [SerializeField] float distanceToObjective;
    [SerializeField] float distanceBetweenTargetAndObject;

    public Unit Target { get { return target; } set { target = value; } }
    public UnitObjective ObjTarget { get => objTarget; set => objTarget = value; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float DistanceToObj { get => distanceToObjective; set => distanceToObjective = value; }
    public float DistBetweenTargetAndObject { get => distanceBetweenTargetAndObject; set => distanceBetweenTargetAndObject = value; }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        unitStats = GetComponent<UnitStats>();
        objTargets = FindObjectsOfType<UnitObjective>();
        foreach (var unitObj in objTargets)
        {
            if ((!_unit.IsEnemy && unitObj.IsEnemy) || (_unit.IsEnemy && !unitObj.IsEnemy))
            {
                objTarget = unitObj;
                break; // Exit the loop once a suitable unitObjective is found
            }
        }
        CalcDist();
    }



    private void Update()
    {
        // Update the current target by finding the closest unit within range
        Target = GetClosestUnit(_unit.GetPosition(), range);
        CalcDist();
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
    private float DistanceBetweenUnitAndObject() => DistanceToObj = Vector3.Distance(transform.position, objTarget.transform.position);
    private float DistanceBetweenObjectiveAndClosestTarget() => DistBetweenTargetAndObject = Vector3.Distance(target.transform.position, ObjTarget.transform.position);

    private void CalcDist()
    {
        if (ObjTarget == null) return;
        DistanceBetweenUnitAndObject();
        if (Target == null) return;
        DistanceBetweenUnitAndTarget();
        DistanceBetweenObjectiveAndClosestTarget();
    }

    public bool GoToObjective()
    {
        if (Target == null)
        {
            return true;
        }
        else if (DistanceToObj <= DistanceToTarget)
        {
            return true;
        }
        else if(DistanceToTarget < DistanceToObj)
        {
            return false;
        }
        return true; 
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
