using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjective : MonoBehaviour
{
    public static event Action EndGame;

    [SerializeField] bool isEnemy;
    [SerializeField] float objectiveCurrentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] int objectiveLevel;
    [SerializeField] Animator anim;
    bool isEndGame;
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public float ObjectiveCurrentHealth { get => objectiveCurrentHealth;
        set 
        {
            if(value != ObjectiveCurrentHealth)
            {
                anim.SetTrigger("HitEffect");
            }
            objectiveCurrentHealth = Mathf.Clamp(value, 0, MaxHealth);

        } 
    }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    void Start()
    {
        ObjHealthBasedOnLevel();
        ObjectiveCurrentHealth = MaxHealth;
    }
    float ObjHealthBasedOnLevel() => MaxHealth *= objectiveLevel;
    private void Update()
    {
        if (objectiveCurrentHealth <= 0 && !isEndGame)
        {
            Debug.Log("EndGameInvokeInUpdate");
            EndGame?.Invoke();
            isEndGame = true;
        }
    }
    public void TakeDamage(float damageRecieved)
    {
        ObjectiveCurrentHealth -= damageRecieved;
    }
}
