using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Work In Progress
/// </summary>
public class TouchScreenSwipe : MonoBehaviour
{
    /// <summary>
    /// The name of the input the horiztonal change will be stored in
    /// </summary>
    public string horizontalInputName;
    /// <summary>
    /// The name of the input the vertical change will be stored in
    /// </summary>
    public string verticalInputName;
#if UNITY_ANDROID
    /// <summary>
    /// The index of the fingerID
    /// </summary>
    int touchIndex = 0;
    /// <summary>
    /// The fingerID of the touch
    /// </summary>
    private int fingerID = 0;
    /// <summary>
    /// Set the horizontal and vertical input based on the movement of the touch
    /// </summary>
    void Update()
    {   //If either input name are null, return a log an error
        if (horizontalInputName == "" || verticalInputName == "")
        {
            Debug.LogError("Vertical or Horizontal input has not been set");
            return;
        }
        //If we already have a touch, then update our current touchIndex.
        //Otherwise, find a new touch
        if (touchIndex != -1)
            //Update our touchIndex
            touchIndex = FindTouch();
        else
            //Search for a touch
            touchIndex = GetTouchIndex();
        //If we don't have any touches or the touch hasn't moved, set the input values to 0
        if (touchIndex == -1 || Input.touchCount == 0 || Input.GetTouch(touchIndex).phase != TouchPhase.Moved)
        {
            InputManager.SetInput(verticalInputName, 0);
            InputManager.SetInput(horizontalInputName, 0);
            return;
        }
        //Otherwise, set them to the change in position
        InputManager.SetInput(verticalInputName, Input.GetTouch(touchIndex).deltaPosition.y);
        InputManager.SetInput(horizontalInputName, Input.GetTouch(touchIndex).deltaPosition.x);
    }

    /// <summary>
    /// Returns the index of a touch that was neither on a button or joystick
    /// </summary>
    /// <returns></returns>
    int GetTouchIndex()
    {   //Loop through the touches
        for (int i = 0; i < Input.touchCount; i++)
        {   //If the touch was not over a UI component, set us to use that fingerID for touch movement
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
            {
                fingerID = Input.GetTouch(i).fingerId;
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Finds the index of a touch using the fingerId
    /// </summary>
    /// <returns>Returns the index of the touch</returns>
    int FindTouch()
    {   //Search through the touches to see if our touchID still exists, if not, return -1.
        for (int i = 0; i < Input.touchCount; i++)
            if (Input.GetTouch(i).fingerId == fingerID)
                return i;
        return -1;
    }
#endif
}
