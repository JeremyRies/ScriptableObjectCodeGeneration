using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectionHelper
{
    public static List<MethodInfo> GetMethods(Type templateInterface)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance;

        List<MethodInfo> methods = new List<MethodInfo>(templateInterface.GetMethods(flags));
        foreach (Type interf in templateInterface.GetInterfaces())
        {
            foreach (MethodInfo method in interf.GetMethods(flags))
                if (!methods.Contains(method))
                    methods.Add(method);
        }

        return methods;
    }


    public static List<(Type, string)> GetConstructorParameters(Type classToTest)
    {
        var parameters = classToTest.GetConstructors()[0].GetParameters();

        return parameters.Select(parameter => (parameter.ParameterType, parameter.Name)).ToList();
    }
    
    public static string ToLowerFirstChar(this string input)
    {
        string newString = input;
        if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
            newString = Char.ToLower(newString[0]) + newString.Substring(1);
        return newString;
    }
}