using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumerableForEach
{
    public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
    {
        foreach(T elm in array) action?.Invoke(elm);
    }
}
