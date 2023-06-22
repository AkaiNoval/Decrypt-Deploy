using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathType
{
    None,
    Melee,
    Bullet,
    Explosion,
    Poison,
    Fire,
    Cryo,
    Electric,
}
[CreateAssetMenu(fileName ="Cause of Death",menuName = "Cause of Death")]
public class SOCauseOfDeath : ScriptableObject
{

    [TextArea] public List<string> meleeDeath = new List<string>();
    [TextArea] public List<string> bulletDeath = new List<string>();
    [TextArea] public List<string> explosionDeath = new List<string>();
    [TextArea] public List<string> poisonDeath = new List<string>();
    [TextArea] public List<string> fireDeath = new List<string>();
    [TextArea] public List<string> cryoDeath = new List<string>();
    [TextArea] public List<string> electricDeath = new List<string>();

    List<string> GetDeathList(DeathType deathType)
    {
        switch (deathType)
        {
            case DeathType.Melee:
                return meleeDeath;
            case DeathType.Bullet:
                return bulletDeath;
            case DeathType.Explosion:
                return explosionDeath;
            case DeathType.Poison:
                return poisonDeath;
            case DeathType.Fire:
                return fireDeath;
            case DeathType.Cryo:
                return cryoDeath;
            case DeathType.Electric:
                return electricDeath;
            case DeathType.None:
                Debug.LogWarning("Check the Death Type");
                return null;
            default:
                return null;
        }
    }

    public string GetDeathMessage(DeathType deathType)
    {
        List<string> deathList = GetDeathList(deathType);
        if (deathList != null && deathList.Count > 0)
        {
            int randomIndex = Random.Range(0, deathList.Count);
            return deathList[randomIndex];
        }
        else
        {
            return "No death message available for the specified DeathType.";
        }
    }
}
    
