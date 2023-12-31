using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Handles slide to crouch
/// </summary>
[CreateAssetMenu(fileName = "SlideToCrouch", menuName = "Transitions/Slide To Crouch", order = 1)]
public class SlideToCrouch : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {   // If we are at crouch speed, but cannot stand up, enter the crouch state
        if (ctrl.TrueVelocity <= ctrl.CrouchSpeed
            && !ctrl.SetColliderState(PlayerColliderState.Standing, false))
        {
            return true;
        }

        return false;
    }
}
