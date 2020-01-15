using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBuffer<T>
{
    T[] array;

    int firstIndex = 0; //追加先が[-1]になる
    int lastIndex;
    public int Count{ get; private set; } = 0;

    public RingBuffer(T defaultT, int maxLen = 2<<9){
        array = new T[maxLen];
        lastIndex = (-1) .Mod (maxLen); //追加先が[0]になる
        for(int i=0; i<maxLen; i++) array[i] = defaultT;
    }

    public void PushFirst(T first){
        if(Count >= array.Length){
            Debug.Log("中身多すぎ"); return;
        }

        array[(firstIndex - 1) .Mod (array.Length)] = first;
        firstIndex = (firstIndex - 1) .Mod (array.Length);
        Count ++;
    }
    public void PushLast(T last){
        if(Count >= array.Length){
            Debug.Log("中身多すぎ"); return;
        }

        array[(lastIndex + 1) .Mod (array.Length)] = last;
        lastIndex = (lastIndex + 1) .Mod (array.Length);
        Count ++;
    }

    public T PopFirst(){
        if(Count == 0) Debug.Log("空やぞ");

        T toReturn = array[firstIndex];
        firstIndex = (firstIndex + 1) .Mod (array.Length);
        Count --;

        return toReturn;
    }
    public T PopLast(){
        if(Count == 0) Debug.Log("空やぞ");

        T toReturn = array[lastIndex];
        lastIndex = (lastIndex - 1) .Mod (array.Length);
        Count --;

        return toReturn;
    }

    public T First{
        get{
            if(Count == 0) Debug.Log("空やぞ");
            return array[firstIndex];
        }
    }
    public T Last{
        get{
            if(Count == 0) Debug.Log("空やぞ");
            return array[lastIndex];
        }
    }

    ///<summary>はじめに入れたほうから数えてi番目です</summary>
    public T this[int i]{
        get{
            if(i < 0 || i >= Count) Debug.Log("Index Out Of Range");
            return array[(i + firstIndex) .Mod (array.Length)];
        }

        set{
            if(i < 0 || i >= Count) Debug.Log("Index Out Of Range");
            array[(i + firstIndex) .Mod (array.Length)] = value;
        }
    }

    new public string ToString(){
        return "Count: " + Count
               + "\nfirstIndex: " + firstIndex
               + "\nlastIndex: " + lastIndex;
    }
}
