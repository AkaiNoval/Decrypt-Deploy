using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjective : MonoBehaviour
{

    [SerializeField] bool isEnemy;
    [SerializeField] float objectiveCurrentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] int objectiveLevel;
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public float ObjectiveCurrentHealth { get => objectiveCurrentHealth; set => objectiveCurrentHealth = Mathf.Clamp(value, 0, maxHealth); }

    void Start()
    {
        ObjHealthBasedOnLevel();
        ObjectiveCurrentHealth = maxHealth;
    }
    float ObjHealthBasedOnLevel() => maxHealth *= objectiveLevel;

    public void TakeDamage(float damageRecieved)
    {
        ObjectiveCurrentHealth -= damageRecieved;
    }
}
