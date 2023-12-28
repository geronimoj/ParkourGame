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
        if (InputManager.GetInput("Crouch") != 0)
        {
            return true;
        }

        return false;
    }
}
