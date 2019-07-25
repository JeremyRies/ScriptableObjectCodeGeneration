using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class ScriptableObjectGenerator
{
    public static void Create(Type @interface)
    {
        var generatedMethods = GenerateMethods(@interface);

        var replacementStrings = new Dictionary<string,string>();
        var name = @interface.Name.Substring(1);
        replacementStrings.Add("NAME", name);
        replacementStrings.Add("INTERFACE", @interface.Name);
        
        replacementStrings.Add("CONTENT", generatedMethods);

        var generatedClass = CodeGenerator.ReplaceInTemplate("Assets/ScriptableObjectTemplate.cs.txt", replacementStrings);

        CodeGenerator.WriteClass(name,generatedClass);
    }

    private static string GenerateMethods(Type type)
    {
        var methods = ReflectionHelper.GetMethods(type);
        var stringBuilder = new StringBuilder();

        for (var index = 0; index < methods.Count; index++)
        {
            var method = methods[index];
            if (method.GetParameters().Length < 1)
            {
                SimpleGetter(method, stringBuilder);
            }
            else
            {
                if (method.GetParameters()[0].ParameterType == typeof(int))
                {
                    GetArryIndex(method, stringBuilder);
                }
                else
                {
                    GetElementByEnum(method,stringBuilder);
                }
            }

            if (index < methods.Count - 1)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
            }
        }

        return stringBuilder.ToString();
    }

    private static void GetElementByEnum(MethodInfo method, StringBuilder stringBuilder)
    {
       var replacementStrings = new Dictionary<string, string>
       {
           {"METHOD", method.Name},
           {"RETURN", method.ReturnType.Name},
           {"ENUM", method.GetParameters()[0].ParameterType.Name}
       };

       var generatedMethod = CodeGenerator.ReplaceInTemplate("Assets/GetElementByEnum.txt", replacementStrings);
       stringBuilder.Append(generatedMethod);
    }

    private static void GetArryIndex(MethodInfo method, StringBuilder stringBuilder)
    {
        var replacementStrings = new Dictionary<string, string>
        {
            {"METHOD", method.Name},
            {"RETURN", method.ReturnType.Name},
        };

        var generatedMethod = CodeGenerator.ReplaceInTemplate("Assets/GetArrayEntryByIndex.txt", replacementStrings);
        stringBuilder.Append(generatedMethod);
    }

    private static void SimpleGetter(MethodInfo method, StringBuilder stringBuilder)
    {
        var replacementStrings = new Dictionary<string, string>
        {
            {"METHOD", method.Name},
            {"RETURN", method.ReturnType.Name}
        };

        var generatedMethod = CodeGenerator.ReplaceInTemplate("Assets/SimpleGetter.txt", replacementStrings);
        stringBuilder.Append(generatedMethod);
    }
}