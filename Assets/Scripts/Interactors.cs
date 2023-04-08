using System.Collections;
using System.Collections.Generic;
using System;
public class Interactors
{
    private static Dictionary<string, object> interactorDict = new();
    public static T GetInteractor<T>(string name) where T:class
    {
        object obj = interactorDict[name];
        if(obj is T)
        {
            return obj as T;
        }
        else
        {
            throw new KeyNotFoundException("Interactor hasn't been found");
        }
    }
    public static void AddInteractor(string name, object _object)
    {
        interactorDict[name] = _object;
    }
}
