using System;
using UnityEditor;
using UnityEngine;

public class UnitTestCreatorEntry : EditorWindow
{
    [MenuItem("Assets/CreateUnitTest")]
    private static void Generate()
    {
        var selectedObject = Selection.activeObject.name;
        
        var selectedType = Type.GetType(selectedObject +"," + typeof(ReferenceScript).Assembly);
        
        if(selectedType.IsClass)
            UnitTestGenerator.Generate(selectedType);
        else
            Debug.LogError("Can only create Unit Test from class");
    }
}