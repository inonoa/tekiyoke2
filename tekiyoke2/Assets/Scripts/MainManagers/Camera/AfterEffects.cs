using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class AfterEffects : MonoBehaviour
{

    [SerializeField] PostEffectWrapper[] effects;
    public PostEffectWrapper Find(string key){
        foreach(PostEffectWrapper pe in effects){
            if(pe.Material.name==key) return pe;
        }
        print("そんなものはない");
        return null;
    }

    [SerializeField] Material matThatDoesNothing;

    RenderTexture[] rTexs;

    void Awake(){

        rTexs = new RenderTexture[effects.Length - 1];

        for(int i=0; i<rTexs.Length; i++){
            rTexs[i] = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rTexs[i].Create();
        }

        foreach(PostEffectWrapper effect in effects){
            if(effect.isActive) AddCommandBufs(effect.Material);
        }
        
    }

    void AddCommandBufs(Material mat){

        CommandBuffer cBuffer = new CommandBuffer();

        int tmpRTID = Shader.PropertyToID("TmpRT");
        cBuffer.GetTemporaryRT(tmpRTID, -1, -1, 0, FilterMode.Bilinear);

        cBuffer.Blit(
            BuiltinRenderTextureType.CurrentActive,
            tmpRTID
        );
        cBuffer.Blit(
            tmpRTID,
            BuiltinRenderTextureType.CurrentActive,
            mat
        );

        cBuffer.ReleaseTemporaryRT(tmpRTID);

        GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterEverything, cBuffer);
    }

//     void OnRenderImage(RenderTexture src, RenderTexture dst){
//         Graphics.Blit(src,        dst, effects[0].Material);
// 
//         for(int i=0; i<effects.Length; i++){
// 
//             if(i==0)                       Graphics.Blit(src,        rTexs[0], effects[i].isActive ? effects[i].Material : matThatDoesNothing);
//             else if(i != effects.Length-1) Graphics.Blit(rTexs[i-1], rTexs[i], effects[i].isActive ? effects[i].Material : matThatDoesNothing);
//             else                           Graphics.Blit(rTexs[i-1], dst,      effects[i].isActive ? effects[i].Material : matThatDoesNothing);
//         }
//     }

}
