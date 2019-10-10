using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//https://gamedev.stackexchange.com/questions/130268/how-do-i-compile-a-c-script-at-runtime-and-attach-it-as-a-component-to-a-game-o
//http://www.arcturuscollective.com/archives/22

public class RuntimeCompileTest : MonoBehaviour
{
    public InputField _updateLoopCode;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            GenerateCode();
    }
    

    private string GetSourceCode()
    {
        return @"
        using UnityEngine;

        public class RuntimeCompiled : MonoBehaviour
        {
            public static RuntimeCompiled AddYourselfTo(GameObject host)
            {
                return host.AddComponent<RuntimeCompiled>();
            }

            void Start()
            {
                Debug.Log(""The runtime compiled component was successfully attached to"" + gameObject.name);
            }

            void Update(){
         " +
               _updateLoopCode.text
               
        + @"}
        }";
    }

    private void GenerateCode()
    {
        var assembly = Compile(GetSourceCode());

        var runtimeType = assembly.GetType("RuntimeCompiled");
        var method = runtimeType.GetMethod("AddYourselfTo");

        var del = (Func<GameObject, MonoBehaviour>)
            Delegate.CreateDelegate(
                typeof(Func<GameObject, MonoBehaviour>),
                method
            );
        
        del.Invoke(gameObject);
    }

    private static Assembly Compile(string source)
    {
        var provider = new CSharpCodeProvider();
        var param = GetCompilerParameters();
        
        var result = provider.CompileAssemblyFromSource(param, source);

        HandleErrors(result);
        
        return result.CompiledAssembly;
    }

    private static CompilerParameters GetCompilerParameters()
    {
        var param = new CompilerParameters();


        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName.Contains("Unity"))
            {
                param.ReferencedAssemblies.Add(assembly.Location);
            }
        }

        param.ReferencedAssemblies.Add("System.dll");

        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;
        return param;
    }
    private static void HandleErrors(CompilerResults result)
    {
        if (result.Errors.Count > 0)
        {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors)
            {
                msg.AppendFormat("Error ({0}): {1}\n",
                    error.ErrorNumber, error.ErrorText);
            }

            throw new Exception(msg.ToString());
        }
    }
}
