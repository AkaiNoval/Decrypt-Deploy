using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RangedAttack : IState
{

    //Only Class Attacker is allowed to be here.
    //Play Reload animation when out of bullets but also when there is no target in range
    //Ref to the magazine

    //[SerializeField] bool spreadMode;
    //[SerializeField] int spreadAmount;
    //[SerializeField] float spreadAngle;
    public void EnterState(UnitStateController unitState)
    {
        unitState.currentState = CurrentState.RangedAttack;

    }
    public void UpdateState(UnitStateController unitState)
    {
        SwitchState(unitState);
    }
    void SwitchState(UnitStateController unitState)
    {
        // If the objective is still in the far range and outside the close range, keep the current state
        if (unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange && unitState.Targeting.DistanceToObj <= unitState.UnitStats.UnitFarRange)
        {
            return;
        }
        // If the target is in the far range and outside the close range, keep the current state
        if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
        {
            return;
        }
        // If the target is null but the distance to the objective is still in the far range, keep the current state
        if (unitState.Targeting.Target == null && unitState.Targeting.DistanceToObj <= unitState.UnitStats.UnitFarRange)
        {
            return;
        }
        // If the target is null and the distance to the objective is outside the far range, switch to the melee attack state
        if (unitState.Targeting.Target == null && unitState.Targeting.DistanceToObj > unitState.UnitStats.UnitFarRange)
        {
            unitState.SwitchState(unitState.StateMoving);
        }
        // If the distance to the target is within both the far range and close range, switch to the melee attack state
        if (unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange)
        {
            unitState.SwitchState(unitState.StateMeleeAttack);
        }
        if (unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitCloseRange)
        {
            unitState.SwitchState(unitState.StateMoving);
        }
        if(unitState.Targeting.Target == null && unitState.Targeting.DistanceToObj <= unitState.UnitStats.UnitFarRange && unitState.Targeting.DistanceToTarget <= unitState.UnitStats.UnitCloseRange)
        {
            unitState.SwitchState(unitState.StateMeleeAttack);
        }
    }
    public void RangeAttack(UnitStateController unitState)
    {
        // Check if there is no target
        if (unitState.Targeting.Target == null)
        {
            // Check if there is an objective
            if (unitState.Targeting.Objective == null)
            {
                return; // No objective set, return
            }

            // Shoot at the objective
            Vector3 objectivePosition = unitState.Targeting.Objective.transform.position;
            Vector2 directionToObjective = objectivePosition - unitState.bulletSpawnPoint.transform.position;

            // Calculate the angle to point towards the objective
            float angleObj = Vector2.SignedAngle(Vector2.right, directionToObjective);

            // Create a rotation based on the calculated angle
            Quaternion bulletRotationObj = Quaternion.AngleAxis(angleObj, Vector3.forward);

            // Spawn a bullet towards the objective
            InstBullet(unitState, unitState.bulletPrefab, unitState.bulletSpawnPoint, bulletRotationObj, unitState.UnitStats.UnitRangeDamage);

            return;
        }

        // Check if the target is too far
        if (unitState.Targeting.DistanceToTarget > unitState.UnitStats.UnitFarRange)
        {
            return; // Target is too far, return
        }

        Collider2D targetCollider = unitState.Targeting.Target.GetComponent<Collider2D>();
        if (targetCollider == null)
        {
            return; // Invalid target collider, return
        }

        // Get the center of the target collider
        Vector3 targetCenter = targetCollider.bounds.center;

        // Calculate the direction to the target
        Vector2 directionToTarget = targetCenter - unitState.bulletSpawnPoint.transform.position;

        // Calculate the angle to point towards the target
        float angleTarget = Vector3.Angle(Vector3.right, directionToTarget);

        // Flip the angle if the target is below the unit
        if (unitState.Targeting.Target.transform.position.y < unitState.transform.position.y)
        {
            angleTarget *= -1;
        }

        // Create a rotation based on the calculated angle
        Quaternion bulletRotationTarget = Quaternion.AngleAxis(angleTarget, Vector3.forward);

        // Spawn a bullet towards the target
        InstBullet(unitState, unitState.bulletPrefab, unitState.bulletSpawnPoint, bulletRotationTarget, unitState.UnitStats.UnitRangeDamage);
    }

    void InstBullet(UnitStateController unitState, GameObject bulletPrefab, GameObject bulletSpawnPoint, Quaternion rotation, float rangedDamage)
    {
        if (unitState.UnitStats.Weapon.EnableMultipleBullets)
        {
            // Shotgun behavior - Spawn multiple bullets with spread
            int bulletPerShot = unitState.UnitStats.Weapon.BulletPerShot; 
            float spreadAngle = unitState.UnitStats.Weapon.SpreadAngle; 

            float spreadOffset = spreadAngle / (bulletPerShot - 1); // Calculate the offset between each bullet

            for (int i = 0; i < bulletPerShot; i++)
            {
                // Calculate the angle for the current bullet
                float bulletAngle = -spreadAngle / 2 + (i * spreadOffset);

                // Apply the bullet angle to the rotation
                Quaternion bulletRotation = rotation * Quaternion.Euler(0f, 0f, bulletAngle);

                // Spawn a bullet with the rotated direction
                SpawnBullet(unitState, bulletPrefab, bulletSpawnPoint, bulletRotation, rangedDamage);
            }
        }
        else
        {
            // Single bullet behavior - Spawn a single bullet without spread
            SpawnBullet(unitState, bulletPrefab, bulletSpawnPoint, rotation, rangedDamage);
        }
    }

    void SpawnBullet(UnitStateController unitState, GameObject bulletPrefab, GameObject bulletSpawnPoint, Quaternion rotation, float rangedDamage)
    {
        GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, rotation);
        bullet.GetComponent<Bullet>().IsEnemyBullet = unitState.GetComponent<Unit>().IsEnemy;
        bullet.GetComponent<Bullet>().BulletDamage = rangedDamage;
    }



    #region Nothing here
    public void ExitState(UnitStateController unitState)
    {

    }

    public void OnTriggerEnter2DState(UnitStateController unitState)
    {

    }

    public void PhysicsUpdateState(UnitStateController unitState)
    {

    } 
    #endregion
}
