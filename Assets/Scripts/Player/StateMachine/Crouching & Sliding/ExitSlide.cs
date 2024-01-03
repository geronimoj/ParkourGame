using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// True when the slide button is released but should not enter a crouch state
/// </summary>
[CreateAssetMenu(fileName = "ExitSlide", menuName = "Transitions/Exit Slide", order = 1)]
public class ExitSlide : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {   // Again, very similar to DoCrocuh & DoSlide, but we check for the button being released, while our speed is still high
        if (InputManager.GetInput("Crouch") == 0 && ctrl.TrueVelocity > ctrl.CrouchSpeed 
            && ctrl.ValidateColliderChange(PlayerColliderState.Standing))
        {
            return true;
        }

        return false;
    }
}
