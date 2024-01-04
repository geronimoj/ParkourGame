using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Transitions;
/// <summary>
/// Checks if the player can grab a ledge
/// </summary>
[CreateAssetMenu(fileName = "AtLedge", menuName = "Transitions/AtLedge", order = 6)]
public class AtLedge : Transition<PlayerController>
{
    /// <summary>
    /// The range that is checked centered on the players head, for a ledge.
    /// </summary>
    [SerializeField]
    [Tooltip("The range that is checked centered on the players head, for a ledge. X and Y are added to the top of the players head separately, to create the range. X is the upper value")]
    private Vector2 ledgeUpperLowerCheckRange = Vector2.zero;

    [SerializeField]
    [Range(2, byte.MaxValue)]
    [Tooltip("The resolution of the ledge checks. Higher resolution adds more checks for a ledge. Min 2 (checks top & bottom of ledge grab area)")]
    private byte ledgeCheckResolution = 2;

    [SerializeField]
    private float minLedgeWidth = 0.25f;
    /// <summary>
    /// Checks if the player is close enough to a ledge to grab it
    /// </summary>
    /// <param name="ctrl">A reference to the player controller</param>
    /// <returns>Returns false if the Crouch input is not 0</returns>
    public override bool ShouldTransition(ref PlayerController ctrl)
    {
        if (InputManager.GetInput("Crouch") != 0)
            return false;

        return CheckForLedge(ctrl.GetAngle(1f, 0f), ctrl);
        /*// Get the horizontal direct we should check
        Vector3 checkDir = ctrl.CheckDir;
        checkDir.y = 0;
        //If checkDir is not assigned a direction, use the players transform.forward
        if (checkDir == Vector3.zero)
        {
            checkDir = ctrl.transform.forward;
            checkDir.y = 0;
        }
        //Make sure checkDir is normalised
        checkDir.Normalize();
        //Store a vector to a position above the player
        Vector3 outPos = GetTopOfPlayer(ref ctrl);
        //Is there anything above the player?
        if (!Physics.Raycast(outPos, checkDir, ctrl.colInfo.TrueRadius + ctrl.atLedgeDistance)
            //Perform a raycast down to see if there is terrain below us
            && Physics.Raycast(outPos + checkDir * (ctrl.colInfo.TrueRadius + ctrl.atLedgeDistance), Vector3.down, out RaycastHit hit, ctrl.lowerGrabDist))
        {   //We have reached a ledge
            Debug.Log("At Ledge");
            //Get the direction we need to check on the horizontal plane
            Vector3 v = hit.point - ctrl.transform.position;
            v.y = 0;
            ctrl.CheckDir = v.normalized;
            //This is not mathematically perfect as it does not account for the players current position
            Vector3 move = hit.point + Vector3.down * ctrl.colInfo.UpperHeight;
            //Perform a raycast from our position just below the top of the ledge to get the distance to the ledge.
            Physics.Raycast(new Vector3(outPos.x, hit.point.y - 1e-5f, outPos.z), ctrl.CheckDir, out hit, ctrl.colInfo.TrueRadius + ctrl.atLedgeDistance);
            //We want to override the x & z position of the point we are going to move to but retain the height.
            //The previous raycast result gave us the top of the obstacle but the most recent one gave us the side.
            //Which we need to use to calculate the position offset to move the player to.
            move.x = hit.point.x;
            move.z = hit.point.z;
            //Calculate the positions horizontal position
            move -= ctrl.CheckDir *  ctrl.colInfo.TrueRadius;
            //Move the player to the destination
            ctrl.Move(move - ctrl.transform.position);

            return true;
        }
        return false;*/
    }
    /// <summary>
    /// Sets the location that defines the top of the player, this is the hieght the majority of the checks start from
    /// </summary>
    /// <param name="ctrl">A reference to the player controller</param>
    /// <returns></returns>
    protected virtual Vector3 GetTopOfPlayer(ref PlayerController ctrl)
    {
        return ctrl.colInfo.GetHighestPoint();
    }

    bool CheckForLedge(Vector3 direction, PlayerController ctrl)
    {   // Cannot check 0 vectors
        if (direction == Vector3.zero)
            return false;

        direction.Normalize();

        Vector3 castOrigin = GetTopOfPlayer(ref ctrl);
        castOrigin.y += ledgeUpperLowerCheckRange.x;
        Ray castRay = new Ray(castOrigin, direction);
        float castDistance = ctrl.colInfo.TrueRadius + ctrl.atLedgeDistance;
        float stepSize = Mathf.Abs(ledgeUpperLowerCheckRange.x - ledgeUpperLowerCheckRange.y) / (ledgeCheckResolution - 1);

        bool foundLedge = false;

        bool lastDidHit = true;
        RaycastHit? lastHit = null;

        bool didHit = false;
        RaycastHit hit = default;
        // Raycast into the wall, starting from the top of the check area, to the bottom.
        for (int step = 0; step < ledgeCheckResolution; step++)
        {
            Vector3 origin = castOrigin;
            origin.y -= step * stepSize;

            castRay.origin = origin;

            didHit = Physics.Raycast(castRay, out hit, castDistance);
            
            // Have a previous hit to compare with
            if (lastHit.HasValue)
            {   // There was no wall & now there is.
                if (!lastDidHit && didHit)
                {
                    foundLedge = true;
                    break;
                }
                // Both previous checks hit a surface.
                if (lastDidHit && didHit)
                {
                    float distDif = Mathf.Abs(lastHit.Value.distance - hit.distance);

                    if (distDif > minLedgeWidth)
                    {
                        foundLedge = true;
                        break;
                    }
                }

                // Both checks have not hit anything, nothing to grab onto
            }

            // Store last hit info
            lastDidHit = didHit;
            lastHit = hit;
        }

        if (foundLedge)
        {   // Raycast to find top of ledge
            castOrigin = hit.point - (hit.normal * (minLedgeWidth / 2f));
            castOrigin.y += stepSize;
            // Cannot find solid ground on the ledge, don't continue wth the grab
            if (!Physics.Raycast(castOrigin, Vector3.down, out var ledgeSurface, stepSize))
                return false;
            // Distance off of wall
            Vector3 destination = hit.point + hit.normal * ctrl.colInfo.TrueRadius;
            // Put player below ledge
            destination.y = ledgeSurface.point.y - ctrl.colInfo.UpperHeight;
            // Move player to be against the top of the ledge.
            ctrl.Move(destination - ctrl.transform.position);
            
            // Set check direction for ledge movement
            Vector3 normal = hit.normal;
            normal.y = 0f;
            ctrl.CheckDir = -normal.normalized;
            ctrl.CheckDirRange = hit.distance;
        }

        return foundLedge;
    }
}
