using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using Microsoft.CSharp;
using UnityEngine;

public class CodedomGen 
{
    private static string _sourceCode = " public class GeneratedTestClass" +
                                        "  { public void Log() { } }";

    public static void CompileAndExecute()
    {
        var assembly = CompileSourceCodeDom(_sourceCode);
        Debug.Log(assembly);
        ExecuteFromAssembly(assembly,"GeneratedTestClass", "Log");
    }
    
    private static Assembly CompileSourceCodeDom(string sourceCode)
    {
        CodeDomProvider cpd = new CSharpCodeProvider();
        var cp = new CompilerParameters();
        cp.ReferencedAssemblies.Add("System.dll");
        
        cp.ReferencedAssemblies.Add("UnityEngine.dll");
        cp.GenerateExecutable = false;
        CompilerResults cr = cpd.CompileAssemblyFromSource(cp, sourceCode);

        return cr.CompiledAssembly;
    }
    
    private static void ExecuteFromAssembly(Assembly assembly, string type, string method)
    {
        Type fooType = assembly.GetType(type);
        MethodInfo printMethod = fooType.GetMethod(method);
        object foo = assembly.CreateInstance(type);
        printMethod.Invoke(foo, BindingFlags.InvokeMethod, null, null, CultureInfo.CurrentCulture);
    }
}