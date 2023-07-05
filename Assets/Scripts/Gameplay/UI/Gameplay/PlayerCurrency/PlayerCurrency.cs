using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] bool metalScrap;
    // Update is called once per frame
    void Update()
    {
        if(!metalScrap)
        {
            text.text = CurrencyManager.Instance.PlayerClonite.ToString();
        }
        else
        {
            text.text = CurrencyManager.Instance.PlayerScrap.ToString();
        }
    }
}
