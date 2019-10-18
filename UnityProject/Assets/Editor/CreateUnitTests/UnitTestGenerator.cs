using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitTestGenerator
{
    private const string _pathToTemplates = "Assets/Editor/CreateUnitTests";
    
    public static void Generate(Type classToTest)
    {
        var className = classToTest.Name;
        var constructorParams = ReflectionHelper.GetConstructorParameters(classToTest);

        var generatedFields = GenerateFields(constructorParams,className);
        var unitTestDependencyStrings = new Dictionary<string, string>();
        unitTestDependencyStrings.Add("CLASSNAME", className);
        unitTestDependencyStrings.Add("Fields", generatedFields);
        var unitTestCode = CodeGenerator.ReplaceInTemplate(_pathToTemplates + "UnitTest.txt", unitTestDependencyStrings);
        
        CodeGenerator.WriteClass(classToTest + "Tests",unitTestCode);
    }

    private static string GenerateFields(List<(Type, string)> constructorParams, string className)
    {
        var dependencyReplacementStrings = new Dictionary<string, string>();

        foreach (var constructorParam in constructorParams)
        {
            var interFaceType = constructorParam.Item1.ToString();
            dependencyReplacementStrings.Add("$INTERFACETYPE", interFaceType);
            
            var mockName = constructorParam.Item2.ToLowerFirstChar();
            
            dependencyReplacementStrings.Add("MOCKNAME", mockName);
        }

        var dependencyFields =
            CodeGenerator.ReplaceInTemplate(_pathToTemplates + "DependencyFields.txt", dependencyReplacementStrings);

        var fieldReplacementStrings = new Dictionary<string, string>();
        fieldReplacementStrings.Add("CLASSNAME", className);
        fieldReplacementStrings.Add("DEPENDENCYFIELDS", dependencyFields);


        return CodeGenerator.ReplaceInTemplate(_pathToTemplates + "Fields.txt", fieldReplacementStrings);
    }
}