using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool isEnemy;
    UnitStats unitStats;
    public bool IsEnemy
    {
        get { return isEnemy; } // Getter for the isEnemy flag
        set
        {
            if (isEnemy != value)
            {
                isEnemy = value; // Update the isEnemy flag with the new value
                UpdateUnitList(); // Call the method to update the unit list based on the new value
            }
        }
    }
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
    }
    private void OnValidate() => UpdateUnitList();
    private void OnEnable() => UpdateUnitList();
    private void OnDisable() => UpdateUnitList();
    private void UpdateUnitList()
    {
        if (UnitManager.AllyList.Contains(this)) UnitManager.AllyList.Remove(this);
        if (UnitManager.EnemyList.Contains(this)) UnitManager.EnemyList.Remove(this);
        //if(IsDead) return;
        if (isEnemy)
        {
            if (!gameObject.activeInHierarchy) return;
            UnitManager.EnemyList.Add(this);                       
        }
        else
        {
            if (!gameObject.activeInHierarchy) return;
            UnitManager.AllyList.Add(this);          
        }
    }

    public Vector3 GetPosition() => transform.position;
    public bool IsDead() => unitStats.UnitCurrentHealth <= 0;
}
