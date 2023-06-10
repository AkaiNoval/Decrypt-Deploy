using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private UnitStats unitStats;
    private RuntimeAnimatorController previousRAC;
    private void Start()
    {
        // Set the initial animator controller during start
        SetAnimatorController();
    }
    private void Update()
    {
        // Check if the animator controller needs to be switched
        if (unitStats.Weapon == null) return;
        SwitchAnimatorController();
    }
    private void SetAnimatorController()
    {
        // Store the initial runtime animator controller
        previousRAC = unitStats.Weapon.runtimeAnimController;
        // Assign the initial runtime animator controller to the animator
        animator.runtimeAnimatorController = previousRAC;
    }
    private void SwitchAnimatorController()
    {
        // Check if the runtime animator controller has changed
        if (previousRAC != unitStats.Weapon.runtimeAnimController)
        {
            // Update the runtime animator controller
            previousRAC = unitStats.Weapon.runtimeAnimController;
            // Assign the new runtime animator controller to the animator
            animator.runtimeAnimatorController = previousRAC;
        }
    }
}

