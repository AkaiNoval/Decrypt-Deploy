using Unity.VisualScripting;
using UnityEngine;


public class UnitStats : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] SOUnitStats soStats;
    [SerializeField] string unitName;
    [SerializeField] string unitID;
    [SerializeField] Class unitClass;
    //Fields for Properties
    [SerializeField] int unitCost;
    [SerializeField] float unitCharisma;
    [SerializeField] float preparationTime;
    [Header("Health")]
    [SerializeField] float maxHealth;   
    [SerializeField] float currentHealth;   
    [SerializeField] float healingSpeed;
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

    #region Properties
    public float UnitMaxHealth { get => maxHealth; set => maxHealth = value; }
    public float UnitCurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float UnitHealingSpeed { get => healingSpeed; set => healingSpeed = value; }
    public float UnitMorale { get => unitMorale; set => unitMorale = value; }
    public float UnitPreparationTime { get => preparationTime; set => preparationTime = value; }
    public int UnitCost { get => unitCost; set => unitCost = value; }
    public float UnitCharisma { get => unitCharisma; set => unitCharisma = value; }
    public float UnitSpeed { get => unitSpeed; set => unitSpeed = value; }
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
    #endregion

    private void Awake()
    {
        if (soStats == null)
        {
          Debug.LogWarning("Unit Stats SO is empty, please check again"); return;
        }
        unitName = soStats.name;
        unitID= soStats.ID;
        //Properties
        UnitClass = soStats.Class;
        UnitCost = soStats.Cost;
        UnitPreparationTime = soStats.PreparationTime;
        UnitMaxHealth = soStats.MaxHealth;
        UnitCurrentHealth = UnitMaxHealth;
        UnitHealingSpeed = soStats.HealingSpeed;
        UnitMorale= soStats.Morale;
        UnitCharisma= soStats.Charisma;
        UnitSpeed = soStats.Speed;
        UnitAgility = soStats.Agility;
        UnitDodgeChance = soStats.DodgeChance;
        UnitCloseRange= soStats.CloseRange;
        UnitFarRange = soStats.FarRange;
        UnitAccuracy = soStats.Accuracy;
        UnitCriticalChance = soStats.CriticalChance;
        UnitCriticalDamage = soStats.CriticalDamage;
        //Damage Type
        UnitMeleeDamage = soStats.MeleeDamage;
        UnitRangeDamage = soStats.RangeDamage;
        UnitPoisonDamage = soStats.PoisonDamage;
        UnitFireDamage = soStats.FireDamage;
        UnitCryoDamage = soStats.CryoDamage;
        UnitElectrifiedDamage = soStats.ElectrifiedDamage;
        UnitExplosionDamage = soStats.ExplosionDamage;
        //Resistance
        UnitBulletResistance = soStats.BulletResistance;
        UnitMeleeResistance = soStats.MeleeResistance;
        UnitPoisonResistance = soStats.PoisonResistance;
        UnitFireResistance = soStats.FireResistance;
        UnitCryoResistance = soStats.CryoResistance;
        UnitElectrifiedResistance = soStats.ElectrifiedResistance;
        UnitExplosionResistance = soStats.ExplosionResistance;
    }
}
