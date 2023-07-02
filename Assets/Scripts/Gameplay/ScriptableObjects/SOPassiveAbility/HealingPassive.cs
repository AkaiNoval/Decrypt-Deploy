using System.Collections;
using UnityEngine;

public enum HealPassiveType
{
    HealLostHP,
    HealLostPercentageHP,
    HealLostHPAndLostPercentageHP,
    HealthBoost
}

[CreateAssetMenu(menuName = "PassiveAbility/Healing")]
public class HealingPassive : PassiveAbilityBase
{
    public string PassiveAbilityName;
    [TextArea]
    public string PassiveAbilityDescription;
    public float triggerInterval;
    public HealPassiveType type;
    [Header("Heal Amount")]
    public float HealingAmount;
    [Header("Heal Percentage Amount")]
    public float PercentageHealingAmount;
    [Header("Health Boost Amount")]
    public float HealthBoostAmount;
    public float HealthBoostDuration;
    public override float TriggerInterval { get => triggerInterval; set => triggerInterval = value; }

    #region Switch Type based on Enum Type
    void SwitchType(UnitStateController unit)
    {
        switch (type)
        {
            case HealPassiveType.HealLostHP:
                HealLostHP(unit);
                break;
            case HealPassiveType.HealLostPercentageHP:
                HealLostPercentageHP(unit);
                break;
            case HealPassiveType.HealLostHPAndLostPercentageHP:
                HealLostHP(unit);
                HealLostPercentageHP(unit);
                break;
            case HealPassiveType.HealthBoost:
                BoostHealth(unit);
                break;
            default:
                break;
        }
    } 
    #endregion

    #region RestoreLostHP
    void HealLostHP(UnitStateController unitState)
    {
        // Apply healing logic to the target unit
        unitState.UnitStats.UnitCurrentHealth += HealingAmount;
    } 
    #endregion

    #region Restore Lost HP Based On Percentage
    void HealLostPercentageHP(UnitStateController unitState)
    {
        // Calculate the amount to heal based on the percentage of the target's missing health
        float missingHealth = unitState.UnitStats.UnitMaxHealth - unitState.UnitStats.UnitCurrentHealth;
        float healAmount = PercentageHealingAmount * missingHealth;

        // Apply the healing logic to the target unit
        unitState.UnitStats.UnitCurrentHealth += healAmount;
    } 
    #endregion

    #region Restore Health and Boost Max HP
    void BoostHealth(UnitStateController unitState)
    {
        // Store the original max health before applying the boost
        float originalMaxHealth = unitState.UnitStats.UnitMaxHealth;
        // Increase the unit's current health by the HealthBoostAmount
        unitState.UnitStats.UnitCurrentHealth += HealthBoostAmount;
        // Increase the unit's max health by the HealthBoostAmount
        unitState.UnitStats.UnitMaxHealth += HealthBoostAmount;
        // Set a coroutine to revert the max health back to its original value after the HealthBoostDuration
        unitState.StartCoroutine(RevertMaxHealth(unitState, originalMaxHealth, HealthBoostDuration));
    }
    IEnumerator RevertMaxHealth(UnitStateController unitState, float originalMaxHealth, float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);
        // Revert the unit's max health back to its original value
        unitState.UnitStats.UnitMaxHealth = originalMaxHealth;
    }
    #endregion
    public override void ApplyPassiveAbility(UnitStateController unit)
    {
        SwitchType(unit);
    }
    public override bool ConditionCheck(UnitStateController unitState)
    {
        if (type == HealPassiveType.HealthBoost) return true;
        if (unitState.UnitStats.UnitCurrentHealth < unitState.UnitStats.UnitMaxHealth && unitState.UnitStats.UnitCurrentHealth > 0) return true;
        else return false;
    }

}
