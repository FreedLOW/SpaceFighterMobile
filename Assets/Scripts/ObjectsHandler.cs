using System.Collections.Generic;
using UnityEngine;

public static class ObjectsHandler
{
    public static Dictionary<string, GameObject> objRef = new Dictionary<string, GameObject>();

    public static void AddRef(string name, GameObject obj)
    {
        if (!objRef.ContainsKey(name))
            objRef.Add(name, obj);
    }
}