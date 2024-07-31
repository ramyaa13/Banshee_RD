using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MapScript))]
public class MapScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapScript script = (MapScript)target;
        if (GUILayout.Button("Store Positions"))
        {
            script.StorePositions();
        }
    }
}
#endif