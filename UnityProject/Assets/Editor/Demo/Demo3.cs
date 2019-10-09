using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Demo3 : EditorWindow
{
    private string _name;

    [MenuItem("CodeGenExamples/Demo3")]
    public static void Demo()
    {
        GetWindow<Demo3>("Demo3");
    }

    public void OnGUI()
    {
        GUILayout.Label("Enter Name!");

        _name = GUILayout.TextField(_name);

        if (GUILayout.Button("Generate"))
        {
            Generate(_name);
        }
    }

    private static void Generate(string scriptName)
    {
        Debug.Log("Generating " + scriptName);
        var testClassString = File.ReadAllText("Assets/Editor/Demo/Demo3Template.cs.txt");

        testClassString = testClassString.Replace("$NAME$", scriptName);

        File.WriteAllText("Assets/Scripts/Demo3/" + scriptName + ".cs", testClassString);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}


[CreateAssetMenu(order = 1,fileName = "Experiment",menuName = "ScriptableObjects/Experiment")]
public class Experiment
{
    public int Value;
}