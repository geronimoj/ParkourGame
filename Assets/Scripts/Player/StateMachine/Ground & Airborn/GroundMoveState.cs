using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.States;
using CustomController;

/// <summary>
/// The state for moving along the ground
/// </summary>
[CreateAssetMenu(fileName = "GroundMove", menuName = "States/GroundMove State", order = 1)]
public class GroundMoveState : State<PlayerController>
{
    public float angularDecelleration = 0.1f;

    [SerializeField]
    private float inputCooldown = 0.1f;
    /// <summary>
    /// When the players speed is below this value, the player will accelerate using belowMinAcceleration
    /// </summary>
    private float minSpeed = 0f;
    /// <summary>
    /// How fast does the player accelerate when below minSpeed
    /// </summary>
    private float belowMinAcceleration = 0f;
    /// <summary>
    /// How quickly does the player accelerate when above minSpeed
    /// </summary>
    private float acceleration = 0f;
    /// <summary>
    /// How quickly does the player decellerate
    /// </summary>
    private float decelleration = 0f;

    //private float timer = 0;
    /// <summary>
    /// Did we get an input this frame
    /// </summary>
    private bool gotInput = false;
    /// <summary>
    /// Did we get a new input
    /// </summary>
    private bool newInput = false;
    /// <summary>
    /// Should the player slow down
    /// </summary>
    private bool slowDown = false;
    /// <summary>
    /// A timer used when smoothing the rotation between expectedDir and currentDir
    /// </summary>
    //private float slowDownTime = 0;
    /// <summary>
    /// The time since the last input
    /// </summary>
    private float inputTimer;

    /// <summary>
    /// Set us up for moving horizontally
    /// </summary>
    /// <param name="ctrl">A reference to the player controller</param>
    protected override void StateStart(ref PlayerController ctrl)
    {
        //Make sure our state is in the base state
        newInput = false;
        ctrl.direction.VertSpeed = 0;

        minSpeed = ctrl.MinSpeed;
        belowMinAcceleration = ctrl.BelowMinAccel;
        acceleration = ctrl.Acceleration;
        decelleration = ctrl.Decelleration;
    }
    /// <summary>
    /// Moves the player horizontally. Also up and down ramps
    /// </summary>
    /// <param name="ctrl">A reference to the player controller</param>
    protected override void StateUpdate(ref PlayerController ctrl)
    {
        //Get the inputs
        float y = InputManager.GetInput("Horizontal");
        float x = InputManager.GetInput("Vertical");
        //Check if we got an input
        if (x != 0 || y != 0)
        {   //Store whether we got an input for use later
            gotInput = true;
            //Reset the timer because we don't want it going off
            inputTimer = inputCooldown;
            //Get our movement direction
            Vector3 v = ctrl.GetAngle(x, y);
            //If we are moving in a new direction, reset the turn timer
           // if (v != ctrl.ExpectedDir)
                //timer = 0;
            ctrl.ExpectedDir = v;
        }
        else
        {   //Get our movement direction
            ctrl.ExpectedDir = Vector3.zero;
            //Store whether we got an input for use later
            gotInput = false;
            //Decrement the timer
            inputTimer -= Time.deltaTime;
            //If its less than 0, we consider that a new input is needed
            if (inputTimer <= 0)
                newInput = true;
        }
        /////////////ACCELERATION/////////////
        //If the direction the player wants to move in is in the opposite direction, we decellerate a bit
        slowDown = Vector3.Dot(ctrl.Direction, ctrl.ExpectedDir) < 0;
        //If we need to slowDown or we haven't gotten an input recently, decellerate, otherwise, accelerate
        if (slowDown || !gotInput && newInput)
        {   //Decellerate
            Accelerate(ctrl, true, true);
            //slowDownTime = (-ctrl.HozSpeed) / -ctrl.Decelleration;
        }
        else
            //Accelerate
            Accelerate(ctrl, false, true);

        //Is the player stationary & only just made an input
        if (ctrl.HozSpeed <= 0 && newInput && gotInput)
        {
            //We got a new input so instantly start moving in the expected direction
            ctrl.Direction = ctrl.ExpectedDir;
            newInput = false;
        }
        else
        {   //Calculate our rotation time
            //if (!slowDown)
            //    slowDownTime = ctrl.MinTurnTime;
            //Start rotating to the expected direction in a specific time frame
            ctrl.Direction = ctrl.ExpectedDir;
            //ctrl.SmoothDirection(ctrl.ExpectedDir, slowDownTime, ref timer);
        }
        //Calculate the movement vector
        Vector3 moveVec = ctrl.TotalVector * Time.deltaTime;
        //Check if we should move the player up or down the surface.
        if (CustomCollider.CastWithOffset(ctrl.colInfo, Vector3.down * (ctrl.StepHeight * 2), Vector3.up * ctrl.StepHeight + moveVec, out RaycastHit hit) && ctrl.colInfo.ValidSlope(hit.normal))
            moveVec.y = (hit.point + hit.normal * (ctrl.colInfo.Radius + ctrl.colInfo.CollisionOffset)).y - ctrl.colInfo.GetLowerPoint().y;

        //Move the character
        ctrl.Move(moveVec);
        ctrl.CheckDir = ctrl.Direction;
        // Clamp velocity from moving into terrain.
        if (ctrl.HozSpeed > ctrl.TrueVelocity)
            ctrl.HozSpeed = ctrl.TrueVelocity;
    }

    /// <summary>
    /// Accelerates the player based on the acceleration values
    /// </summary>
    /// <param name="decelerate">Should the player decellerate</param>
    /// <param name="doClamp">Should the speed be clamped between 0 and maxSpeed</param>
    public void Accelerate(PlayerController ctrl, bool decelerate, bool doClamp = true)
    {
        if (decelerate)
            //Decelerate
            ctrl.HozSpeed -= decelleration * Time.deltaTime;
        else
            //Accelerate
            if (ctrl.HozSpeed < minSpeed)
            ctrl.HozSpeed += belowMinAcceleration * Time.deltaTime;
        else
            ctrl.HozSpeed += acceleration * Time.deltaTime;

        Vector3 dif = ctrl.ExpectedDir - ctrl.direction.HozDirection;
        float mag = dif.magnitude * Time.deltaTime;

        ctrl.HozSpeed -= mag * angularDecelleration;
        //Make sure the caller wants the speed capped. There are some instances where this would not be wanted
        if (doClamp)
            ctrl.HozSpeed = Mathf.Clamp(ctrl.HozSpeed, 0, ctrl.direction.MaxHozSpeed);
    }
}
