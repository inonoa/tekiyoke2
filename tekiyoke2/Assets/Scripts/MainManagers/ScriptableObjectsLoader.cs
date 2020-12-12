using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        Object holder = Resources.Load("ScriptableObjectsHolder");
        DontDestroyOnLoad(Instantiate(holder as GameObject));
    }
}
