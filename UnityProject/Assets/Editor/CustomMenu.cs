using UnityEngine;
using UnityEditor;

public class CustomMenu : ScriptableObject
{
    [MenuItem("Custom/TestRunner %t")]
    static void DoIt()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Test Runner");
    }
}