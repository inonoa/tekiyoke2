using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IReusable
{
    T[] pool;
    int nextIndex = 0;
    public int Capacity{ get; private set; }

    public ObjectPool(T prefab, int capacity, Transform parent){
        
        this.Capacity = capacity;
        pool = new T[capacity];
        for(int i=0;i<capacity;i++){
            pool[i] = GameObject.Instantiate(prefab, parent);
            pool[i].gameObject.SetActive(false);
        }
    }

    public T ActivateOne(){
        int tmp = nextIndex;
        nextIndex = (nextIndex+1) % Capacity;

        pool[tmp].gameObject.SetActive(true);
        pool[tmp].Activate();
        return pool[tmp];
    }
}
