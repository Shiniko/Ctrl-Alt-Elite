using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DinoFracture.Editor
{
    public class DinoFractureSettingsProvider : SettingsProvider
    {
        class Styles
        {
            public static GUIContent ClearEditModeCacheText = new GUIContent("Clear Cache Data In Edit Mode", "If true, cached data used to speed up fracturing will be cleared when pre-fracturing inside the editor in edit mode. This helps reduce memory pressure after fracturing.\n\nThis value has no effect during play mode nor in builds; it will reset after the editor is closed.");
            public static GUIContent ClearTogglePlayModeCacheText = new GUIContent("Clear Cache Data Toggling Play Mode", "If true, cached data used to speed up fracturing will be cleared when toggling play mode in the editor. This will reflect the state of a build upon startup.\n\nThis value has no effect in builds; it will reset after the editor is closed.");
        }

        public DinoFractureSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        {
        }

        public override void OnGUI(string searchContext)
        {
            // Make our controls draw like they are serialized properties
            bool hierarchyMode = EditorGUIUtility.hierarchyMode;
            EditorGUIUtility.hierarchyMode = true;

            EditorGUI.BeginChangeCheck();
            FractureEngine.ClearCacheDataInEditMode = EditorGUILayout.Toggle(Styles.ClearEditModeCacheText, FractureEngine.ClearCacheDataInEditMode, GUI.skin.toggle);
            if (EditorGUI.EndChangeCheck())
            {
                if (FractureEngine.ClearCacheDataInEditMode)
                {
                    FractureEngine.ClearCachedFractureData();
                }
            }

            FractureEngine.ClearCacheDataTogglingPlayMode = EditorGUILayout.Toggle(Styles.ClearTogglePlayModeCacheText, FractureEngine.ClearCacheDataTogglingPlayMode, GUI.skin.toggle);

            EditorGUIUtility.hierarchyMode = hierarchyMode;
        }

        [SettingsProvider]
        public static SettingsProvider CreateDinoFractureSettingsProvider()
        {
            return new DinoFractureSettingsProvider("Preferences/DinoFracture", SettingsScope.User);
        }
    }
}