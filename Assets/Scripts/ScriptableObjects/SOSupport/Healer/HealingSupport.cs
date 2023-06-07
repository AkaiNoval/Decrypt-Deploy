using System.Collections;
using UnityEditor;
using UnityEngine;


public enum HealSupportType
{
    HealLostHP,
    HealLostPercentageHP,
    HealLostHPAndLostPercentageHP,   
}
[CreateAssetMenu(menuName = "Support/Healer")]
public class HealingSupport : SupportAbilityBase
{
    public string SupportName;
    [TextArea]
    public string SupportDescription;
    public HealSupportType healType;
    [Header("Heal Amount")]
    public float HealingAmount;
    [Header("Heal Percentage Amount")]
    public float PercentageHealingAmount;

    public override void ApplySupport(UnitStateController unitState)
    {
        SwitchStype(unitState);
    }
    void SwitchStype(UnitStateController unitState)
    {
        switch (healType)
        {
            case HealSupportType.HealLostHP:
                HealLostHP(unitState);
                break;
            case HealSupportType.HealLostPercentageHP:
                HealLostPercentageHP(unitState);
                break;
            case HealSupportType.HealLostHPAndLostPercentageHP:
                HealLostHP(unitState);
                HealLostPercentageHP(unitState);
                break;
            default:
                break;
        }
    }
    void HealLostHP(UnitStateController unitState)
    {
        // Apply healing logic to the target unit
        unitState.Targeting.Target.GetComponent<UnitStats>().UnitCurrentHealth += HealingAmount;
    }
    void HealLostPercentageHP(UnitStateController unitState)
    {
        // Calculate the amount to heal based on the percentage of the target's missing health
        float missingHealth = unitState.Targeting.Target.GetComponent<UnitStats>().UnitMaxHealth - unitState.Targeting.Target.GetComponent<UnitStats>().UnitCurrentHealth;
        float healAmount = PercentageHealingAmount * missingHealth;

        // Apply the healing logic to the target unit
        unitState.Targeting.Target.GetComponent<UnitStats>().UnitCurrentHealth += healAmount;
    }
}


