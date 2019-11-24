using System;
using UnityEditor;
using UnityEngine;

public class UnitTestCreatorEntry : EditorWindow
{
    [MenuItem("Assets/Create/Testing/UnitTest %T", true)]
    private static bool ValidateIfCanGenerate()
    {
        var selectedObject = Selection.activeObject.name;
        var selectedType = Type.GetType(selectedObject + "," + typeof(ReferenceScript).Assembly);
 
        if (selectedType != null && selectedType.IsClass)
        {
            return true;
        }

        return false;
    }

    [MenuItem("Assets/Create/Testing/UnitTest %T", priority = 2)]
    private static void Generate()
    {
        var selectedObject = Selection.activeObject.name;
        var selectedType = Type.GetType(selectedObject + "," + typeof(ReferenceScript).Assembly);

        UnitTestGenerator.Generate(selectedType);
    }
}