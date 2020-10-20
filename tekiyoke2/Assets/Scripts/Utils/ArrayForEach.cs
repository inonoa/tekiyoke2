using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnumerableForEach
{
    ///<summary>Arrayに元からForEachは生えていることに気づいた、愚か……</summary>
    public static void ForEach<T>(this IEnumerable<T> array, Action<T> action, Predicate<T> where = null){
        foreach(T elm in array){
            if(where != null ? where(elm) : true) action?.Invoke(elm);
        }
    }
}

public static class FindByName{
    public static T Find<T>(this IEnumerable<T> collection, string name)
        where T : INamable
    {
        foreach(T elm in collection){
            if(elm.Name == name){
                return elm;
            }
        }
        Debug.LogError("そんな" + typeof(T).ToString() + "はない");
        return default(T);
    }

    public static int FindIndex<T>(this IEnumerable<T> collection, string name)
        where T : INamable
    {
        int i=0;
        foreach(T elm in collection){
            if(elm.Name == name){
                return i;
            }
            i++;
        }
        Debug.LogError("そんな" + typeof(T).ToString() + "はない");
        return -1;
    }
}

public interface INamable{
    string Name{ get; }
}
