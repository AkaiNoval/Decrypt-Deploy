using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCanvasOnUnitDeath : MonoBehaviour
{
    [SerializeField] GameObject unit;
    [SerializeField] GameObject localCanvas;
    private UnitStats unitStats;

    void Awake()
    {
        unitStats = unit.GetComponent<UnitStats>();
    }

    void Update()
    {
        if (!unitStats.IsDead()) return;
        if (unitStats.IsDead() != localCanvas.activeSelf)
        {
            localCanvas.SetActive(!unitStats.IsDead());
        }
    }
}

