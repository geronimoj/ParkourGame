﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains all the information for the player. Contains code that should be used to Move and rotate the player
/// </summary>
[RequireComponent(typeof(StateManager))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    //Player Size Information
    public ColliderInfo colInfo;
    //Acceleration Information
    #region Speed
    /// <summary>
    /// How quickly does the player accelerate when above minSpeed
    /// </summary>
    [SerializeField]
    private float acceleration = 5;
    /// <summary>
    /// A Set for acceleration
    /// </summary>
    public float Acceleration
    {
        set
        {
            acceleration = value;
        }
    }
    /// <summary>
    /// How fast does the player accelerate when below minSpeed
    /// </summary>
    [SerializeField]
    private float belowMinAcceleration = 20;
    /// <summary>
    /// A Set for belowMinAcceleration
    /// </summary>
    public float BelowMinAccel
    {
        set
        {
            belowMinAcceleration = value;
        }
    }
    /// <summary>
    /// How quickly does the player decellerate
    /// </summary>
    [SerializeField]
    private float decelleration = 10;
    /// <summary>
    /// A Set for decelleration
    /// </summary>
    public float Decelleration
    {
        get
        {
            return decelleration;
        }
        set
        {
            decelleration = value;
        }
    }
    //Speed Information
    /// <summary>
    /// When the players speed is below this value, the player will accelerate using belowMinAcceleration
    /// </summary>
    [SerializeField]
    private float minSpeed = 3;
    /// <summary>
    /// A Set for minSpeed
    /// </summary>
    public float MinSpeed
    {
        set
        {
            minSpeed = value;
        }
    }
    /// <summary>
    /// How much upwards force is applied to the player when they jump.
    /// </summary>
    [SerializeField]
    private float jumpForce = 0;
    /// <summary>
    /// A Get/Set for jumpForce
    /// </summary>
    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            jumpForce = value;
        }
    }
    /// <summary>
    /// How strong gravity is on the player
    /// </summary>
    [SerializeField]
    private float gravity = 1;
    /// <summary>
    /// A Get/Set for gravity
    /// </summary>
    public float Gravity
    {
        get
        {
            return gravity;
        }
        set
        {
            gravity = value;
        }
    }
    #endregion
    //Rotation Information
    #region Rotation
    /// <summary>
    /// The sensitivity used for rotating the player with the mouse
    /// </summary>
    [SerializeField]
    private float sensitivity = 200;
    /// <summary>
    /// A Set for sensitivity
    /// </summary>
    public float Sensitivity
    {
        set
        {
            sensitivity = value;
        }
    }
    /// <summary>
    /// How quickly does currentDir transition to expectedDir
    /// </summary>
    [SerializeField]
    private float minTurnTime = 0;
    /// <summary>
    /// A Get for minTurnTime
    /// </summary>
    public float MinTurnTime
    {
        get
        {
            return minTurnTime;
        }
    }
    #endregion
    /// <summary>
    /// The time between button presses in which the player will not slow down
    /// </summary>
    [SerializeField]
    private float inputTime = 0;
    /// <summary>
    /// A Get for inputTime
    /// </summary>
    public float InputTime
    {
        get
        {
            return inputTime;
        }
    }

    #region Movement
    public MovementDirection direction;
    /// <summary>
    /// Get or set the horizontal speed
    /// </summary>
    public float HozSpeed
    {
        get
        {
            return direction.HozSpeed;
        }
        set
        {
            direction.HozSpeed = value;
        }
    }
    /// <summary>
    /// Get or Set the vertical speed
    /// </summary>
    public float VertSpeed
    {
        get
        {
            return direction.VertSpeed;
        }
        set
        {
            direction.VertSpeed = value;
        }
    }
    /// <summary>
    /// Get or Set the total speed of the player
    /// </summary>
    public float TotalSpeed
    {
        get
        {
            return direction.TotalSpeed;
        }
        set
        {
            direction.TotalSpeed = value;
        }
    }
    /// <summary>
    /// Get or Set the direction of movement
    /// </summary>
    public Vector3 Direction
    {
        get
        {
            return direction.Direction;
        }
        set
        {
            direction.Direction = value;
        }
    }
    /// <summary>
    /// Get the direction and speed combined
    /// </summary>
    public Vector3 TotalVector
    {
        get
        {
            return direction.TotalVector;
        }
    }
    /// <summary>
    /// A call function for direction.
    /// See MovementDirection.SmoothDirection
    /// </summary>
    /// <param name="final"></param>
    /// <param name="time"></param>
    /// <param name="timer"></param>
    public void SmoothDirection(Vector3 final, float time, ref float timer)
    {
        direction.SmoothDirection(final, time, ref timer);
    }
    /// <summary>
    /// A call function for direction.
    /// See MovementDirection.SmoothDirection
    /// </summary>
    /// <param name="final"></param>
    /// <param name="velocity"></param>
    public void SmoothDirection(Vector3 final, float velocity)
    {
        direction.SmoothDirection(final, velocity);
    }
    #endregion
    /// <summary>
    /// The expected direction the player is expected to move.
    /// </summary>
    private Vector3 expectedDir;
    /// <summary>
    /// A Get/Set for expectedDir
    /// </summary>
    public Vector3 ExpectedDir
    {
        get
        {
            return expectedDir;
        }
        set
        {
            expectedDir = value.normalized;
        }
    }
#if UNITY_EDITOR
    /// <summary>
    /// Used for debugging information
    /// </summary>
    private Vector3 test;
#endif
    /// <summary>
    /// The previous point the player was standing on the ground. Used for checking if the player should fall off a ledge
    /// </summary>
    private Vector3 previousGround = Vector3.zero;
    /// <summary>
    /// A Get/Set for previousGround
    /// </summary>
    public Vector3 PreviousGround
    {
        get
        {
            return previousGround;
        }
        set
        {
            previousGround = value;
        }
    }
    /// <summary>
    /// A reference direction used to had vectors between states and transtitions.
    /// Expected to be normalised
    /// </summary>
    private MovementDirection checkDir;
    /// <summary>
    /// Returns a normalised Vector3 for a stored direction
    /// Sets the direction of the Vector
    /// </summary>
    public Vector3 CheckDir
    {
        get
        {
            return checkDir.Direction;
        }
        set
        {
            checkDir.Direction = value.normalized;
        }
    }
    /// <summary>
    /// Returns the length of CheckDir
    /// Sets the length of CheckDir
    /// </summary>
    public float CheckDirRange
    {
        get
        {
            return checkDir.TotalSpeed;
        }
        set
        {
            checkDir.TotalSpeed = value;
        }
    }
    /// <summary>
    /// The point to teleport the player too if their y position is too low
    /// </summary>
    public Vector3 respawnPosition;
    //Class References
    /// <summary>
    /// A reference to the StateManager
    /// </summary>
    private StateManager stateManager = null;
    /// <summary>
    /// A reference to the cameraFollower script on the camera
    /// </summary>
    public CameraFollower camFol = null;
    //Camera Rotation
    /// <summary>
    /// Set to true when you want to forcefully rotate the players camera to a target Rotation
    /// </summary>
    private bool rotateToAngle = false;
    /// <summary>
    /// The speed at which the players camera rotates. This is automatically calculated
    /// </summary>
    private float forceRotateSpeed = 0f;
    /// <summary>
    /// How much time will pass while rotating the camera
    /// </summary>
    [SerializeField]
    private float forceRotateTime = 0.2f;
    /// <summary>
    /// A reference to the target rotation
    /// </summary>
    private float targetRotation = 0;
    /// <summary>
    /// A reference to the text being used to display the players speed
    /// </summary>
    public Text speedText;

    #region ParkourInfo
    #region Vault&StepUpInfo
    /// <summary>
    /// The extra Range to check if the player should step down a ledge. Used for small steps and ramps
    /// </summary>
    [SerializeField]
    [Tooltip("The height at which obstacles will be automatically walked over. Also the value when obstacles are in consideration to be low obstacles. This is calculated from the bottom of the player")]
    private float stepHeight = 0.1f;
    /// <summary>
    /// A Get for the step height
    /// </summary>
    public float StepHeight
    {
        get
        {
            return stepHeight;
        }
    }
    /// <summary>
    /// The height that dictates a low ledge
    /// </summary>
    [SerializeField]
    [Tooltip("The height that dictates a low obstacle. Anything above this value is not considered a low obstacle. This is calculated from the bottom of the player")]
    private float lowLedgeHeight = 1;
    /// <summary>
    /// A Get for the lower ledge height
    /// </summary>
    public float LowLedgeHeight
    {
        get
        {
            return lowLedgeHeight;
        }
    }
    /// <summary>
    /// The distance of the vault
    /// </summary>
    [SerializeField]
    [Tooltip("The distance of the vault")]
    private float vaultDistance;
    /// <summary>
    /// A Get for vaultDistance. The distance of a vault
    /// </summary>
    public float VaultDistance
    {
        get
        {
            return vaultDistance;
        }
        set
        {
            vaultDistance = value;
        }
    }
    /// <summary>
    /// The height of the player during the vault
    /// </summary>
    [Tooltip("The height of the player during the vault")]
    [SerializeField]
    private float playerVaultHeight;
    /// <summary>
    /// A get for playerVaultHeight. The height of the player during the vault.
    /// </summary>
    public float PlayerVaultHeight
    {
        get
        {
            return playerVaultHeight;
        }
        set
        {
            playerVaultHeight = value;
        }
    }

    private bool forceJump = false;
    /// <summary>
    /// Set to true if you want to force the player to jump the next the DidJump transition is checked.
    /// </summary>
    public bool ForceJump
    {
        get
        {
            return forceJump;
        }
        set
        {
            forceJump = value;
        }
    }
    #endregion

    #region LedgeInfo
    /// <summary>
    /// The speed at which the player climbs up the ledge
    /// </summary>
    public float pullUpSpeed = 3;
    /// <summary>
    /// The speed at which the player can move along the ledge
    /// </summary>
    public float shimmySpeed = 1;
    /// <summary>
    /// How far above the ledge the player can be to grab it
    /// </summary>
    public float lowerGrabDist = 0.5f;
    #endregion
    /// <summary>
    /// How much of the -checkDir should be applied to expectedDir (the direction the player will jump off), if they are looking into the wall
    /// </summary>
    public float jumpOffPercent = 0.1f;
    /// <summary>
    /// The z rotation of the camera during a wall run
    /// </summary>
    public float cameraAngle = 5;
    /// <summary>
    /// If the players vertical speed is less than this value, cancel the wall climb
    /// </summary>
    public float vertSpeedCancel = -3;
    /// <summary>
    /// How long the player will float in the air before breaking out of the wall climb
    /// </summary>
    public float floatTime = 0.5f;
    /// <summary>
    /// The maximum distance the player can wall climb
    /// </summary>
    public float maxDist = 2;
    /// <summary>
    /// The minimum distance the player can wall climb
    /// </summary>
    public float minDist = 1;
    /// <summary>
    /// The speed at which the player climbs the wall
    /// </summary>
    public float climbSpeed = 5;
    /// <summary>
    /// The angle at which the player wall climbs over wall runs
    /// </summary>
    [Tooltip("The angle at which the player wall climbs over wall runs")]
    [Range(0,90)]
    public float wallClimbAngle = 30;
    /// <summary>
    /// The distance into the ledge that must still be solid ground for the player to clamber up
    /// </summary>
    [Tooltip("The space required beyond a ledge to be able to climb up it")]
    public float openSpaceRequired = 0.5f;
    /// <summary>
    /// The distance at which we are considered at a ledge
    /// </summary>
    [Tooltip("The distance at which we are considered at a ledge. It's basically just used as a bonus distance to some raycasts")]
    public float atLedgeDistance = 0.1f;
    #endregion
    #endregion

    /// <summary>
    /// Gets a reference to the state manager and locks to cursor if the user is in windows mode
    /// </summary>
    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
#if UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity *= 10;
#endif

#if UNITY_EDITOR
        sensitivity *= 10;
#endif
        if (stateManager == null)
            Debug.LogError("No state manager found");
        if (speedText == null)
            Debug.LogWarning("No speedometer set");
    }

    private void Start()
    {
        colInfo.SetTransform(transform);
    }
    /// <summary>
    /// Rotates the player and calls the update function for the current state
    /// </summary>
    void Update()
    {
        //Check the player hasn't fallen to far
        if (transform.position.y < -100f)
            transform.position = respawnPosition;

        if (speedText != null)
            speedText.text = "Speed: " + direction.TotalSpeed.ToString("F2");

        if (!rotateToAngle)
            //Rotate the player camera using the mouse
            Rotate(sensitivity, 1, 0.5f);
        else
            RotateTo(targetRotation);

        if (stateManager != null)
            //Calls the current state
            stateManager.DoState(this);
    }
    /// <summary>
    /// Returns a new direction assuming transform.forward represent x = 0, z = 1
    /// </summary>
    /// <param name="x">The horizontal value</param>
    /// <param name="z">The vertical value</param>
    /// <returns>Returns the Direction from transform.forward based on x & z</returns>
    public Vector3 GetAngle(float x, float z)
    {
        //Z is forward, X is to the left
        float curAngle = transform.eulerAngles.y;
        Vector2 v = new Vector2(z, x);
        v.Normalize();
        float angleDif = Mathf.Atan2(v.x,  v.y) * Mathf.Rad2Deg;

        curAngle += angleDif;

        return new Vector3(Mathf.Sin(curAngle * Mathf.Deg2Rad), 0, Mathf.Cos(curAngle * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Moves the player. Contains all collision detection required
    /// </summary>
    /// <param name="dir">The direction with a magnitude of distance the player should be moved</param>
    /// <param name="cancelOnFail">If true, the player will not be moved if dir is changed</param>
    public void MoveTo(Vector3 dir, bool cancelOnFail = false)
    {   
#if UNITY_EDITOR
        test = dir;
#endif
        RaycastHit[] hits;
        //Make sure we have a direction to move in
        if (dir != Vector3.zero)
        {
            //Get the collisions
            hits = MoveToRaycasts(dir, out int offsetIndex);

            bool updateHitInfo = false;
            int attempts = 0;
            //Loop through the collisions and adjust the current movement direction so that it is not pointing into any normals
            for (int i = 0; (i < hits.Length || updateHitInfo); i++)
            {   //Ensure we have hits as updateHitInfo can enter this loop even with a length of 0
                if (hits.Length == 0)
                    break;
                //Reset the index and don't update hit info. this is done so that we loop through all hits again
                if (updateHitInfo)
                {
                    updateHitInfo = false;
                    i = 0;
                }

                attempts++;
                if (attempts > 99)
                {
                    Debug.LogError("Could not solve for collision response. Not moving character");
                    return;
                }
                //Make sure aren't colliding with ourself
                if (hits[i].distance != 0 && hits[i].point != Vector3.zero)
                {
                    float dot = Vector3.Dot(hits[i].normal, dir);
                    //Make sure we are heading into the normal
                    if (dot < 0)
                    {   //If we can't move there, exit if this bool is true
                        if (cancelOnFail)
                            return;
                        //Get a vector from the point, to our raycast origin
                        Vector3 curNew = colInfo.GetCenteralPoint(hits[i].point, hits[i].normal) - hits[i].point;
                        //Get the dot product against the normal (its literally dot that we calculated earlier but positive)
                        dot = Vector3.Project(dir, hits[i].normal).magnitude;
                        //Subtract the dot product from are calculated vector to only get the overshooting amount.
                        dot -= Vector3.Project(curNew, hits[i].normal).magnitude;
                        float dist = colInfo.Radius + colInfo.CollisionOffset;
                        if (dot > -dist)
                        {
                            //We then apply this as a vector along the normal of the hit surface with a bit of extra stuff to adjust the movement vector away from the wall
                            dir += hits[i].normal * (dist + dot);
                            //Tell ourself to update the movement information
                            updateHitInfo = true;
                        }
                    }
                    //This is just to make sure that the last hit check actually gets updated
                    if (updateHitInfo)
                    {
                        hits = MoveToRaycasts(dir, out offsetIndex);
                    }
                }
                else
                {
                    if (i < offsetIndex)
                        //If this debug message is being called, we have a big problem
                        Debug.LogWarning("In collider: " + hits[i].transform.gameObject.name);
                    else
                        //This Debug message is ok to see
                        Debug.LogWarning("Too close to surface: " + hits[i].transform.gameObject.name);
                }
            }
        }
#if UNITY_EDITOR
        //Draw the movement and then move us along it
        Debug.DrawLine(colInfo.GetLowestPoint(), colInfo.GetLowestPoint() + dir, Color.magenta, 20f);
#endif
        transform.Translate(dir, Space.World);
        //Do a raycast down to check if we are on the ground
        hits = ColliderInfo.CastAllWithOffset(colInfo, colInfo.GravityDirection * 1e-3f);
        //Loop through the downwards raycast results and check if any of them meet the on ground conditions
        for (int i = 0; i < hits.Length; i++)
            if (colInfo.ValidSlope(hits[i].normal))
            {   //If we just entered onGround, then change our movement vector to be that direction
                //This exists to help with exiting from slopes we can't stand on. However, in turn, its causes other issues with going up small un-ramped bumps, 
                //causing the player to drift on them. So we also check that the point we hit is below us, so that small ledges at our height aren't affected
                if (!colInfo.OnGround && (hits[i].point - colInfo.GetLowestPoint()).y < 0)
                    direction.HozDirection = dir.normalized;
                //We found one of them so set us to be on the ground, and set previous ground
                colInfo.OnGround = true;
                PreviousGround = hits[i].point;
                return;
            }
        //Set us to not be on the ground. We can only hit this if the previous checks failed
        colInfo.OnGround = false;
    }
    /// <summary>
    /// Performs the raycasts to detect collisions for MoveTo
    /// </summary>
    /// <param name="dir">The direction & magnitude of the raycasts</param>
    /// <param name="offsetIndex">The index that the output hitInfo is from the second raycast instead of the first</param>
    /// <returns>An array containing hitInfo from two raycasts. One with radius, the other with radius + offset</returns>
    private RaycastHit[] MoveToRaycasts(Vector3 dir, out int offsetIndex)
    {   //Raycast for the players regular collider
        RaycastHit[] regular = ColliderInfo.CastAll(colInfo, dir);
        //Raycast for the players collider with the offset
        RaycastHit[] withOffset = ColliderInfo.CastAllWithOffset(colInfo, dir);
        //Sort them by distance. Closest ones should be checked first
        System.Array.Sort(regular, Conditions.CompareDist);
        System.Array.Sort(withOffset, Conditions.CompareDist);
        //Combine the raycast results
        RaycastHit[] total = new RaycastHit[regular.Length + withOffset.Length];
        for (int i = 0; i < total.Length; i++)
        {
            if (i >= regular.Length)
                total[i] = withOffset[i - regular.Length];
            else
                total[i] = regular[i];
        }
        offsetIndex = regular.Length;
        return total;
    }
    /// <summary>
    /// Rotates the player based on the Inputs Mouse Y and Mouse X
    /// </summary>
    /// <param name="sensitivity">A multiplier for how much the rotation should be made</param>
    /// <param name="xFactor">How much affect is made along the x plane. 1 = full, 0 = none. xFactor is not clamped</param>
    /// <param name="yFactor">How much affect is made along the y plane. 1 = full, 0 = none. yFactor is not clamped</param>
    void Rotate(float sensitivity, float xFactor, float yFactor)
    {   //Make sure all values are valid. They default to 1 if invalid
        if (sensitivity <= 0)
        {
            Debug.LogError("Invalid Sensitivity");
            sensitivity = 1;
        }
        if (xFactor <= 0)
        {
            Debug.LogError("Invalid xFactor");
            xFactor = 1;
        }
        if (yFactor <= 0)
        {
            Debug.LogError("Invalid yFactor");
            yFactor = 1;
        }
        Vector3 rotation = transform.eulerAngles;
        //Still need to clamp the cameras rotation
        float xChange = -InputManager.GetInput("Mouse Y") * (sensitivity * yFactor) * Time.deltaTime;
        float x = (transform.eulerAngles.x) + xChange;
        x -= 80;
        if (x < 180 && x > 0)
            x = 0;

        if (x <= 0)
            x += 360;
        x = Mathf.Clamp(x, 200, 360);
        x += 80;
        rotation.x = x;

        rotation.y += InputManager.GetInput("Mouse X") * (sensitivity * xFactor) * Time.deltaTime;
        rotation.y %= 360;

        transform.eulerAngles = rotation;
    }

    /// <summary>
    /// Rotates the player to look at deg along the horizontal plane over a period of time
    /// </summary>
    /// <param name="deg">The angle the player is expected to look at</param>
    void RotateTo(float deg)
    {
        Vector3 rotation = transform.eulerAngles;
        //Subtract the current y rotation from deg
        deg -= transform.eulerAngles.y;
        //We capp deg between 180 and -180
        if (deg > 180)
            deg -= 360;
        if (deg < -180)
            deg += 360;
        //deg now represents the change in degrees
        float step = forceRotateSpeed * Time.deltaTime;

        //Check if we can jump strait to the target angle
        if (Mathf.Abs(deg) <= step)
        {
            rotation.y += deg;
            rotateToAngle = false;
            return;
        }

        //if deg is > 0, rotate left,
        if (deg > 0)
            rotation.y += step;
        //else rotate right
        else
            rotation.y -= step;

        transform.eulerAngles = rotation;
    }
    /// <summary>
    /// Call when you want to forcefully make the player look in a direction along the horizontal plane
    /// </summary>
    /// <param name="angleInDeg">The target rotation</param>
    public void ForceRotate(float angleInDeg)
    {   //Set the target rotation
        targetRotation = angleInDeg;
        //Make sure the angle is valid
        angleInDeg -= transform.eulerAngles.y;
        //We capp deg between 180 and -180
        if (angleInDeg > 180)
            angleInDeg -= 360;
        if (angleInDeg < -180)
            angleInDeg += 360;
        //We are too close to the angle so we consider it invalid
        if (angleInDeg < 1 && angleInDeg > -1)
            return;
        //Calculate the rotation speed
        forceRotateSpeed = Mathf.Abs(angleInDeg) / forceRotateTime;
        //Begin the rotation
        rotateToAngle = true;
    }
    /// <summary>
    /// Accelerates the player based on the acceleration values
    /// </summary>
    /// <param name="decelerate">Should the player decellerate</param>
    /// <param name="doClamp">Should the speed be clamped between 0 and maxSpeed</param>
    public void Accelerate(bool decelerate, bool doClamp = true)
    {
        if (decelerate)
            //Decelerate
            HozSpeed -= decelleration * Time.deltaTime;
        else
            //Accelerate
            if (HozSpeed < minSpeed)
            HozSpeed += belowMinAcceleration * Time.deltaTime;
            else
            HozSpeed += acceleration * Time.deltaTime;
        //Make sure the caller wants the speed capped. There are some instances where this would not be wanted
        if (doClamp)
            direction.HozSpeed = Mathf.Clamp(HozSpeed, 0, direction.MaxHozSpeed);
    }
    #region UNITYEDITOR
#if UNITY_EDITOR
    /// <summary>
    /// Draw debugging information
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        colInfo.SetTransform(transform);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + test);
        Gizmos.DrawWireSphere(colInfo.GetUpperPoint() + test, colInfo.Radius);
        Gizmos.DrawWireSphere(colInfo.GetLowerPoint() + test, colInfo.Radius);
    }
#endif
    #endregion
}