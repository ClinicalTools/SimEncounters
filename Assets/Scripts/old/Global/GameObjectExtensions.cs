using UnityEngine;
using System.Collections.Generic;

public static class GameObjectExtensions
{
    /// <summary>
    /// Returns all direct children with the matching component
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <returns></returns>
    public static T[] GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component
    {
        List<T> components = new List<T>();
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            T component = gameObject.transform.GetChild(i).GetComponent<T>();
            if (component != null)
                components.Add(component);
        }

        return components.ToArray();
    }

    public static string GetGameObjectPath(this GameObject gameObject )
    {
        string path = gameObject.transform.name;
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            path = gameObject.transform.name + "/" + path;
        }
        return path;
    }
}
