using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectUtility
{
    public static void CreateAsset(string className)
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport); //doesn't really work
        Create(className);
    }

    private static void Create(string className)
    {
        Debug.Log("Compile Finished");
        var asset = ScriptableObject.CreateInstance(className);
        string path = "Assets/Configs/";

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + className + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}