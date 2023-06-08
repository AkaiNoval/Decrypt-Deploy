using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SOWeapon : ScriptableObject
{
    public Sprite WeaponSprite;
    public RuntimeAnimatorController runtimeAnimController;
    [Header("Critical")]
    public float unitCriticalChance;
    public float unitCriticalDamage;
    [Header("Affection")]
    public float DodgeChance;
    public float Speed;
    public float Accuracy;
    [Header("Damage Attribute")]
    public float unitMeleeDamage;
    public float unitRangeDamage;
    public float unitPoisonDamage;
    public float unitFireDamage;
    public float unitCryoDamage;
    public float unitElectrifiedDamage;
    public float unitExplosionDamage;
}
