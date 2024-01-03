using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Handles slide & crouch logic
/// </summary>
[CreateAssetMenu(fileName = "DoCrouch", menuName = "Transitions/Do Crouch", order = 1)]
public class DoCrouch : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        if (InputManager.GetInput("Crouch") != 0 && ctrl.TrueVelocity <= ctrl.CrouchSpeed
            && ctrl.ValidateColliderChange(PlayerColliderState.Crouched))
        {
            return true;
        }

        return false;
    }
}
