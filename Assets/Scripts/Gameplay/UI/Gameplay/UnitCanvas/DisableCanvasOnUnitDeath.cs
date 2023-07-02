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
        localCanvas.SetActive(false);
    }
}

