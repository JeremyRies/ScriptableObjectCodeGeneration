using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitTestGenerator
{
    #region Constants
    private const string k_outputFolder = "Assets/Scripts/Tests/Editor/";

    private const string k_pathToTemplates = "Assets/Editor/CreateUnitTests/Templates/";
    private const string k_templateNameMain = "TemplateMain.txt";
    private const string k_templateNameFields = "TemplateFields.txt";
    private const string k_templateNameSetupBody = "TemplateSetupBody.txt";
    private const string k_templateNameTestCases = "TemplateTestCase.txt";

    private const string k_templateNamespaces = "NAMESPACES";
    private const string k_templateNamespace = "NAMESPACE";
    private const string k_templateClassname = "CLASSNAME";
    private const string k_templateFields = "FIELDS";
    private const string k_templateSetup = "SETUP";
    private const string k_templateTeardown = "TEARDOWN";
    private const string k_templateTestcases = "TESTCASES";
    private const string k_templateType = "TYPE";
    private const string k_templateMockname = "MOCKNAME";
    private const string k_templateMocks = "MOCKS";
    private const string k_templateDependencies = "DEPENDENCIES";
    private const string k_templateMethod = "METHOD";
    private const string k_templateMethodBody = "METHOD_BODY";

    private const string k_classnameSuffix = "Tests";
    #endregion

    public static void Generate(Type classToTest, string fullPath)
    {
        var className = classToTest.Name;
        var constructorParams = ReflectionHelper.GetConstructorParameters(classToTest);

        //--- Generate parts ---//
        var namespaces = GenerateNamespaces(constructorParams);
        var generatedFields = GenerateFields(constructorParams);
        var setupContent = GenerateSetup(className, constructorParams);
        var teardownContent = GenerateTeardown();
        var testCasesContent = GenerateTestCases(classToTest);

        //--- Generate main file ---//
        var unitTestContent = new Dictionary<string, string>();
        unitTestContent.Add(k_templateNamespaces, namespaces);
        unitTestContent.Add(k_templateNamespace, classToTest.Namespace);
        unitTestContent.Add(k_templateClassname, className);
        unitTestContent.Add(k_templateFields, generatedFields);
        unitTestContent.Add(k_templateSetup, setupContent);
        unitTestContent.Add(k_templateTeardown, teardownContent);
        unitTestContent.Add(k_templateTestcases, testCasesContent);

        var unitTestCode = CodeGenerator.ReplaceInTemplate(k_pathToTemplates + k_templateNameMain, unitTestContent);

        Debug.Log($"[UnitTestGenerator] Unit test generated for {className}");
        
        CodeGenerator.WriteClass(className + k_classnameSuffix, unitTestCode, k_outputFolder + GetOutputDir(fullPath));
    }

    private static string GetOutputDir(string fullPath)
    {
        var split = fullPath.Split('/');
        var filePath = "";

        // Assets/Scripts/.../.../filename.cs
        for (int i = 2; i < split.Length - 1; i++)
        {
            filePath += split[i] + '/';
        }

        return filePath;
    }

    private static string GenerateNamespaces(List<(Type, string)> constructorParams)
    {
        HashSet<string> namespaceSet = new HashSet<string>();
        foreach (var param in constructorParams)
        {
            var type = param.Item1;
            namespaceSet.Add(type.Namespace);
        }
        
        var namespaces = "";
        foreach (var name in namespaceSet)
        {
            namespaces += $"using {name};\r\n";
        }
        
        return namespaces;
    }

    private static string GenerateFields(List<(Type, string)> constructorParams)
    {
        var first = true;
        var content = "";
        var dependencyReplacementStrings = new Dictionary<string, string>();

        foreach (var constructorParam in constructorParams)
        {
            var interFaceType = constructorParam.Item1.Name;
            dependencyReplacementStrings.Add(k_templateType, interFaceType);

            var mockName = constructorParam.Item2.ToLowerFirstChar();

            dependencyReplacementStrings.Add(k_templateMockname, "_" + mockName);

            var dependencyField =
            CodeGenerator.ReplaceInTemplate(k_pathToTemplates + k_templateNameFields, dependencyReplacementStrings);
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
            var type = param.Item1.Name;
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

        vars.Add(k_templateMocks, mocksCreation);
        vars.Add(k_templateClassname, className);
        vars.Add(k_templateDependencies, dependencies);

        return CodeGenerator.ReplaceInTemplate(k_pathToTemplates + k_templateNameSetupBody, vars);
    }

    private static string GenerateTeardown()
    {
        return "";
    }

    private static string GenerateTestCases(Type classToTest)
    {
        var first = true;
        var allCases = "";
        var cases = new Dictionary<string, string>();

        var publicMethods = ReflectionHelper.GetMethods(classToTest);
        foreach (var method in publicMethods)
        {
            cases.Add(k_templateMethod, method.Name);
            cases.Add(k_templateMethodBody, $"// Arrange" + "\r\n\t\t" +
                "// TODO: setup your mocks here" + "\r\n\t\t" +
                "\r\n\t\t" +
                $"// Execute" + "\r\n\t\t" +
                (method.ReturnType != typeof(void) ? $"var result = _model.{method.Name}();" : $"_model.{method.Name}();") + "\r\n\t\t" +
                "\r\n\t\t" +
                $"// Verify" + "\r\n\t\t" +
                $"Assert.Fail(\"Test case not ready\");");

            if (first) first = false;
            else allCases += "\r\n\r\n\t";

            allCases += CodeGenerator.ReplaceInTemplate(k_pathToTemplates + k_templateNameTestCases, cases);

            cases.Clear();
        }

        return allCases;
    }
}
 