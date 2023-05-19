using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isEnemy;

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
    private void OnValidate()
    {
        /*
        By adding the OnValidate() method, it will be called whenever the Inspector values are changed. 
        This way, even if you modify the isEnemy field directly in the Inspector, 
        the UpdateUnitList() method will be triggered, 
        ensuring the unit is correctly added or removed from the appropriate list.
         */
        UpdateUnitList();
    }

    private void OnEnable()
    {
        UpdateUnitList();
    }

    private void OnDisable()
    {
        UpdateUnitList();
    }

    private void UpdateUnitList()
    {
        if (UnitManager.AllyList.Contains(this)) UnitManager.AllyList.Remove(this);
        if (UnitManager.EnemyList.Contains(this)) UnitManager.EnemyList.Remove(this);
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
}
