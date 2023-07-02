using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject Explosion;
    public float Radius;
    public int DamageAmount;
    public KillCounter killCounter;
    [SerializeField] bool drawGizmos;
    [SerializeField] LayerMask layerMask;
    private void OnDestroy()
    {
        // Instantiate the explosion
        GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);

        // Check for Units within the radius and deal damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius, layerMask);
        foreach (Collider2D collider in colliders)
        {
            UnitStats unitstat = collider.GetComponent<UnitStats>();
            if (unitstat != null)
            {
                unitstat.TakeExplosionDamage(DamageAmount, unitstat.UnitExplosionResistance); //Explosion Damage wont never deal critical damage
                killCounter.DamageDealt += DamageAmount;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

