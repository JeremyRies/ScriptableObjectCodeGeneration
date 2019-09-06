using System;
using System.IO;
using UnityEditor;

public class Demo2 : EditorWindow
{
    [MenuItem("CodeGenExamples/Demo2")]
    public static void Demo()
    {
        for (int i = 0; i < 5; i++)
        {
            var testClassString = "using UnityEngine;" + Environment.NewLine +
                                  "public class AnotherTest"+i+" : MonoBehaviour {" + Environment.NewLine +
                                  "public void Start(){" + Environment.NewLine +
                                  "transform.position = new Vector3(0," + i+ ",0);" + Environment.NewLine +
                                  "}" + Environment.NewLine +
                                  " }";
            File.WriteAllText("Assets/Scripts/Demo2/AnotherTest"+i+".cs", testClassString);
        }

        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

//using UnityEngine;
//
//public class AnotherTest0 : MonoBehaviour
//{
//    public void Start()
//    {
//    }
//}