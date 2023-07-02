using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnButtonsController : MonoBehaviour
{
    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] List<TMP_Text> clonieTexts = new List<TMP_Text>();
    //TESTING ONLY DO NOT USE THIS AFTER THE DEMO, USE JSON INSTEAD
    private void Start()
    {
        if(PlayerData.Instance.UnitData.Count > 0)
        {
            SetImageAndCost();
            DisableAlpha();
        }
        else
        {
            Debug.LogWarning("Look like player doesn't have any Unit, please verify");
        }

    }
    void SetImageAndCost()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (i >= PlayerData.Instance.UnitData.Count) return;
            images[i].sprite = PlayerData.Instance.UnitData[i].Portrait;
            if (clonieTexts[i] == null) return;
            clonieTexts[i].text = PlayerData.Instance.UnitData[i].Clonite.ToString();
        }
    }
    void DisableAlpha()
    {
        foreach (var image in images)
        {
            if (image.sprite != null) continue;
            Color imageColor = image.color;
            // Set alpha to zero
            imageColor.a = 0f;
            image.color = imageColor;        
        }
    }
}
