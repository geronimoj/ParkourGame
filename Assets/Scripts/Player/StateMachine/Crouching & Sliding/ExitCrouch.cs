using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

public class ExitCrouch : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        if (InputManager.GetInput("Crouch") == 0 && ctrl.SetColliderState(PlayerColliderState.Standing, false))
        {
            return true;
        }

        return false;
    }
}
