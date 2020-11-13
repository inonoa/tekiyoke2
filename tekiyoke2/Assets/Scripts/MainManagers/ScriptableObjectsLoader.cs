using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        GameObject holder = Resources.Load("ScriptableObjectsHolder") as GameObject;
        DontDestroyOnLoad(Instantiate(holder));
    }
}
