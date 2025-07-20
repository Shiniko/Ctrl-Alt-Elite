using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DogContext))]
public class DogContextCustomInspector : Editor
{
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
        EditorGUILayout.HelpBox("This script requires an Animator, Collider, and Rigidbody to be present on the object at all times. These references are automatically grabbed during game start.", MessageType.Info);

        // Movement Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("These speeds refer to the animation being used ('anim_dog_canter' / 'anim_dog_sprint' / 'anim_dog_walk') if the speed is too fast it'll look like the dog is sliding around! Adjust as neccesary", MessageType.Info);
        SerializedProperty walkSpeedProp = serializedObject.FindProperty("walkSpeed");
        EditorGUILayout.PropertyField(walkSpeedProp);

        SerializedProperty runSpeedProp = serializedObject.FindProperty("runSpeed");
        EditorGUILayout.PropertyField(runSpeedProp);

        SerializedProperty canterSpeedProp = serializedObject.FindProperty("canterSpeed");
        EditorGUILayout.PropertyField(canterSpeedProp);

        // Roaming Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Roaming Settings", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("The dog will move between these two points (refer to the sphere wireframe gizmos)",MessageType.Info);
        SerializedProperty maxRoamProp = serializedObject.FindProperty("_maxRoamDistance");
        SerializedProperty minRoamProp = serializedObject.FindProperty("_minRoamDistance");
        SerializedProperty axisProp = serializedObject.FindProperty("movementAxis");
        SerializedProperty stallTimeProp = serializedObject.FindProperty("stallTime");
        SerializedProperty startRoamingProp = serializedObject.FindProperty("startRoaming");
        SerializedProperty minimumTravelDistanceProp = serializedObject.FindProperty("_minimumTravelDistance");
        EditorGUILayout.PropertyField(maxRoamProp);
        EditorGUILayout.PropertyField(minRoamProp);
        EditorGUILayout.PropertyField(axisProp, new GUIContent("Movement Axis", "The axis for the dog to walk along during its roaming state"));
        EditorGUILayout.PropertyField(stallTimeProp, new GUIContent("Stall Time","This controls how long (in seconds) the dog will stay in a location during the roaming state."));
        EditorGUILayout.PropertyField(startRoamingProp, new GUIContent("Start Roaming", "Decides whether or not the dog should immediately go into the roaming state at game start."));
        EditorGUILayout.PropertyField(minimumTravelDistanceProp, new GUIContent("Minimum Travel Distance", "The minimum distance the dog will travel between roaming points. This ensures the dog doesnt jitter by only moving a few steps forward and allows for more realistic movement."));

        // Gizmo Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Gizmo Settings", EditorStyles.boldLabel);
        SerializedProperty sizeProp = serializedObject.FindProperty("_size");
        SerializedProperty colorProp = serializedObject.FindProperty("_gizmoColor");
        EditorGUILayout.Slider(sizeProp, 0.1f, 1f);
        EditorGUILayout.PropertyField(colorProp);

        // Apply changes to serialized properties
        serializedObject.ApplyModifiedProperties();
    }
}
