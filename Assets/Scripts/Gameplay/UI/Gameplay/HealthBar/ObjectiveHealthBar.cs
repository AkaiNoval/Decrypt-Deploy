using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHealthBar : MonoBehaviour
{
    [SerializeField] Image whiteLostHealthBar;
    [SerializeField] Image redHealthBar;
    [SerializeField] UnitObjective unitObjective;
    [SerializeField] float smoothSpeed;
    private void Update()
    {
        redHealthBar.fillAmount = unitObjective.ObjectiveCurrentHealth / unitObjective.MaxHealth;
        LostHealthMask();
    }

    void LostHealthMask()
    {
        if (whiteLostHealthBar.fillAmount > redHealthBar.fillAmount)
        {
            whiteLostHealthBar.fillAmount -= smoothSpeed;
        }
        else
        {
            whiteLostHealthBar.fillAmount = redHealthBar.fillAmount;
        }
    }
}


