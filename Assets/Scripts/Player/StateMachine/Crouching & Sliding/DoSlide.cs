using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Defines if the player should enter a slide
/// </summary>
public class DoSlide : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {   // Very similar to DoCrouch with the check inverted
        if (InputManager.GetInput("Crouch") != 0 && ctrl.TotalSpeed > ctrl.CrouchSpeed 
            && ctrl.SetColliderState(PlayerColliderState.Crouched, false))
        {
            return true;
        }

        return false;
    }
}
