﻿using UnityEditor;
using UnityEngine;
/// <summary>
/// Draws all of the players stored information in tabs to not cover the inspector with usless variables
/// </summary>
[CustomEditor(typeof(PlayerController))]
[CanEditMultipleObjects]
public class PlayerControllerEditor : Editor
{
    /// <summary>
    /// Which tab is currently open
    /// </summary>
    private int toolbar;
    /// <summary>
    /// Which tab of the parkour toolbar is currently open
    /// </summary>
    private int parkourToolbar;
    
    #region Hitbox
    SerializedProperty colliderInfo;
    SerializedProperty colliderStates;
    #endregion

    #region Speed

    SerializedProperty speedCap;
    SerializedProperty minSpeed;
    SerializedProperty acceleration;
    SerializedProperty belowMinAcceleration;
    SerializedProperty decelleration;
    SerializedProperty gravity;
    SerializedProperty jumpForce;
    SerializedProperty crouchSpeed;

    #endregion

    #region Rotations

    SerializedProperty cameraFollower;
    SerializedProperty sensitivity;
    SerializedProperty minTurnTime;
    SerializedProperty forceRotateTime;

    #endregion

    #region Parkour

    #region Running
    SerializedProperty stepHeight;
    #endregion

    #region LedgeActions

    SerializedProperty lowLedgeHeight;
    SerializedProperty ledgeSpaceRequire;
    SerializedProperty ledgeReach;
    SerializedProperty lowerGrabDist;

    #endregion

    #region WallClimb
    #endregion

    #region
    SerializedProperty vaultHeight;
    SerializedProperty vaultDistance;
    #endregion

    #endregion

    #region Misc

    SerializedProperty speedText;
    SerializedProperty respawnPoint;

    #endregion
    /// <summary>
    /// Assign all the serialzable properties from the player
    /// </summary>
    private void OnEnable()
    {
        #region Hitbox
        colliderInfo = serializedObject.FindProperty("colInfo");
        colliderStates = serializedObject.FindProperty("colliderStates");
        #endregion

        #region Speed
        speedCap = serializedObject.FindProperty("direction");
        minSpeed = serializedObject.FindProperty("minSpeed");
        acceleration = serializedObject.FindProperty("acceleration");
        belowMinAcceleration = serializedObject.FindProperty("belowMinAcceleration");
        decelleration = serializedObject.FindProperty("decelleration");
        gravity = serializedObject.FindProperty("gravity");
        jumpForce = serializedObject.FindProperty("jumpForce");
        crouchSpeed = serializedObject.FindProperty("crouchMaxSpeed");
        #endregion

        #region Rotation
        cameraFollower = serializedObject.FindProperty("camFol");
        sensitivity = serializedObject.FindProperty("sensitivity");
        minTurnTime = serializedObject.FindProperty("minTurnTime");
        forceRotateTime = serializedObject.FindProperty("forceRotateTime");
        #endregion

        #region Misc
        speedText = serializedObject.FindProperty("speedText");
        respawnPoint = serializedObject.FindProperty("respawnPosition");
        #endregion
        stepHeight = serializedObject.FindProperty("stepHeight");
        lowLedgeHeight = serializedObject.FindProperty("lowLedgeHeight");
        ledgeSpaceRequire = serializedObject.FindProperty("openSpaceRequired");
        ledgeReach = serializedObject.FindProperty("atLedgeDistance");
        vaultDistance = serializedObject.FindProperty("vaultDistance");
        vaultHeight = serializedObject.FindProperty("playerVaultHeight");
        lowerGrabDist = serializedObject.FindProperty("lowerGrabDist");
    }
    /// <summary>
    /// Draw the inspector tabs with its specific stuff
    /// </summary>
    public override void OnInspectorGUI()
    {
        toolbar = GUILayout.Toolbar(toolbar, new string[] { "Hitbox", "Speed", "Rotation", "Parkour", "Misc"});

        serializedObject.Update();
        switch (toolbar)
        {
            case 0:
                EditorGUILayout.PropertyField(colliderInfo);
                EditorGUILayout.PropertyField(colliderStates);
                break;
            case 1:
                EditorGUILayout.PropertyField(speedCap);
                EditorGUILayout.PropertyField(minSpeed);
                EditorGUILayout.PropertyField(acceleration);
                EditorGUILayout.PropertyField(belowMinAcceleration);
                EditorGUILayout.PropertyField(decelleration);
                EditorGUILayout.PropertyField(gravity);
                EditorGUILayout.PropertyField(jumpForce);
                EditorGUILayout.PropertyField(crouchSpeed);
                break;
            case 2:
                EditorGUILayout.PropertyField(cameraFollower);
                EditorGUILayout.PropertyField(sensitivity);
                EditorGUILayout.PropertyField(minTurnTime);
                EditorGUILayout.PropertyField(forceRotateTime);
                break;
            case 3:

                parkourToolbar = GUILayout.Toolbar(parkourToolbar, new string[] { "Running", "Wall Run", "Wall Climb", "Vault&StepUp", "Ledge Info" });

                switch(parkourToolbar)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        EditorGUILayout.PropertyField(stepHeight);
                        EditorGUILayout.PropertyField(lowLedgeHeight);
                        EditorGUILayout.PropertyField(ledgeSpaceRequire);
                        EditorGUILayout.PropertyField(vaultDistance);
                        EditorGUILayout.PropertyField(vaultHeight);
                        break;
                    case 4:
                        EditorGUILayout.PropertyField(lowerGrabDist);
                        EditorGUILayout.PropertyField(ledgeSpaceRequire);
                        EditorGUILayout.PropertyField(ledgeReach);
                        break;
                }

                break;
            case 4:
                EditorGUILayout.PropertyField(speedText);
                EditorGUILayout.PropertyField(respawnPoint);
                break;
            default:
                DrawDefaultInspector();
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
