﻿using UnityEngine;
using UnityEditor;

namespace DestroyIt
{
    public static class HideFlagsUtility
    {
        [MenuItem("Help/Hide Flags/Show All Objects")]
        private static void ShowAll()
        {
            var allGameObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var go in allGameObjects)
            {
                switch (go.hideFlags)
                {
                    case HideFlags.HideAndDontSave:
                        go.hideFlags = HideFlags.DontSave;
                        break;
                    case HideFlags.HideInHierarchy:
                    case HideFlags.HideInInspector:
                        go.hideFlags = HideFlags.None;
                        break;
                }
            }
        }
    }
}