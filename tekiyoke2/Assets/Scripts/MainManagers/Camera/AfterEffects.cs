using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffects : MonoBehaviour
{
    [SerializeField] Material[] mats;
    [SerializeField] bool[] appliesMat;
    [SerializeField] Material matThatDoesNothing;
    [SerializeField] string[] volumePropertyNames;

    PostEffectWrapper[] effects;
    public PostEffectWrapper this[int i] => effects[i];

    RenderTexture[] rTexs;

    void Awake(){

        Debug.Assert(mats.Length == appliesMat.Length);
        Debug.Assert(mats.Length == volumePropertyNames.Length);

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
                             mats[i].GetFloat(volumePropertyNames[i]),
                             appliesMat[i]
                         );
            effects[i].isActive = appliesMat[i];
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst){

        int numActiveMats = 0;
        for(int i=0; i<effects.Length; i++){
            effects[i].isActive = appliesMat[i];
            if(effects[i].isActive) numActiveMats ++;
        }

        if(numActiveMats==0){
            Graphics.Blit(src, dst, matThatDoesNothing);
        }
        else if(numActiveMats==1){
            for(int i=0; i<effects.Length; i++){
                if(effects[i].isActive){
                    Graphics.Blit(src, dst, effects[i].material);
                    break;
                }
            }
        }
        else{
            int rtidx = 0;

            for(int i=0; i<effects.Length; i++){
                if(effects[i].isActive){
                    if(rtidx==0)                      Graphics.Blit(src,            rTexs[0],     effects[i].material);
                    else if(rtidx != numActiveMats-1) Graphics.Blit(rTexs[rtidx-1], rTexs[rtidx], effects[i].material);
                    else                              Graphics.Blit(rTexs[rtidx-1], dst,          effects[i].material);

                    rtidx ++;
                }
            }
        }
    }
}

public class PostEffectWrapper {

    public bool isActive = false;

    public readonly Material material;
    public readonly string volumePropertyName;
    public readonly float defaultVolume;

    public PostEffectWrapper(Material mat, string volumePropertyName, float defaultVolume, bool isActive = false){
        (this.material, this.volumePropertyName, this.defaultVolume, this.isActive) = (mat, volumePropertyName, defaultVolume, isActive);
    }

    public void SetVolume(float volumeRate){
        material.SetFloat(volumePropertyName, defaultVolume * volumeRate);
    }

    public float GetVolume() => material.GetFloat(volumePropertyName) / defaultVolume;
}
