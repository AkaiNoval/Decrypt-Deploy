using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class WeaponButton : MonoBehaviour, ISelectHandler
{
    public static Action<WeaponButton> onSelected;
    public SOWeapon weapon;
    public Sprite unitWeaponImage;
    [SerializeField] GameObject buttonImage;
    [SerializeField] TMP_Text weaponScrapCost;
    Sprite previousWeaponImage;
    private void Start()
    {
        previousWeaponImage = unitWeaponImage;
    }
    private void Update()
    {
        if(previousWeaponImage != unitWeaponImage)
        {
            previousWeaponImage = unitWeaponImage;
            buttonImage.GetComponent<Image>().sprite = unitWeaponImage;
            weaponScrapCost.text = weapon.ScrapCost.ToString();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelected?.Invoke(this);
    }
}
