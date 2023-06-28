using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemy;
    [SerializeField] GameObject AbilityIcon;
    UnitStats unitStats;
    public bool IsEnemy
    {
        get { return isEnemy; }
        private set
        {
            Debug.Log("Hi");
            isEnemy = value;
        }
    }
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
    }

    private void Update()
    {
        HideUIEnemy();
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

        if (unitStats.IsDead())
            return; // Exit the method if the unit is dead

        if (!gameObject.activeInHierarchy)
            return; // Exit the method if the game object is not active

        if (isEnemy)
            UnitManager.EnemyList.Add(this); // Add the unit to EnemyList
        else
            UnitManager.AllyList.Add(this); // Add the unit to AllyList
    }
    void HideUIEnemy()
    {  
        if (AbilityIcon != null)
        {

            if (isEnemy)
            {
                if (!isEnemy) return;
                AbilityIcon.SetActive(false);
            }
            else
            {
                AbilityIcon.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("AbilityIcon is not assigned");
        }
    }
    public Vector3 GetPosition() => transform.position;
    public bool IsDead() => unitStats.IsDead();

}

