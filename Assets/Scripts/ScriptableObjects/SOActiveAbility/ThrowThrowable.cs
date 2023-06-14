using UnityEngine;


[CreateAssetMenu(menuName = "ActiveAbility/ThrowThrowable")]
public class ThrowThrowable : ActiveAbilityBase
{
    public GameObject Throwable;
    public float UpwardModifier;
    public float VelocityModifier;
    public float RotationSpeed; // New variable for rotation speed
    public float LifeTime;

    public override void ApplyActiveAbility(UnitStateController unitState)
    {
        ThrowFragGrenade(unitState);
    }

    void ThrowFragGrenade(UnitStateController unitState)
    {
        GameObject fragGrenade = Instantiate(Throwable);
        fragGrenade.transform.position = unitState.transform.position;
        Rigidbody2D rb = fragGrenade.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * UpwardModifier, ForceMode2D.Impulse);
        rb.AddForce(unitState.transform.right * VelocityModifier, ForceMode2D.Impulse);
        // Apply rotation
        rb.angularVelocity = RotationSpeed; // Set the angular velocity for rotation
        Destroy(fragGrenade, LifeTime);
    }
}
