#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MyCustomEditorWindow : OdinEditorWindow
{
    [MenuItem("Base Menu/Open Scene Server")]
    private static void OpenSceneOne() {
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
    }
    
    [MenuItem("Base Menu/Open Scene Client")]
    private static void OpenSceneTwo() {
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[1].path);
    }
    
    [MenuItem("Base Menu/Open Scene Test")]
    private static void OpenSceneThree() {
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[2].path);
    }

}

#endif
