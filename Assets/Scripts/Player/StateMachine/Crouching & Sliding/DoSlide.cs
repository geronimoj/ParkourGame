using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Defines if the player should enter a slide
/// </summary>
[CreateAssetMenu(fileName = "DoSlide", menuName = "Transitions/Do Slide", order = 1)]
public class DoSlide : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {   // Very similar to DoCrouch with the check inverted
        if (InputManager.GetInput("Crouch") != 0 && ctrl.TrueVelocity > ctrl.CrouchSpeed 
            && ctrl.ValidateColliderChange(PlayerColliderState.Crouched))
        {
            return true;
        }

        return false;
    }
}
