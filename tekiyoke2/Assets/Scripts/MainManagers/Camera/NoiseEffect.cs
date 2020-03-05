using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEffect : MonoBehaviour
{
    //将来何パターンか作ってこれを配列にする？
    [SerializeField] Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dst){
        Graphics.Blit(src, dst, mat);
    }
}
