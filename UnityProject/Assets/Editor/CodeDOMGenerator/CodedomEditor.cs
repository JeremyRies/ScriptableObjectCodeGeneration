using UnityEditor;

public class CodedomEditor : EditorWindow
{
    [MenuItem("CodeGenExamples/RunCodeDomExample")]
    public static void RunCodeDomExample()
    {
        CodedomGen.CompileAndExecute();
    }
}