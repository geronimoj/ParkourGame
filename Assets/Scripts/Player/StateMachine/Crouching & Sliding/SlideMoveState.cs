using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.States;

[CreateAssetMenu(fileName = "SlideMove", menuName = "States/SlideMove State", order = 1)]
public class SlideMoveState : State<PlayerController>
{
    /// <summary>
    /// Rate at which the slide loses speed
    /// </summary>
    [SerializeField]
    private float slideDecellerationRate = 1f;

    protected override void StateStart(ref PlayerController ctrl)
    {
        ctrl.camFol.positionOffset.y = -0.5f;
    }

    protected override void StateUpdate(ref PlayerController ctrl)
    {
        float speed = ctrl.HozSpeed;
        speed -= slideDecellerationRate * Time.deltaTime;
        // Clamp speed to crouch speed during slide. Will auto transition to crouch state when this condition is met.
        if (speed < ctrl.CrouchSpeed)
            speed = ctrl.CrouchSpeed;
        // Set slide speed
        ctrl.HozSpeed = speed;
        // Actually move the player
        ctrl.Move(ctrl.TotalVector * Time.deltaTime);
    }

    protected override void StateEnd(ref PlayerController ctrl)
    {
        ctrl.camFol.positionOffset.y = 0f;
    }
}
