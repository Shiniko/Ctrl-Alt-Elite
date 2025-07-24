using UnityEditor;

[CustomEditor(typeof(Interactable))]
public class InteractableCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.",MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Spill_Interact))]
public class InteractableCustomInspector2 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Break_Interact))]
public class InteractableCustomInspector3 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Catnip_Interact))]
public class InteractableCustomInspector4 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(DoorInteract))]
public class InteractableCustomInspector5 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Exit_Interact))]
public class InteractableCustomInspector6 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Hide_Interact))]
public class InteractableCustomInspector7 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(Scratch_Interact))]
public class InteractableCustomInspector8 : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When the player enters the collider attached to this object they will then be in range to interact with it. Ensure it is a trigger.", MessageType.Info);
        EditorGUILayout.HelpBox("Make sure this script is on its own GameObject, not on the same object as the visuals for example.", MessageType.Info);
        base.OnInspectorGUI();
    }
}