using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the script for the joystick for android builds
/// </summary>
public class Joystick : MonoBehaviour
{
    /// <summary>
    /// The name of the axis from the input manager the vertical component of the joystick will be stored in
    /// </summary>
    public string vertInputName;
    /// <summary>
    /// The name of the axis from the input manager the horizontal component of the joystick will be stored in
    /// </summary>
    public string hozInputName;
    /// <summary>
    /// The distance from that parent the joystick is allowed to be
    /// Also represents the distance in which the magnitude of the vector will be treated as 1
    /// </summary>
    public float range = 1;

#if UNITY_ANDROID
    private Vector3 dif = Vector3.zero;
    /// <summary>
    /// The index of the TouchInfo we are using for the joystick
    /// </summary>
    private int touchIndex = 0;
    /// <summary>
    /// The ID of the touch
    /// </summary>
    private int fingerID = 0;
    /// <summary>
    /// A refernce to the rectTransform
    /// </summary>
    private RectTransform rt;
    /// <summary>
    /// Set to true when the player has their finger on the joystick
    /// </summary>
    private bool useJoyStick = false;
    /// <summary>
    /// Set to true if we want to set effectively disable this component
    /// </summary>
    private bool doSkip = false;
    /// <summary>
    /// Gets the RectTransform, if it fails, it will log an error
    /// </summary>
    private void Start()
    {
        rt = GetComponent<RectTransform>();

        if (rt == null)
        {
            doSkip = true;
            Debug.LogError("Could not find required RectTransform");
        }
        if (range <= 0)
        {
            Debug.LogError("Invalid Range. Setting value to 1");
            range = 1;
        }
        if (vertInputName == "" || hozInputName == "")
        {
            Debug.LogError("Input values not set");
            doSkip = true;
        }
    }
    /// <summary>
    /// Updates the horionztal and vertical Inputs based on the position of the joystick
    /// </summary>
    void Update()
    {
        if (doSkip)
            return;
        //Are we using the joystick currently
        if (!useJoyStick)
        {   //Set its position to be at its origin and set the inputs to be 0
            rt.localPosition = Vector3.zero;
            InputManager.SetInput(vertInputName, 0);
            InputManager.SetInput(hozInputName, 0);
            return;
        }
        //We are using the joystick so find the touch ID.
        touchIndex = FindTouch();
        //If we don't have any touches, disable the joystick
        if (touchIndex < 0)
        {
            useJoyStick = false;
            return;
        }
        //Move the joystick
        rt.position = Input.GetTouch(touchIndex).position;
        //We use the parents position as the origin of the joystick
        dif = rt.localPosition;
        //Limit the distance the joystick can move away from its origin
        if (dif.magnitude > range)
        {
            dif.Normalize();
            dif *= range;
            rt.localPosition = dif;
        }
        //Update the inputs based on this value
        InputManager.SetInput(vertInputName, dif.y / range);
        InputManager.SetInput(hozInputName, dif.x / range);
    }
    /// <summary>
    /// Sets the state of the joystick to either on or off
    /// </summary>
    public void SetState(bool on)
    {
        useJoyStick = on;
        //Check that we have touches
        if (on == false || Input.touchCount <= 0)
        {   //If not, disable the joystick
            useJoyStick = false;
            return;
        }
        //Store the distance to the first touch
        float closestDist = Vector3.Distance(Input.touches[0].position, rt.position);
        touchIndex = 0;
        //Loop through the other touches and check if they are closer
        for (int i = 1; i < Input.touchCount; i++)
        {   //If so, replace the touchIndex
            if (Vector3.Distance(Input.touches[i].position, rt.position) < closestDist)
            {
                touchIndex = i;
                closestDist = Vector3.Distance(Input.touches[i].position, rt.position);
            }
        }
        //Save the fingerID
        fingerID = Input.GetTouch(touchIndex).fingerId;
    }
    /// <summary>
    /// Finds the index of a touch using the fingerId
    /// </summary>
    /// <returns>Returns the index of the touch</returns>
    private int FindTouch()
    {   //Loop through the touches and compare it with our fingerID. If we can't find our touch, return -1
        for (int i = 0; i < Input.touchCount; i++)
            if (Input.GetTouch(i).fingerId == fingerID)
                return i;
        return -1;
    }
#endif
}
