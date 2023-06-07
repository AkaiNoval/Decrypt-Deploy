using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemy;
    UnitStats unitStats;
    bool hasUnitStats; // Flag to indicate if the unit has UnitStats component

    public bool IsEnemy
    {
        get { return isEnemy; }
        set
        {
            if (isEnemy != value)
            {
                isEnemy = value;
                UpdateUnitList();
            }
        }
    }
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        hasUnitStats = unitStats != null; // Check if the unit has UnitStats component
    }

    private void Update()
    {
        UpdateUnitList();
    }
    private void OnDisable()
    {
        UnitManager.AllyList.Remove(this); // Remove the unit from AllyList
        UnitManager.EnemyList.Remove(this); // Remove the unit from EnemyList
    }
    private void UpdateUnitList()
    {
        UnitManager.AllyList.Remove(this); // Remove the unit from AllyList
        UnitManager.EnemyList.Remove(this); // Remove the unit from EnemyList

        if (IsDead())
            return; // Exit the method if the unit is dead

        if (!gameObject.activeInHierarchy)
            return; // Exit the method if the game object is not active

        if (isEnemy)
            UnitManager.EnemyList.Add(this); // Add the unit to EnemyList
        else
            UnitManager.AllyList.Add(this); // Add the unit to AllyList
    }

    public Vector3 GetPosition() => transform.position;

    public bool IsDead()
    {
        if (hasUnitStats && unitStats.UnitCurrentHealth <= 0)
        {
            return true; // The unit is dead
        }
        else
        {
            return false; // The unit is not dead
        }
    }
}

