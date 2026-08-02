#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EntregaSceneBuilder
{
    private const string MenuPath = "Assets/Scenes/Menu.unity";
    static EntregaSceneBuilder()
    {
        EditorApplication.delayCall += EnsureDeliveryScenes;
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                EditorApplication.delayCall += EnsureDeliveryScenes;
        };
    }

    private static void EnsureDeliveryScenes()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (File.Exists(MenuPath))
        {
            if (EditorBuildSettings.scenes.Length < 2) EnsureBuildSettings();
            return;
        }
        Scene current = SceneManager.GetActiveScene();
        Scene menu = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        new GameObject("Menu Controller").AddComponent<MenuSceneController>();
        EditorSceneManager.SaveScene(menu, MenuPath);
        if (File.Exists("Assets/Scenes/SampleScene.unity")) EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
        else if (current.IsValid() && !string.IsNullOrEmpty(current.path)) EditorSceneManager.OpenScene(current.path);
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh(); EnsureBuildSettings();
    }

    private static void EnsureBuildSettings()
    {
        EditorBuildSettings.scenes = new[] {
            new EditorBuildSettingsScene(new GUID(AssetDatabase.AssetPathToGUID(MenuPath)), true),
            new EditorBuildSettingsScene(new GUID(AssetDatabase.AssetPathToGUID("Assets/Scenes/SampleScene.unity")), true)
        };
    }
}
#endif
