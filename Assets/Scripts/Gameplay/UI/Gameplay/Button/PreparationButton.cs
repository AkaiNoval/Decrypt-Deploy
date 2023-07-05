using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PreparationButton : MonoBehaviour
{
    [SerializeField] InstantiateUnit instantiateUnit;
    [SerializeField] Button button;
    [SerializeField] Image unitPortrait;
    [SerializeField] float waitTime;
    void Start()
    {
        if (instantiateUnit.unitStats == null) return;
        button.interactable = false;
        unitPortrait.color = new Color(unitPortrait.color.r, unitPortrait.color.g, unitPortrait.color.b, 100f / 255f);
        waitTime = instantiateUnit.unitStats.PreparationTime;
        
    }
    private void OnEnable()
    {
        GameManager.GameStarted += StartCoroutine;
    }
    private void OnDisable()
    {
        GameManager.GameStarted -= StartCoroutine;
    }
    void StartCoroutine()
    {
        StartCoroutine(UnitPreparationTime());
    }
    IEnumerator UnitPreparationTime()
    {
        Debug.Log("Start Preparing Unit");
        yield return new WaitForSeconds(waitTime);
        button.interactable = true;
        unitPortrait.color = new Color(unitPortrait.color.r, unitPortrait.color.g, unitPortrait.color.b, 255f / 255f);
    }
    public void RestartPreparationTime()
    {
        StartCoroutine(UnitPreparationTime());
        button.interactable = false;
        unitPortrait.color = new Color(unitPortrait.color.r, unitPortrait.color.g, unitPortrait.color.b, 100f / 255f);
    }
}
