using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestGenerator
{
    private const string _pathToTemplates = "Assets/Editor/CreateUnitTests/";
    private const string _templateName = "UnitTestTemplate.txt";

    public static void Generate(Type classToTest)
    {
        var className = classToTest.Name;
        var constructorParams = ReflectionHelper.GetConstructorParameters(classToTest);

        //--- Generate parts ---//
        var generatedFields = GenerateFields(constructorParams);
        var setupContent = GenerateSetup(className);
        var teardownContent = GenerateTeardown();
        var testCasesContent = GenerateTestCases(classToTest);

        //--- Generate main file ---//
        var unitTestContent = new Dictionary<string, string>();
        unitTestContent.Add("CLASSNAME", className);
        unitTestContent.Add("FIELDS", generatedFields);
        unitTestContent.Add("SETUP", setupContent);
        unitTestContent.Add("TEARDOWN", teardownContent);
        unitTestContent.Add("TESTCASES", testCasesContent);

        var unitTestCode = CodeGenerator.ReplaceInTemplate(_pathToTemplates + _templateName, unitTestContent);

        CodeGenerator.WriteClass(classToTest + "Tests", unitTestCode, "Assets/Editor/UnitTests");
    }

    private static string GenerateFields(List<(Type, string)> constructorParams)
    {
        var dependencyReplacementStrings = new Dictionary<string, string>();

        foreach (var constructorParam in constructorParams)
        {
            var interFaceType = constructorParam.Item1.ToString();
            dependencyReplacementStrings.Add("INTERFACETYPE", interFaceType);

            var mockName = constructorParam.Item2.ToLowerFirstChar();

            dependencyReplacementStrings.Add("MOCKNAME", "_" + mockName);
        }

        var dependencyFields =
            CodeGenerator.ReplaceInTemplate(_pathToTemplates + "DependencyFields.txt", dependencyReplacementStrings);

        return dependencyFields;
    }

    private static string GenerateSetup(string className)
    {
        var dependencies = "_someDependency.Object";
        return $"_model = new {className}({dependencies});";
    }

    private static string GenerateTeardown()
    {
        return "";
    }

    private static string GenerateTestCases(Type classToTest)
    {
        var cases = new Dictionary<string, string>();

        var publicMethods = ReflectionHelper.GetMethods(classToTest);
        foreach (var method in publicMethods)
        {
            cases.Add("METHOD", method.Name);
            cases.Add("METHOD_BODY", "// TODO");
        }

        return CodeGenerator.ReplaceInTemplate(_pathToTemplates + "TestCases.txt", cases);
    }
}