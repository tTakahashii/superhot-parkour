using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(MovementManager))]
public class MovementManagerEditor : Editor
{
    #region SerializedProperties

    // Movement general properties
    private SerializedProperty playerState, 
        boxcastOffset, boxcastSize, maxDistance, layerMask;

    // CharController properties
    private SerializedProperty charController,
        forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, gravityMultiplier,
        smoothCH, groundDetectionCH;

    // PhysicsController properties
    private SerializedProperty rb,
        forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH,
        smoothPH, groundDetectionPH;

    private SerializedProperty cam,
        forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH;

    // NonPhysicsController properties

    #endregion SerializedProperties

    MovementManager movementManager;

    private SerializedProperty[] generalProperties, charProperties, physicsProperties, nonPhysicsProperties;
    private bool charControllerSettingsGroup = true, 
        physicsControllerSettingsGroup = true, 
        nonPhysicsControllerSettingsGroup = true;
     
    private void OnEnable()
    {
        movementManager = (MovementManager)target;

        //GENERAL
        playerState = serializedObject.FindProperty("playerState");
        boxcastOffset = serializedObject.FindProperty("boxcastOffset");
        boxcastSize = serializedObject.FindProperty("boxcastSize");
        maxDistance = serializedObject.FindProperty("maxDistance");
        layerMask = serializedObject.FindProperty("layerMask");

        generalProperties = new SerializedProperty[]
        {
            playerState, maxDistance, boxcastOffset, boxcastSize, layerMask
        };

        // CHARCONTROLLER
        charController = serializedObject.FindProperty("charController");
        forwardSpeedCH = serializedObject.FindProperty("forwardSpeedCH");
        horizontalSpeedCH = serializedObject.FindProperty("horizontalSpeedCH");
        jumpForceCH = serializedObject.FindProperty("jumpForceCH");
        runMultiplierCH = serializedObject.FindProperty("runMultiplierCH");
        gravityMultiplier = serializedObject.FindProperty("gravityMultiplier");
        smoothCH = serializedObject.FindProperty("smoothCH");
        groundDetectionCH = serializedObject.FindProperty("groundDetectionCH");

        charProperties = new SerializedProperty[]
        {
            charController, 
            forwardSpeedCH, horizontalSpeedCH, jumpForceCH, runMultiplierCH, gravityMultiplier,
            smoothCH, groundDetectionCH
        };

        // PHYSICSCONTROLLER
        rb = serializedObject.FindProperty("rb");
        forwardSpeedPH = serializedObject.FindProperty("forwardSpeedPH");
        horizontalSpeedPH = serializedObject.FindProperty("horizontalSpeedPH");
        jumpForcePH = serializedObject.FindProperty("jumpForcePH");
        runMultiplierPH = serializedObject.FindProperty("runMultiplierPH");
        smoothPH = serializedObject.FindProperty("smoothPH");
        groundDetectionPH = serializedObject.FindProperty("groundDetectionPH");

        physicsProperties = new SerializedProperty[]
        {
            rb,
            forwardSpeedPH, horizontalSpeedPH, jumpForcePH, runMultiplierPH,
            smoothPH, groundDetectionPH
        };

        //NONPHYSICSCONTROLLER
        cam = serializedObject.FindProperty("cam");
        forwardSpeedNonPH = serializedObject.FindProperty("forwardSpeedNonPH");
        horizontalSpeedNonPH = serializedObject.FindProperty("horizontalSpeedNonPH");
        verticalSpeedNonPH = serializedObject.FindProperty("verticalSpeedNonPH");
        runMultiplierNonPH = serializedObject.FindProperty("runMultiplierNonPH");

        nonPhysicsProperties = new SerializedProperty[]
        {
            cam,
            forwardSpeedNonPH, horizontalSpeedNonPH, verticalSpeedNonPH, runMultiplierNonPH
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // GENERAL
        foreach (SerializedProperty property in generalProperties)
        {
            EditorGUILayout.PropertyField(property);
        }

        EditorGUILayout.Space(10f);

        switch (movementManager.playerState)
        {
            case MovementManager.MovementState.None:
                charControllerSettingsGroup = physicsControllerSettingsGroup = nonPhysicsControllerSettingsGroup = false;
                break;

            case MovementManager.MovementState.ControllerBased:
                charControllerSettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(true, "CharacterController Settings");
                physicsControllerSettingsGroup = nonPhysicsControllerSettingsGroup = false;
                break;

            case MovementManager.MovementState.PhysicsBased:
                physicsControllerSettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(true, "PhysicsController Settings");
                charControllerSettingsGroup = nonPhysicsControllerSettingsGroup = false;
                break;

            case MovementManager.MovementState.NonPhysicsBased:
                nonPhysicsControllerSettingsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(true, "NonPhysicsController Settings");
                charControllerSettingsGroup = physicsControllerSettingsGroup = false;
                break;
        }


        if (charControllerSettingsGroup)
        {
            foreach (SerializedProperty property in charProperties)
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        else if (physicsControllerSettingsGroup)
        {
            foreach (SerializedProperty property in physicsProperties)
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        else if (nonPhysicsControllerSettingsGroup)
        {
            foreach (SerializedProperty property in nonPhysicsProperties)
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
