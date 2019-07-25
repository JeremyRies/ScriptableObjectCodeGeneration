using System;
using System.Collections.Generic;
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
}