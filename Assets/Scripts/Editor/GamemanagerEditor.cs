using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawner))]
public class GamemanagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjectSpawner gameManager = (ObjectSpawner)target;
        if (GUILayout.Button("Level Update"))
        {
            gameManager.EnableTestLevel();
            Debug.Log("Level Update");
        }
    }
}
