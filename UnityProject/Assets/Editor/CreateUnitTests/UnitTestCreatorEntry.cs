using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class UnitTestCreatorEntry : EditorWindow
{
    [MenuItem("Assets/Create/Testing/UnitTest %T", true)]
    private static bool ValidateIfCanGenerate()
    {
        if (Selection.activeObject == null) return false;
        
        MonoScript selectedScript = Selection.activeObject as MonoScript;
        if (selectedScript == null) return false;

        Type selectedType = selectedScript.GetClass();

        if (selectedType == null) return false;
        if (!selectedType.IsClass) return false;

        return true;
    }

    [MenuItem("Assets/Create/Testing/UnitTest %T", priority = 2)]
    private static void Generate()
    {
        MonoScript selectedScript = Selection.activeObject as MonoScript;
        Type selectedType = selectedScript.GetClass();

        UnitTestGenerator.Generate(selectedType, AssetDatabase.GetAssetPath(selectedScript));
    }
}