using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public static class CodeGenerator
{
    public static string ReplaceInTemplate(string templatePath, Dictionary<string, string> replacementVariables)
    {
        var template = File.ReadAllText(templatePath);
        foreach (var variable in replacementVariables)
        {
            var replacementString = variable.Value;
            template = template.Replace("$" + variable.Key + "$", replacementString);
        }

        return template;
    }

    public static void WriteClass(string fileName, string classContent, string outputPath)
    {
        if (!Directory.Exists(outputPath))
        {
            var dir = Directory.CreateDirectory(outputPath);
        }
        File.WriteAllText(outputPath + fileName + ".cs", classContent);
     
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        
//        ScriptableObjectUtility.CreateAsset(fileName);
    }
    

}