using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.States;

[CreateAssetMenu(fileName = "SlideMove", menuName = "States/SlideMove State", order = 1)]
public class CrouchAndSlideMoveState : GroundMoveState
{
    [SerializeField]
    private float slideOffLedgeVertVelocity = 1f;
    /// <summary>
    /// Rate at which the slide loses speed
    /// </summary>
    [SerializeField]
    private float slideDecellerationRate = 1f;
    /// <summary>
    /// Original speed cap
    /// </summary>
    private float ogMaxSpeed = 0f;

    private bool amSliding = false;
    /// <summary>
    /// Is the player currently sliding
    /// </summary>
    private bool AmSliding
    {
        get => amSliding;
        set
        {
            if (!value)
                player.direction.MaxHozSpeed = player.CrouchSpeed;

            amSliding = value;
        }
    }
    /// <summary>
    /// Reference to player for AmSliding callback
    /// </summary>
    PlayerController player;

    protected override void StateStart(ref PlayerController ctrl)
    {
        ctrl.SetColliderState(PlayerColliderState.Crouched, false, true);
        ctrl.camFol.positionOffset.y = -0.5f;

        player = ctrl;
        // Define sliding state
        ogMaxSpeed = ctrl.direction.MaxHozSpeed;
        AmSliding = ctrl.TrueVelocity > ctrl.CrouchSpeed;

        base.StateStart(ref ctrl);
    }

    protected override void StateUpdate(ref PlayerController ctrl)
    {   // If sliding, run slide logic
        if (AmSliding)
        {
            float speed = ctrl.HozSpeed;
            speed -= slideDecellerationRate * Time.deltaTime;
            // Clamp speed to crouch speed during slide. Will auto transition to crouch state when this condition is met.
            if (speed < ctrl.CrouchSpeed)
            {
                speed = ctrl.CrouchSpeed; // Deduct a small amount to avoid floating point error
                AmSliding = false;
            }
            // Set slide speed
            ctrl.HozSpeed = speed;
            ctrl.CheckDir = ctrl.direction.HozDirection;
            // Actually move the player
            ctrl.Move(ctrl.TotalVector * Time.deltaTime);
        }
        else
        {   // Otherwise run normal movement logic
            base.StateUpdate(ref ctrl);
        }
    }

    protected override void StateEnd(ref PlayerController ctrl)
    {
        base.StateEnd(ref ctrl);
        // Reset modified variables
        ctrl.direction.MaxHozSpeed = ogMaxSpeed;
        ctrl.camFol.positionOffset.y = 0f;

        if (!ctrl.OnGround)
            ctrl.VertSpeed -= slideOffLedgeVertVelocity;

        player = null;
    }
}
