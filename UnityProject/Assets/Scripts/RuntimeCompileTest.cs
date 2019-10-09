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

        // We ask the compiled method to add its component to this.gameObject
        var addedComponent = del.Invoke(gameObject);

        // The delegate pre-bakes the reflection, so repeated calls don't
        // cost us every time, as long as we keep re-using the delegate.
    }

    public static Assembly Compile(string source)
    {
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();


        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName.Contains("Unity"))
            {
                param.ReferencedAssemblies.Add(assembly.Location);
            }
        }
        param.ReferencedAssemblies.Add("System.dll");


        // This assembly contains runtime C# code from your Assets folders:
        // (If you're using editor scripts, they may be in another assembly)
        //param.ReferencedAssemblies.Add("CSharp.dll");


        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0) {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors) {
                msg.AppendFormat("Error ({0}): {1}\n",
                    error.ErrorNumber, error.ErrorText);
            }
            throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }
}