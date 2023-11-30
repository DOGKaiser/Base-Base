#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class MyCustomEditorWindow : OdinEditorWindow
{
    [MenuItem("My Game/My Editor")]
    private static void OpenWindow() {
    }

    public string Hello;
}

#endif
