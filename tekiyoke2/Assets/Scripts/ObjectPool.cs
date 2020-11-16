using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IReusable
{
    T[] pool;
    int nextIndex = 0;
    public int Capacity{ get; private set; }

    public ObjectPool(T prefab, int capacity, Transform parent)
    {
        Transform poolTF = new GameObject(prefab.name + " Pool").transform;
        poolTF.SetParent(parent);
        
        this.Capacity = capacity;
        pool = new T[capacity];
        for(int i = 0; i < capacity; i++)
        {
            pool[i] = Object.Instantiate(prefab, poolTF);
            pool[i].gameObject.SetActive(false);
        }
    }

    public T ActivateOne(string paramsStr){
        int tmp = nextIndex;
        nextIndex = (nextIndex+1) % Capacity;

        //if(pool[tmp].InUse) Debug.Log(pool[tmp] + " was in use.");
        pool[tmp].gameObject.SetActive(true);
        pool[tmp].Activate(paramsStr);
        return pool[tmp];
    }
}
