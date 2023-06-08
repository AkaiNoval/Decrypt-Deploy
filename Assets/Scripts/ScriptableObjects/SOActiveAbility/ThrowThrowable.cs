using UnityEngine;


[CreateAssetMenu(menuName = "ActiveAbility/ThrowThrowable")]
public class ThrowThrowable : ActiveAbilityBase
{
    public GameObject Throwable;
    public float UpwardModifier;
    public float VelocityModifier;
    public override void ApplyActiveAbility(UnitStateController unitState)
    {
        ThrowFragGrenade(unitState);
    }
    void ThrowFragGrenade(UnitStateController unitState)
    {
        // Instantiate the Frag Grenade game object and throw it
        GameObject fragGrenade = Instantiate(Throwable);
        fragGrenade.transform.position = unitState.transform.position;
        Rigidbody2D rb = fragGrenade.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * UpwardModifier, ForceMode2D.Impulse);
        rb.AddForce(unitState.transform.right * VelocityModifier, ForceMode2D.Impulse);
    }

}
