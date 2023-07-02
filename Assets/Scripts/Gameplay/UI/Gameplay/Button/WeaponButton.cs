using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WeaponButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public static Action<WeaponButton> onSelected;
    public SOWeapon weapon;
    public Sprite unitWeaponImage;
    [SerializeField] bool isSelected;
    [SerializeField] GameObject buttonImage;
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
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected=true;
        onSelected?.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected=false;
    }
}
