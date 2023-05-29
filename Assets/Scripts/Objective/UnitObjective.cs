using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjective : MonoBehaviour
{

    [SerializeField] bool isEnemy;
    [SerializeField] float objectiveHealth;
    [SerializeField] int objectiveLevel;
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }

    void Start()
    {
        ObjHealthBasedOnLevel();
    }
    float ObjHealthBasedOnLevel() => objectiveHealth *= objectiveLevel;
}
