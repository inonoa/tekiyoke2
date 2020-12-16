
using UnityEngine;

public static class PrintExtensions
{
    public static void PrintLines<T>(this MonoBehaviour mono, params T[] objs)
    {
        Debug.Log(string.Join("\n", objs));
    }
}