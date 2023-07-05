using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<SOUnitStats> UnitData = new List<SOUnitStats>();
    public List<SOWeapon> WeaponData = new List<SOWeapon>();
    #region Singleton
    // Singleton instance
    private static PlayerData instance;

    // Public access to the singleton instance
    public static PlayerData Instance
    {
        get { return instance; }
    }
    #endregion

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this object
            Destroy(gameObject);
            return;
        }

        // If no instance exists, set this as the instance
        instance = this;
    }
}
