using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffects : MonoBehaviour
{
    [SerializeField] Material[] mats;
    [SerializeField] bool[] appliesMat;
    [SerializeField] Material matThatDoesNothing;

    RenderTexture[] rTexs;

    void Awake(){

        rTexs = new RenderTexture[mats.Length - 1];

        for(int i=0; i<rTexs.Length; i++){
            rTexs[i] = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rTexs[i].Create();
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst){

        int numActiveMats = 0;
        foreach(bool applies in appliesMat){
            if(applies) numActiveMats++;
        }

        if(numActiveMats==0){
            Graphics.Blit(src, dst, matThatDoesNothing);
        }
        else if(numActiveMats==1){
            for(int i=0; i<appliesMat.Length; i++){
                if(appliesMat[i]){
                    Graphics.Blit(src, dst, mats[i]);
                    break;
                }
            }
        }
        else{
            int rtidx = 0;

            for(int i=0; i<mats.Length; i++){
                if(appliesMat[i]){
                    if(rtidx==0)                      Graphics.Blit(src,            rTexs[0],     mats[i]);
                    else if(rtidx != numActiveMats-1) Graphics.Blit(rTexs[rtidx-1], rTexs[rtidx], mats[i]);
                    else                              Graphics.Blit(rTexs[rtidx-1], dst,          mats[i]);

                    rtidx ++;
                }
            }
        }
    }
}
