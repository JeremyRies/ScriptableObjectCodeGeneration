using System.IO;
using UnityEditor;
using UnityEngine;

public class Demo1 : EditorWindow
{
    [MenuItem("CodeGenExamples/Demo1")]
    public static void Demo()
    {
        File.WriteAllText("Assets/Scripts/Demo1/Test.cs", "public class Test { }");
//        
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
    }
}

//public class Test { }
