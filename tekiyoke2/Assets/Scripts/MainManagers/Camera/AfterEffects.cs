using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class AfterEffects : MonoBehaviour
{

    [SerializeField] PostEffectWrapper[] effects;
    public PostEffectWrapper Find(string key) => effects.Find(key);

    void Awake(){
        Camera camera = GetComponent<Camera>();

        effects.ForEach(
            ef =>{
                ef.Init(camera);
            }
        );
        
    }

}
