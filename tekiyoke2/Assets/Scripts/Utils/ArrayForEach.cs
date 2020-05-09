using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrayForEach
{
    public static void ForEach<T>(this T[] array, Action<T> action){
        foreach(T elm in array){
            action?.Invoke(elm);
        }
    }
}
