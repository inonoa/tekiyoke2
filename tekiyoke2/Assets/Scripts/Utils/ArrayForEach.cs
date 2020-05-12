using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrayForEach
{
    public static void ForEach<T>(this T[] array, Action<T> action, Predicate<T> where = null){
        foreach(T elm in array){
            if(where != null ? where(elm) : true) action?.Invoke(elm);
        }
    }
}
