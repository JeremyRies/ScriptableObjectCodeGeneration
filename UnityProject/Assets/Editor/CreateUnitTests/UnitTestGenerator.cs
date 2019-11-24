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
        var namespaces = GenerateNamespaces(classToTest);
        var generatedFields = GenerateFields(constructorParams);
        var setupContent = GenerateSetup(className, constructorParams);
        var teardownContent = GenerateTeardown();
        var testCasesContent = GenerateTestCases(classToTest);

        //--- Generate main file ---//
        var unitTestContent = new Dictionary<string, string>();
        unitTestContent.Add("NAMESPACES", namespaces);
        unitTestContent.Add("CLASSNAME", className);
        unitTestContent.Add("FIELDS", generatedFields);
        unitTestContent.Add("SETUP", setupContent);
        unitTestContent.Add("TEARDOWN", teardownContent);
        unitTestContent.Add("TESTCASES", testCasesContent);

        var unitTestCode = CodeGenerator.ReplaceInTemplate(_pathToTemplates + _templateName, unitTestContent);

        Debug.Log($"[UnitTestGenerator] Unit test generated for {className}");
        CodeGenerator.WriteClass(className + "Tests", unitTestCode, "Assets/Editor/UnitTests/");
    }

    private static string GenerateNamespaces(Type classToTest)
    {
        return $"using {classToTest.Namespace};";
    }

    private static string GenerateFields(List<(Type, string)> constructorParams)
    {
        var first = true;
        var content = "";
        var dependencyReplacementStrings = new Dictionary<string, string>();

        foreach (var constructorParam in constructorParams)
        {
            var interFaceType = constructorParam.Item1.ToString();
            dependencyReplacementStrings.Add("TYPE", interFaceType);

            var mockName = constructorParam.Item2.ToLowerFirstChar();

            dependencyReplacementStrings.Add("MOCKNAME", "_" + mockName);

            var dependencyField =
            CodeGenerator.ReplaceInTemplate(_pathToTemplates + "DependencyFields.txt", dependencyReplacementStrings);
            dependencyReplacementStrings.Clear();

            if (first) first = false;
            else content += "\r\n\t";

            content += dependencyField;
        }

        

        return content;
    }

    private static string GenerateSetup(string className, List<(Type, string)> constructorParams)
    {
        Dictionary<string, string> vars = new Dictionary<string, string>();
        bool first = true;
        var dependencies = "";
        var mocksCreation = "";

        foreach (var param in constructorParams)
        {
            var type = param.Item1;
            var name = param.Item2;

            if (first) first = false;
            else
            {
                mocksCreation += "\r\n\t\t";
                dependencies += ",\r\n\t\t\t";
            }

            mocksCreation += $"_{name} = _mockRepository.Create<{type}>();";
            dependencies += $"_{name}.Object";
        }

        vars.Add("MOCKS", mocksCreation);
        vars.Add("CLASSNAME", className);
        vars.Add("DEPENDENCIES", dependencies);

        return CodeGenerator.ReplaceInTemplate(_pathToTemplates + "SetupBody.txt", vars);
    }

    private static string GenerateTeardown()
    {
        return "_mockRepository.VerifyAll();";
    }

    private static string GenerateTestCases(Type classToTest)
    {
        var first = true;
        var allCases = "";
        var cases = new Dictionary<string, string>();

        var publicMethods = ReflectionHelper.GetMethods(classToTest);
        foreach (var method in publicMethods)
        {
            cases.Add("METHOD", method.Name);
            cases.Add("METHOD_BODY", $"// Arrange" + "\r\n\t\t" +
                "// TODO: setup your mocks here" + "\r\n\t\t" +
                "\r\n\t\t" +
                $"// Execute" + "\r\n\t\t" +
                (method.ReturnType != typeof(void) ? $"var result = _model.{method.Name}();" : $"_model.{method.Name}();") + "\r\n\t\t" +
                "\r\n\t\t" +
                $"// Verify" + "\r\n\t\t" +
                $"Assert.Fail(\"Test case not ready\");");

            if (first) first = false;
            else allCases += "\r\n\r\n\t";

            allCases += CodeGenerator.ReplaceInTemplate(_pathToTemplates + "TestCases.txt", cases);

            cases.Clear();
        }

        return allCases;
    }
}
 