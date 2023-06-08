using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveAbility
{
    void ApplyActiveAbility(UnitStateController unitState);
}
public abstract class ActiveAbilityBase : ScriptableObject, IActiveAbility
{
    public abstract void ApplyActiveAbility(UnitStateController unitState);

}
