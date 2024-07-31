using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;

public class SceneSwitcherEditor : Editor
{
    [MenuItem("Tools/Switch to GameManager Scene %h")]
    public static void SwitchToGameManagerScene()
    {
        SwitchScene("Menu");
    }

    [MenuItem("Tools/Switch to Car Selection Scene %l")]
    public static void SwitchToCarSelectionScene()
    {
        SwitchScene("Lobby");
    }

    [MenuItem("Tools/Switch to Track Scene %g")]
    public static void SwitchToTrackScene()
    {
        SwitchScene("BansheeMap");
    }

    private static void SwitchScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main/" + sceneName + ".unity");
        }
    }
}
#endif
