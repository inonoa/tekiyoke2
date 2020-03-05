using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffects : MonoBehaviour
{
    [SerializeField] Material[] mats;

    void OnRenderImage(RenderTexture src, RenderTexture dst){
        foreach(Material mt in mats){
            Graphics.Blit(src, dst, mt);
        }
    }
}
