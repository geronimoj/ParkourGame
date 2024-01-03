using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

[CreateAssetMenu(fileName = "ExitCrouch", menuName = "Transitions/Exit Crouch", order = 1)]
public class ExitCrouch : Transition<PlayerController>
{
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        if (InputManager.GetInput("Crouch") == 0 && ctrl.ValidateColliderChange(PlayerColliderState.Standing))
        {
            return true;
        }

        return false;
    }
}
