using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance { get; private set; }

    // Lists to store ally and enemy deaths
    public List<GameObject> AllyDeathList = new List<GameObject>();
    public List<GameObject> EnemyDeathList = new List<GameObject>();
    [SerializeField] List<DeathInfo> deathInfos = new List<DeathInfo>();

    [Serializable]
    private class DeathInfo
    {
        public string Name;
        public bool WasEnemy;
        [TextArea]
        public string CauseOfDeath;
        public int KillCount;
        public float DamageDealt;
        
    }
    private void Awake()
    {
        // Ensure only one instance of DeathManager exists
        if (Instance == null)
        {
            // Set the instance to this if it doesn't exist
            Instance = this;
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    public void AddToDeathList(GameObject unit)
    {
        DeathInfo deathInfo = new DeathInfo();
        UnitStats unitStats = unit.GetComponent<UnitStats>();
        Unit unitInfo = unit.GetComponent<Unit>();
        KillCounter killCounter = unit.GetComponent<KillCounter>();
        deathInfo.Name = unitStats.SoStats.Name;
        deathInfo.WasEnemy = unitInfo.IsEnemy;
        //deathInfo.CauseOfDeath = unit.CauseOfDeath;
        deathInfo.KillCount = killCounter.KillsList.Count;
        deathInfo.DamageDealt = killCounter.DamageDealt;

        deathInfos.Add(deathInfo); // Add the new DeathInfo to the array

        if (!unitInfo.IsEnemy)
        {
            AllyDeathList.Add(unit);
        }
        else
        {
            EnemyDeathList.Add(unit);
            CurrencyManager.Instance.PlayerScrap += 5;
        }
    }
}

