using UnityEditor;

[CustomEditor(typeof(Interactable))]
public class InteractableCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        base.OnInspectorGUI();
        
    }
}
