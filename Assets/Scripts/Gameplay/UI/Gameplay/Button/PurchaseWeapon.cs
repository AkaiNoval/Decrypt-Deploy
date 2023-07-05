using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseWeapon : MonoBehaviour
{
    [SerializeField] SOWeapon weapon;
    [SerializeField] Sprite weaponImage;
    [SerializeField] bool isSelected;
    [SerializeField] WeaponButton thisButtonWeapon;
    private void OnEnable()
    {
        WeaponButton.onSelected += ChangePurchaseWeapon;
    }
    private void OnDisable()
    {
        WeaponButton.onSelected -= ChangePurchaseWeapon;
    }
    void ChangePurchaseWeapon(WeaponButton buttonWeapon)
    {
        thisButtonWeapon = buttonWeapon;
    }
    public void UpdateImageAndWeapon() 
    {
        if (thisButtonWeapon == null) return;
        thisButtonWeapon.weapon = weapon;
        thisButtonWeapon.unitWeaponImage = weaponImage;
    }
}
