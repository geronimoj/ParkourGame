using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Checks if the player wants to jump
/// </summary>
[CreateAssetMenu(fileName = "DidJump", menuName = "Transitions/DidJump", order = 3)]
public class DidJump : Transition<PlayerController>
{   
    /// <summary>
    /// Extra horizontal speed given to the player upon jumping
    /// </summary>
    public float extraJumpSpeed = 3;
    /// <summary>
    /// Multiplier applied to player horizontal speed when jumping
    /// </summary>
    public float hozSpeedMultiplier = 1f;
    /// <summary>
    /// Did the player press the space bar
    /// </summary>
    /// <param name="ctrl">A reference to the player controller</param>
    /// <returns>Returns true when the player presses the space bar</returns>
    public override bool ShouldTransition(ref PlayerController ctrl)
    {   //Did the player press jump & is this a new input
        if (InputManager.NewInput("Jump") != 0 || ctrl.ForceJump)
        {   //Reset ForceJump so we don't, ya know, keep jumping
            ctrl.ForceJump = false;
            //Set our currentdir to be our expectedDir, this is primarily for wall climb jumping & wall run jumping
            ctrl.Direction = ctrl.ExpectedDir;
            //Apply the extra jump speed and 
            //clamp the speed completely
            float speed = ctrl.HozSpeed;
            speed += extraJumpSpeed;
            speed *= hozSpeedMultiplier;
            ctrl.HozSpeed = speed;
            //Set the jump force
            ctrl.VertSpeed = ctrl.JumpForce;
            Debug.Log("Jump");
            return true;
        }
        return false;
    }
}
