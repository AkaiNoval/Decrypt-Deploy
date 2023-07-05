using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateUnit : MonoBehaviour
{
    public SOUnitStats unitStats;
    [SerializeField] SOWeapon unitWeapon;
    [SerializeField] PreparationButton preparationButton;
    [SerializeField] PopUpWindow popUpWindow;
    [SerializeField] List<GameObject> Units= new List<GameObject>();  
    [SerializeField] int slot;
    [SerializeField] GameObject spawnPosition;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    private void Start()
    {
        if (slot >= PlayerData.Instance.UnitData.Count) return;
        unitStats = PlayerData.Instance.UnitData[slot];  
    }
    public void SpawnUnit()
    {
        if (unitStats == null) return;
        unitWeapon = GetComponentInChildren<WeaponButton>().weapon;
        
        if (CurrencyManager.Instance.PlayerClonite >= unitStats.Clonite && CurrencyManager.Instance.PlayerScrap >= unitWeapon.ScrapCost)
        {
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(spawnPosition.transform.position.x, randomY, spawnPosition.transform.position.z);
            foreach (var unit in Units)
            {
                if (unit.TryGetComponent(out UnitStats unitStats) == unitStats)
                {
                    var spawnedUnit = Instantiate(unit, spawnPos, Quaternion.identity);
                    spawnedUnit.GetComponent<UnitStats>().Weapon = unitWeapon;
                    Debug.Log("Spawned " + spawnedUnit + " with " + unitWeapon);
                }
            }
            CurrencyManager.Instance.PlayerClonite -= unitStats.Clonite;
            CurrencyManager.Instance.PlayerScrap -= unitWeapon.ScrapCost;
            preparationButton.RestartPreparationTime();
        }
        else
        {
            popUpWindow.ShowPopup("Unable to clone due to a shortage of necessary materials.");
        }

    }
    public void GetWeapon(SOWeapon Weapon)
    {
        unitWeapon = Weapon;
    }
    private void OnDrawGizmos()
    {
        if (spawnPosition == null) return;
        Vector3 pos1 = new Vector3(spawnPosition.transform.position.x, minY, spawnPosition.transform.position.z);
        Vector3 pos2 = new Vector3(spawnPosition.transform.position.x, maxY, spawnPosition.transform.position.z);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pos1, pos2);
    }

}
