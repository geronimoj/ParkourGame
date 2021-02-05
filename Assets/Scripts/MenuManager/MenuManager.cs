using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// For controlling wether a menu is currently active and swapping scenes
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// To determine if the menu is currently open
    /// </summary>
    bool isActive = false;
    /// <summary>
    /// A reference to the player controller
    /// </summary>
    public PlayerController ctrl = null;
    /// <summary>
    /// A reference to the menu GameObject
    /// </summary>
    public GameObject menu = null;
    /// <summary>
    /// Checks if we should open the menu
    /// </summary>
    void Update()
    {   //If we don't have a menu, disable the GameObject
        if (menu == null)
        {
            Debug.LogWarning("No menu GameObject assigned");
            enabled = false;
            return;
        }
#if UNITY_STANDALONE_WIN
        //If we are in a windows build, check the escape button
        if (InputManager.NewInput("Cancel") != 0)
            isActive = !isActive;
#endif
        if (!isActive)
        {
#if !UNITY_ANDROID
            //If we are not on android, disable the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif      //If we have a player controller, enable it
            if (ctrl != null)
                ctrl.enabled = true;
            //Set our gameObject to false... Wait that won't work...
            menu.SetActive(isActive);

            return;
        }
#if !UNITY_ANDROID
        //If we are not on android, enable the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif
        menu.SetActive(isActive);
        //If we have a player controller, disable it
        if (ctrl != null)
            ctrl.enabled = false;
    }

    /// <summary>
    /// For Toggeling whether this menu is active
    /// </summary>
    /// <param name="set">What the menu is set to</param>
    public void ToggleIsActive(bool set)
    {
        isActive = set;
    }
    /// <summary>
    /// Changes the current scene to sceneName
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void SetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
