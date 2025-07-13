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


        // Movement Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
        SerializedProperty speedProp = serializedObject.FindProperty("speed");
        EditorGUILayout.PropertyField(speedProp);

        // Roaming Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Roaming Settings", EditorStyles.boldLabel);
        SerializedProperty maxRoamProp = serializedObject.FindProperty("_maxRoamDistance");
        SerializedProperty minRoamProp = serializedObject.FindProperty("_minRoamDistance");
        SerializedProperty axisProp = serializedObject.FindProperty("movementAxis");
        SerializedProperty stallTimeProp = serializedObject.FindProperty("stallTime");
        EditorGUILayout.PropertyField(maxRoamProp);
        EditorGUILayout.PropertyField(minRoamProp);
        EditorGUILayout.PropertyField(axisProp, new GUIContent("Movement Axis", "The axis for the dog to walk along during its roaming state"));
        EditorGUILayout.PropertyField(stallTimeProp);

        // Gizmo Settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Gizmo Settings", EditorStyles.boldLabel);
        SerializedProperty sizeProp = serializedObject.FindProperty("size");
        SerializedProperty colorProp = serializedObject.FindProperty("gizmoColor");
        EditorGUILayout.Slider(sizeProp, 0.1f, 1f);
        EditorGUILayout.PropertyField(colorProp);

        // Apply changes to serialized properties
        serializedObject.ApplyModifiedProperties();
    }
}
