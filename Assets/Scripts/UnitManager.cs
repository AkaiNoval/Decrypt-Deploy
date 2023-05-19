using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static List<Unit> AllyList = new List<Unit>();
    public static List<Unit> EnemyList = new List<Unit>();
    public List<string> allyListName;
    public List<string> enemyListName;
    [SerializeField] int allyCount;
    [SerializeField] int enemyCount;

    void Start()
    {
        allyCount = AllyList.Count;
        enemyCount = EnemyList.Count;
    }
    void Update()
    {
        CheckListChanges();
    }
    void CheckListChanges()
    {
        if (AllyList.Count != allyCount || EnemyList.Count != enemyCount)
        {
            allyCount = AllyList.Count;
            enemyCount = EnemyList.Count;
        }
        AddUnitsNameToList();
    }
    void AddUnitsNameToList()
    {
        allyListName.Clear();
        enemyListName.Clear();  
        foreach (var unit in AllyList)
        {
            if (unit != null)
            {
                allyListName.Add(unit.name);
            }        
        }
        foreach (var unit in EnemyList)
        {
            if(unit != null)
            {
                enemyListName.Add(unit.name);
            }          
        }
    }

}
