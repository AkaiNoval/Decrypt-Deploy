using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCombatPurpose
{
    Attack,
    Support
}
public class UnitBehaviourController : MonoBehaviour
{
    UnitStats unitStats;
    [SerializeField] UnitCombatPurpose combatPurpose;
    CombatBehaviour combatBehaviour;
    SupportBehaviour supportBehaviour;
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        combatBehaviour = GetComponent<CombatBehaviour>();
        supportBehaviour = GetComponent<SupportBehaviour>();

    }
    private void Update()
    {
        SetCombatBehavior();
    }
    private void SetCombatBehavior()
    {
        // Determine the appropriate combat behavior based on the combat purpose
        switch (combatPurpose)
        {
            case UnitCombatPurpose.Attack:
                combatBehaviour.ExecuteCombat();
                break;
            case UnitCombatPurpose.Support:
                supportBehaviour.ExecuteSupport();
                break;
        }
    }


}
