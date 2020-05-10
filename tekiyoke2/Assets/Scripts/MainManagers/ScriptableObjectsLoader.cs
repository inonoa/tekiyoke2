using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void Init(){
        DontDestroyOnLoad(Instantiate(Resources.Load("ScriptableObjectsHolder") as GameObject));
    }
}
