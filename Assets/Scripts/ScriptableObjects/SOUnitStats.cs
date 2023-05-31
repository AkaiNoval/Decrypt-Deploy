using UnityEngine;

public enum Class
{
    Attacker,
    Supporter
}
[CreateAssetMenu(fileName = "Stats", menuName = "Unit Stats")]
public class SOUnitStats : ScriptableObject
{
    [Header("Info")]
    public string Name; //Lock
    public string ID; //Lock => Bester ID management
    [TextArea]
    public string Description; //Lock
    public Class Class; //Lock
    public float PreparationTime;//CanChange
    public int Cost;//CanChange
    [Header("Health")]
    public float MaxHealth;//CanChange
    public float HealingSpeed;//CanChange
    [Header("Basic")]
    public float Morale;
    public float Charisma;//Lock
    public float Speed;
    public float Agility;
    public float DodgeChance;
    [Header("Damage Attributes")]
    public float CloseRange;
    public float FarRange;
    public float Accuracy;
    public float CriticalChance;
    public float CriticalDamage;
    [Header("Damage")]
    public float MeleeDamage;
    public float RangeDamage;
    public float PoisonDamage;
    public float FireDamage;
    public float CryoDamage;
    public float ElectrifiedDamage;
    public float ExplosionDamage;
    [Header("DamageResistance")]
    public float BulletResistance;
    public float MeleeResistance;
    public float PoisonResistance;
    public float FireResistance;
    public float CryoResistance;
    public float ElectrifiedResistance;
    public float ExplosionResistance;

}
