using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPassiveAbility
{
    void ApplyPassiveAbility(UnitStateController unit);
    bool ConditionCheck(UnitStateController unitState);
}
public abstract class PassiveAbilityBase : ScriptableObject, IPassiveAbility
{
    public abstract float TriggerInterval { get; set; }
    public abstract void ApplyPassiveAbility(UnitStateController unit);
    public void StartTimer(UnitStateController unitState)
    {
        // Set the initial timer value
        // Start a coroutine to count down the timer
        if (unitState.IsCoroutineRunning) return;
        unitState.IsCoroutineRunning = true;
        unitState.StartCoroutine(CountdownTimer(unitState));

    }
    IEnumerator CountdownTimer(UnitStateController unitState)
    {
        // Infinite loop to keep the coroutine running
        while (true)
        {
            float timer = TriggerInterval; // Set the initial timer value
            // Countdown the timer until it reaches zero
            while (timer > 0f)
            {
                timer -= Time.deltaTime; // Decrease the timer by deltaTime each frame
                yield return null; // Wait for the next frame
            }

            // Wait until the condition becomes true
            while (!ConditionCheck(unitState))
            {
                yield return null; // Wait for the next frame
            }
            GoToPassiveState(unitState); // Condition is true, execute the active method
        }
    }

    public abstract bool ConditionCheck(UnitStateController unitState);
    void GoToPassiveState(UnitStateController unitState)
    {
        if (unitState.currentState == CurrentState.UsingPassiveAbility) return;
        Debug.Log("Chuyển State ở đây");
        unitState.SwitchState(unitState.StatePassiveAbility);
    }
}
