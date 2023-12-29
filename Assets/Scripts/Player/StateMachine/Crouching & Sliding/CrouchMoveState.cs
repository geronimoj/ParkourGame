using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrouchMove", menuName = "States/CrouchMove State", order = 1)]
public class CrouchMoveState : GroundMoveState
{
    /// <summary>
    /// Original speed cap
    /// </summary>
    private float ogMaxSpeed = 0f;

    protected override void StateStart(ref PlayerController ctrl)
    {
        ogMaxSpeed = ctrl.direction.MaxHozSpeed;
        ctrl.direction.MaxHozSpeed = ctrl.CrouchSpeed;
        ctrl.camFol.positionOffset.y = -0.4f;

        base.StateStart(ref ctrl);
    }

    protected override void StateEnd(ref PlayerController ctrl)
    {
        base.StateEnd(ref ctrl);

        ctrl.camFol.positionOffset.y = 0f;
        ctrl.direction.MaxHozSpeed = ogMaxSpeed;
        ogMaxSpeed = 0f;
    }
}
