using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;

/// <summary>
/// Checks if we should transition into a wall climb
/// </summary>
[CreateAssetMenu(fileName = "DoWallClimb", menuName = "Transitions/DoWallClimb", order = 4)]
public class DoWallClimb : Transition<PlayerController>
{
    [SerializeField]
    [Range(0f, 90f)]
    [Tooltip("The angle (into the wall) at which the player wall climbs over wall runs")]
    private float wallClimbAngle = 30f;
    /// <summary>
    /// Required vertical speed to start the wall climb.
    /// </summary>
    [SerializeField]
    [Tooltip("Vertical speed required to start a wall climb")]
    private float requiredVerticalSpeed = 0f;

    /// <summary>
    /// Distance from the wall required for a wallcimb to start
    /// </summary>
    [SerializeField]
    [Tooltip("The distance from a wall the player must be to start a wallclimb")]
    private float wallClimbReach = 0.01f;
    /// <summary>
    /// Checks if the player should perform a wall climb
    /// </summary>
    /// <param name="ctrl">A refernece the player player controller</param>
    /// <returns>Returns true if the player is moving upwards, towards a wall and is close enough</returns>
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        //Make sure the player has vertical speed left
        if (ctrl.VertSpeed > requiredVerticalSpeed
            //Is their a wall in the direction we want to go
            && Physics.Raycast(ctrl.transform.position, ctrl.direction.HozDirection, out RaycastHit hit, ctrl.colInfo.TrueRadius + wallClimbReach)
            //Make sure the player is moving into the wall enough
            && Vector3.Dot(ctrl.direction.HozDirection, hit.normal) <= -Mathf.Sin(wallClimbAngle * Mathf.Deg2Rad)
            //Make sure the player is looking at the wall enough
            && Vector3.Dot(new Vector3(ctrl.transform.forward.x, 0, ctrl.transform.forward.z).normalized, hit.normal) <= -Mathf.Sin(wallClimbAngle * Mathf.Deg2Rad)
            //Do an extra raycast directly towards the wall to update our hit info for the closest point to the wall
            && Physics.Raycast(ctrl.transform.position, -hit.normal, out hit, hit.distance + 0.01f))
        {
            Debug.Log("Wall Climb");
            //Set the check direction for the wall climb to use to make sure it is still on a wall
            ctrl.CheckDirRange = hit.distance;
            Vector3 v = -hit.normal;
            v.y = 0;
            ctrl.CheckDir = v;
            return true;
        }

        return false;
    }
}