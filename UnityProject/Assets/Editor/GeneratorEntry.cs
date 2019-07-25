using System;
using UnityEditor;
using UnityEngine;

public class GeneratorEntry : EditorWindow
{
    [MenuItem("Assets/GenerateScriptableObject")]
    private static void Generate()
    {
        var selectedObject = Selection.activeObject.name;
        
        var selectedType = Type.GetType(selectedObject +"," + typeof(ReferenceScript).Assembly);
        
        if(selectedType.IsInterface)
            ScriptableObjectGenerator.Create(selectedType);
        else
            Debug.LogError("Can only create Scriptable Objects from interfaces");
    }
}