using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class UnitStats : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] SOUnitStats soStats;
    [SerializeField] string unitName;
    [SerializeField] string unitID;
    [SerializeField] Class unitClass;
    [Header("Abilities")]
    public SupportAbilityBase SupportType;
    public PassiveAbilityBase PassiveAbility;
    public ActiveAbilityBase ActiveAbility;
    //Fields for Properties
    [Header("Weapon")]
    public SOWeapon Weapon;
    SOWeapon previousWeapon;
    [Header("Info")]
    [SerializeField] int unitCost;
    [SerializeField] float unitCharisma;
    [SerializeField] float preparationTime;
    [SerializeField] float damageDealt;
    [TextArea][SerializeField] string causeOfDeath;
    [Header("Health")]
    [SerializeField] float maxHealth;   
    [SerializeField] float currentHealth;   
    [SerializeField] float healingAmountPerSecond;
    [Header("Basic")]
    [SerializeField] float unitMorale;
    [SerializeField] float unitSpeed;
    [SerializeField] float unitAgility;
    [SerializeField] float unitDodgeChance;
    [Header("Damage Attributes")]
    [SerializeField] float unitCloseRange;
    [SerializeField] float unitFarRange;
    [SerializeField] float unitAccuracy;
    [SerializeField] float unitCriticalChance;
    [SerializeField] float unitCriticalDamage;
    [Header("Damage")]
    [SerializeField] float unitMeleeDamage;
    [SerializeField] float unitRangeDamage;
    [SerializeField] float unitPoisonDamage;
    [SerializeField] float unitFireDamage;
    [SerializeField] float unitCryoDamage;
    [SerializeField] float unitElectrifiedDamage;
    [SerializeField] float unitExplosionDamage;
    [Header("Damage Resistance")]
    [SerializeField] float unitBulletResistance;
    [SerializeField] float unitMeleeResistance;
    [SerializeField] float unitPoisonResistance;
    [SerializeField] float unitFireResistance;
    [SerializeField] float unitCryoResistance;
    [SerializeField] float unitElectrifiedResistance;
    [SerializeField] float unitExplosionResistance;


    [SerializeField] Animator animator;
    #region Properties
    public SOUnitStats SoStats { get => soStats; private set => soStats = value; }
    public float UnitMaxHealth { get => maxHealth; set => maxHealth = value; }
    public float UnitCurrentHealth { get => currentHealth; 
        set 
        { 
            
            currentHealth = Mathf.Clamp(value, 0, maxHealth); 
        } 
    }
    public float HealingAmountPerSecond { get => healingAmountPerSecond; set => healingAmountPerSecond = value; }
    public float UnitMorale { get => unitMorale; set => unitMorale = value; }
    public float UnitPreparationTime { get => preparationTime; set => preparationTime = value; }
    public int UnitCost { get => unitCost; set => unitCost = value; }
    public float UnitCharisma { get => unitCharisma; set => unitCharisma = value; }
    public float UnitSpeed { get => unitSpeed; set => unitSpeed = Mathf.Clamp(value,0.1f,3f); }
    public float UnitAgility { get => unitAgility; set => unitAgility = value; }
    public float UnitDodgeChance { get => unitDodgeChance; set => unitDodgeChance = value; }
    public float UnitCloseRange { get => unitCloseRange; set => unitCloseRange = value; }
    public float UnitFarRange { get => unitFarRange; set => unitFarRange = value; }
    public float UnitAccuracy { get => unitAccuracy; set => unitAccuracy = value; }
    public float UnitCriticalChance { get => unitCriticalChance; set => unitCriticalChance = value; }
    public float UnitCriticalDamage { get => unitCriticalDamage; set => unitCriticalDamage = value; }
    //Damage Type
    public float UnitMeleeDamage { get => unitMeleeDamage; set => unitMeleeDamage = value; }
    public float UnitRangeDamage { get => unitRangeDamage; set => unitRangeDamage = value; }
    public float UnitPoisonDamage { get => unitPoisonDamage; set => unitPoisonDamage = value; }
    public float UnitFireDamage { get => unitFireDamage; set => unitFireDamage = value; }
    public float UnitCryoDamage { get => unitCryoDamage; set => unitCryoDamage = value; }
    public float UnitElectrifiedDamage { get => unitElectrifiedDamage; set => unitElectrifiedDamage = value; }
    public float UnitExplosionDamage { get => unitExplosionDamage; set => unitExplosionDamage = value; }
    //Resistance
    public float UnitBulletResistance { get => unitBulletResistance; set => unitBulletResistance = value; }
    public float UnitMeleeResistance { get => unitMeleeResistance; set => unitMeleeResistance = value; }
    public float UnitPoisonResistance { get => unitPoisonResistance; set => unitPoisonResistance = value; }
    public float UnitFireResistance { get => unitFireResistance; set => unitFireResistance = value; }
    public float UnitCryoResistance { get => unitCryoResistance; set => unitCryoResistance = value; }
    public float UnitElectrifiedResistance { get => unitElectrifiedResistance; set => unitElectrifiedResistance = value; }
    public float UnitExplosionResistance { get => unitExplosionResistance; set => unitExplosionResistance = value; }
    public Class UnitClass { get => unitClass; set => unitClass = value; }
    public string CauseOfDeath { get => causeOfDeath; set => causeOfDeath = value; }
    #endregion

    public bool IsDead()
    {
        if (UnitCurrentHealth <= 0)
        {
            return true; // The unit is dead
        }
        else
        {
            return false; // The unit is not dead
        }
    }
    bool wasDead = false;
    private void Awake()
    {
        if (SoStats == null)
        {
            Debug.LogWarning("Unit Stats SO is empty, please check again"); return;
        }
        GetBaseStats();
        if (Weapon != null)
        {
            previousWeapon = Weapon;
            // Increase the stats based on the referenced weapon
            IncreaseStatsFromWeapon(Weapon);
        }
    }

    private void GetBaseStats()
    {
        unitName = SoStats.name;
        unitID = SoStats.ID;
        //Properties
        UnitClass = SoStats.Class;
        UnitCost = SoStats.Clonite;
        UnitPreparationTime = SoStats.PreparationTime;
        UnitMaxHealth = SoStats.MaxHealth;
        UnitCurrentHealth = UnitMaxHealth;
        HealingAmountPerSecond = SoStats.HealingAmountPerSecond;
        UnitMorale = SoStats.Morale;
        UnitCharisma = SoStats.Charisma;
        UnitSpeed = SoStats.Speed;
        UnitAgility = SoStats.Agility;
        UnitDodgeChance = SoStats.DodgeChance;
        UnitCloseRange = SoStats.CloseRange;
        UnitFarRange = SoStats.FarRange;
        UnitAccuracy = SoStats.Accuracy;
        UnitCriticalChance = SoStats.CriticalChance;
        UnitCriticalDamage = SoStats.CriticalDamage;
        //Damage Type
        UnitMeleeDamage = SoStats.MeleeDamage;
        UnitRangeDamage = SoStats.RangeDamage;
        UnitPoisonDamage = SoStats.PoisonDamage;
        UnitFireDamage = SoStats.FireDamage;
        UnitCryoDamage = SoStats.CryoDamage;
        UnitElectrifiedDamage = SoStats.ElectrifiedDamage;
        UnitExplosionDamage = SoStats.ExplosionDamage;
        //Resistance
        UnitBulletResistance = SoStats.BulletResistance;
        UnitMeleeResistance = SoStats.MeleeResistance;
        UnitPoisonResistance = SoStats.PoisonResistance;
        UnitFireResistance = SoStats.FireResistance;
        UnitCryoResistance = SoStats.CryoResistance;
        UnitElectrifiedResistance = SoStats.ElectrifiedResistance;
        UnitExplosionResistance = SoStats.ExplosionResistance;
    }
    void IncreaseStatsFromWeapon(SOWeapon weapon)
    {
        UnitMeleeDamage += weapon.unitMeleeDamage;
        UnitRangeDamage += weapon.unitRangeDamage;
        UnitPoisonDamage += weapon.unitPoisonDamage;
        UnitFireDamage += weapon.unitFireDamage;
        UnitCryoDamage += weapon.unitCryoDamage;
        UnitElectrifiedDamage += weapon.unitElectrifiedDamage;
        UnitExplosionDamage += weapon.unitExplosionDamage;
        UnitCriticalChance += weapon.unitCriticalChance;
        UnitCriticalDamage += weapon.unitCriticalDamage;
        UnitDodgeChance += weapon.DodgeChance;
        UnitSpeed += weapon.Speed;
        UnitAccuracy += weapon.Accuracy;
        UnitCloseRange += weapon.CloseRange;
        UnitFarRange += weapon.FarRange;
    }
    void Update()
    {
        OnDeath();
        if (IsDead()) return;
        PassiveHealing();
        SwitchWeapon(Weapon);
    }
    void OnDeath()
    {
        if (IsDead() && !wasDead)
        {
            wasDead= true;
            Collider2D collider2D = gameObject.GetComponent<Collider2D>();
            collider2D.enabled = false;
            DeathManager.Instance.AddToDeathList(gameObject);
            return;
        }
    }
    void PassiveHealing() => UnitCurrentHealth = Mathf.Clamp(UnitCurrentHealth + HealingAmountPerSecond * Time.deltaTime, 0, UnitMaxHealth);
    
    void SwitchWeapon(SOWeapon weapon)
    {
        // Check if the new weapon is the same as the previous weapon
        if (previousWeapon == weapon)
        {
            // If it is the same, no need to switch, so return
            return;
        }
        // Store the new weapon as the previous weapon
        previousWeapon = weapon;
        // Increase the unit's stats based on the new weapon
        GetBaseStats();
        IncreaseStatsFromWeapon(weapon);
    }

    #region TakeMeleeDamage
    public float CalculateReducedDamage(float incomingDamage, float resistanceType, bool isCritical)
    {
        float damageReduction = resistanceType / 100f; // Convert percentage to decimal
        float reducedDamage = incomingDamage * (1f - damageReduction);
        if (isCritical)
        {
            //CriticalDamageTextPopUp
            Debug.Log("isCritical");
        }
        animator.SetTrigger("HitEffect");
        return Mathf.RoundToInt(reducedDamage);
    }
    #endregion


    #region TakeExplosionDamage
    public void TakeExplosionDamage(float damage, float resistanceType)
    {
        float effectiveDamage = damage * (1f - unitExplosionResistance / 100f);
        UnitCurrentHealth -= effectiveDamage;
    } 
    #endregion
    

}
