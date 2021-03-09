using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumerableForEach
{
    ///<summary>Arrayに元からForEachは生えていることに気づいた、愚か……</summary>
    public static void ForEach<T>(this IEnumerable<T> array, Action<T> action, Predicate<T> where = null)
    {
        foreach(T elm in array){
            if(where != null ? where(elm) : true) action?.Invoke(elm);
        }
    }
}
