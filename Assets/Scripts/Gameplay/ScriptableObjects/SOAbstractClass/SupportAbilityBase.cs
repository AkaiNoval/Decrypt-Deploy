using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISupport
{
    void ApplySupport(UnitStateController unitState);
}
public abstract class SupportAbilityBase : ScriptableObject, ISupport
{
    public abstract void ApplySupport(UnitStateController unitState);
}
