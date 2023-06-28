using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
   [SerializeField] List<SOUnitStats> killsList = new List<SOUnitStats>();
   [SerializeField] float damageDealt;

    public List<SOUnitStats> KillsList { get => killsList; set => killsList = value; }
    public float DamageDealt { get => damageDealt; set => damageDealt = value; }
}
