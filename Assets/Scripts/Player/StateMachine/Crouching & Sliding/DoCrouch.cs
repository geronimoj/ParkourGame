using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Handles slide & crouch logic
/// </summary>
public class DoCrouch : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        if (InputManager.GetInput("Crouch") != 0 && ctrl.TotalSpeed <= ctrl.CrouchSpeed
            && ctrl.SetColliderState(PlayerColliderState.Crouched, false))
        {
            return true;
        }

        return false;
    }
}
