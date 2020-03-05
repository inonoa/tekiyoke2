using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffects : MonoBehaviour
{
    [SerializeField] Material[] mats;

    RenderTexture[] rTexs;

    void Start(){

        rTexs = new RenderTexture[mats.Length - 1];

        for(int i=0; i<rTexs.Length; i++){
            rTexs[i] = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rTexs[i].Create();
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst){

        if(mats.Length==1) Graphics.Blit(src, dst, mats[0]);
        else{
            for(int i=0; i<mats.Length; i++){
                if(i==0)                      Graphics.Blit(src,        rTexs[0], mats[i]);
                else if(i!=mats.Length-1)     Graphics.Blit(rTexs[i-1], rTexs[i], mats[i]);
                else                          Graphics.Blit(rTexs[i-1], dst,      mats[i]);
            }
        }
    }
}
