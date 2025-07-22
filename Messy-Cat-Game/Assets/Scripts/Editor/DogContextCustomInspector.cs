using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DogContext))]
public class DogContextCustomInspector : Editor
{
    #region dropdowns
    private bool showRoamingSettings = false;
    private bool showAgroSettings = false;
    private bool showInvestigateSettings = false;
    private bool showGizmoSettings = false;
    private bool showMovementSettings = false;
    #endregion
    public override void OnInspectorGUI()
    {
        // Get reference to the target object
        DogContext dogContext = (DogContext)target;

        // Draw script field (read-only)
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(dogContext), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();

        // Required References
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Required References", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This script requires an Animator (Animator needs to be using the 'Dog State Machine' controller), Collider, and Rigidbody to be present on the object at all times. These references are automatically grabbed during game start.", MessageType.Info);

        // Movement Settings
        EditorGUILayout.Space();
        showMovementSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementSettings, "Movement Settings");
        if (showMovementSettings) 
        {
            EditorGUILayout.HelpBox("These speeds refer to the animation being used ('anim_dog_canter' / 'anim_dog_sprint' / 'anim_dog_walk') if the speed is too fast it'll look like the dog is sliding around! Adjust as neccesary", MessageType.Info);
            SerializedProperty walkSpeedProp = serializedObject.FindProperty("walkSpeed");
            EditorGUILayout.PropertyField(walkSpeedProp);
            SerializedProperty runSpeedProp = serializedObject.FindProperty("runSpeed");
            EditorGUILayout.PropertyField(runSpeedProp);
            SerializedProperty canterSpeedProp = serializedObject.FindProperty("canterSpeed");
            EditorGUILayout.PropertyField(canterSpeedProp);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Roaming Settings
        EditorGUILayout.Space();
        showRoamingSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showRoamingSettings, "Roaming Settings");
        if (showRoamingSettings)
        {
            EditorGUILayout.HelpBox("The dog will move between these two points (refer to the sphere wireframe gizmos)", MessageType.Info);
            SerializedProperty maxRoamProp = serializedObject.FindProperty("_maxRoamDistance");
            SerializedProperty minRoamProp = serializedObject.FindProperty("_minRoamDistance");
            SerializedProperty axisProp = serializedObject.FindProperty("movementAxis");
            SerializedProperty stallTimeProp = serializedObject.FindProperty("stallTime");
            SerializedProperty startRoamingProp = serializedObject.FindProperty("startRoaming");
            SerializedProperty minimumTravelDistanceProp = serializedObject.FindProperty("_minimumTravelDistance");
            EditorGUILayout.PropertyField(maxRoamProp);
            EditorGUILayout.PropertyField(minRoamProp);
            EditorGUILayout.PropertyField(axisProp, new GUIContent("Movement Axis", "The axis for the dog to walk along during its roaming state"));
            EditorGUILayout.PropertyField(stallTimeProp, new GUIContent("Stall Time", "This controls how long (in seconds) the dog will stay in a location during the roaming state."));
            EditorGUILayout.PropertyField(startRoamingProp, new GUIContent("Start Roaming", "Decides whether or not the dog should immediately go into the roaming state at game start."));
            EditorGUILayout.PropertyField(minimumTravelDistanceProp, new GUIContent("Minimum Travel Distance", "The minimum distance the dog will travel between roaming points. This ensures the dog doesnt jitter by only moving a few steps forward and allows for more realistic movement."));
            if (Mathf.Abs(maxRoamProp.floatValue - minRoamProp.floatValue) < minimumTravelDistanceProp.floatValue)
            {
                EditorGUILayout.HelpBox("The roaming range is less than the minimum travel distance! This will cause the dog to move past these points! The max value based on current settings is " + Mathf.Abs(maxRoamProp.floatValue - minRoamProp.floatValue), MessageType.Warning);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        //Agro Settings
        EditorGUILayout.Space();
        showAgroSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showAgroSettings, "Agro Settings");
        if (showAgroSettings)
        {
            SerializedProperty seeCatTimeProp = serializedObject.FindProperty("seeCatTime");
            SerializedProperty dogAgroMeterProp = serializedObject.FindProperty("dogAgroMeter");
            SerializedProperty barkingRangeProp = serializedObject.FindProperty("barkingRange");
            EditorGUILayout.PropertyField(seeCatTimeProp, new GUIContent("See Cat Time", "The amount of time (in seconds) the dog must see the cat before it starts to chase it."));
            EditorGUILayout.PropertyField(dogAgroMeterProp);
            EditorGUILayout.PropertyField(barkingRangeProp, new GUIContent("Barking Range", "The distance at which the dog will bark at the cat. This is used to alert the person."));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //Investigate Settings
        EditorGUILayout.Space();
        showInvestigateSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showInvestigateSettings, "Investigate Settings");
        if (showInvestigateSettings)
        {
            SerializedProperty investigationTimeProp = serializedObject.FindProperty("investigationTime");
            EditorGUILayout.PropertyField(investigationTimeProp, new GUIContent("Investigation Time", "The amount of time (in seconds) the dog will investigate a suspicious event before returning to idle"));
            EditorGUILayout.HelpBox("This button only works in play mode! Causes the dog to transition to the investigate state for testing purposes", MessageType.Info);
            if (GUILayout.Button("Test Investigate State"))
            {
                dogContext.TestInvestigateState();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        // Gizmo Settings
        EditorGUILayout.Space();
        showGizmoSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showGizmoSettings, "Gizmo Settings");
        if (showGizmoSettings)
        {
            SerializedProperty sizeProp = serializedObject.FindProperty("_size");
            SerializedProperty colorProp = serializedObject.FindProperty("_gizmoColor");
            EditorGUILayout.Slider(sizeProp, 0.1f, 1f);
            EditorGUILayout.PropertyField(colorProp);
        }


        // Apply changes to serialized properties
        serializedObject.ApplyModifiedProperties();
    }
}
