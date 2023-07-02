using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] Image whiteLostHealthBar;
    [SerializeField] Image redHealthBar; 
    [SerializeField] UnitStats unitStats;
    [SerializeField] float smoothSpeed;

    private void Update()
    {
        redHealthBar.fillAmount = unitStats.UnitCurrentHealth / unitStats.UnitMaxHealth;
        LostHealthMask();
    }

    void LostHealthMask()
    {
        if(whiteLostHealthBar.fillAmount > redHealthBar.fillAmount)
        {
            whiteLostHealthBar.fillAmount -= smoothSpeed;
        }
        else
        {
            whiteLostHealthBar.fillAmount = redHealthBar.fillAmount;
        }
    }
}
