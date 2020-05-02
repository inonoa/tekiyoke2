using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundGroup : MonoBehaviour
{
    [SerializeField] SoundEffect[] ses;

    public AudioClip Find(string name){
        foreach(SoundEffect se in ses){
            if(se.Name==name) return se.Clip;
        }
        Debug.LogError("そんなSEはない");
        return null;
    }
}

[Serializable]
public class SoundEffect{

    [SerializeField] string _Name;
    public string Name => _Name;

    [SerializeField] AudioClip _Clip;
    public AudioClip Clip => _Clip;
}
