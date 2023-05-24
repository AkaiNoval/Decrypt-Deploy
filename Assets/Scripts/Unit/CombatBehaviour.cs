using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatBehaviour : MonoBehaviour
{
    UnitStats unitStats;
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
    }
    public void ExecuteCombat()
    {
        if (unitStats.UnitClass == Class.Shooter)
        {
            FarRangeCombat();
        }
        else if (unitStats.UnitClass == Class.Fighter)
        {
            CloseRangeCombat();
        }
    }
    void FarRangeCombat()
    {
        Debug.Log("Doing Far Range Combat");
    }
    void CloseRangeCombat()
    {
        Debug.Log("Doing Close Range Combat");
    }
}
