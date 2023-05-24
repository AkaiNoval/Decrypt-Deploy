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
    [SerializeField] Unit target;  // The current target
    [SerializeField] float range = 10f;  // The maximum range for selecting a target
    [SerializeField] float distance;  // The maximum range for selecting a target

    public Unit Target
    {
        get { return target; }
        set { target = value; }
    }
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        unitStats= GetComponent<UnitStats>();
    }
    private void Update()
    {
        // Update the current target by finding the closest unit within range
        Target = GetClosestUnit(_unit.GetPosition(), range);
        DistanceBetweenTarget();
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

    private void DistanceBetweenTarget()
    {
        if(target==null) return;
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= unitStats.UnitFarRange && distance > unitStats.UnitCloseRange)
        {
            unitStats.UnitClass = Class.Shooter;
        }
        else if(distance <= unitStats.UnitCloseRange)
        {
            unitStats.UnitClass = Class.Fighter;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        if (_unit.IsEnemy)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.cyan;
        }
        Gizmos.DrawWireSphere(_unit.GetPosition(), range);
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
