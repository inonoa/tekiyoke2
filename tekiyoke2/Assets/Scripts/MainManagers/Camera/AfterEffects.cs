using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AfterEffects : MonoBehaviour
{
    //この辺ラッパーに入れるべきなのかな
    [SerializeField] Material[] mats;
    [SerializeField] bool[] appliesMat;
    [SerializeField] string[] volumePropertyNames;
    [SerializeField] float[] defaultVolumes;
    [SerializeField] Material matThatDoesNothing;

    PostEffectWrapper[] effects;
    public PostEffectWrapper Find(string key){
        foreach(PostEffectWrapper pe in effects){
            if(pe.material.name==key) return pe;
        }
        print("そんなものはない");
        return null;
    }

    RenderTexture[] rTexs;

    void Awake(){

        Debug.Assert(mats.Length == appliesMat.Length);
        Debug.Assert(mats.Length == volumePropertyNames.Length);
        Debug.Assert(mats.Length == defaultVolumes.Length);

        rTexs = new RenderTexture[mats.Length - 1];

        for(int i=0; i<rTexs.Length; i++){
            rTexs[i] = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rTexs[i].Create();
        }

        effects = new PostEffectWrapper[mats.Length];
        for(int i=0;i<mats.Length;i++){
            effects[i] = new PostEffectWrapper(
                             mats[i],
                             volumePropertyNames[i],
                             defaultVolumes[i],
                             appliesMat[i]
                         );
            int i_ = i;
            effects[i].ActiveChanged += (s, e) => appliesMat[i_] = (bool)s;
            effects[i].SetVolume(1);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst){

        for(int i=0; i<effects.Length; i++){
            effects[i].SetActive(appliesMat[i]);

            if(i==0)                       Graphics.Blit(src,        rTexs[0], effects[i].IsActive ? effects[i].material : matThatDoesNothing);
            else if(i != effects.Length-1) Graphics.Blit(rTexs[i-1], rTexs[i], effects[i].IsActive ? effects[i].material : matThatDoesNothing);
            else                           Graphics.Blit(rTexs[i-1], dst,      effects[i].IsActive ? effects[i].material : matThatDoesNothing);
        }
    }
}
