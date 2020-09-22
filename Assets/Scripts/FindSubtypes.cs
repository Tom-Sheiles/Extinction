using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class FindSubtypes
{
   public static List<T> FindGenericSubtypes<T>()
    {
        List<T> types = new List<T>();
        List<Type> subTypes = new List<Type>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var subType = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(T)));
            subTypes.AddRange(subType);
        }

        foreach (var type in subTypes)
        {
            object instance = Activator.CreateInstance(type);

            T mt = (T)instance;
            types.Add(mt);
        }

        return types;
    }
}
