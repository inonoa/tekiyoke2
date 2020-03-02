using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScShoOutOfWindController : MonoBehaviour
{
    Camera cmrOutOfWind;

    Action<Texture2D> onTaken;

    [HideInInspector] public Canvas canvas;

    public void Start(){
        cmrOutOfWind = GetComponent<Camera>();
        cmrOutOfWind.targetTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
    }

    public void BeginScShoOutOfWind(Action<Texture2D> callbackOnTaken){

        onTaken = callbackOnTaken;
        StartCoroutine("ScShoOutOfWind");
    }

    IEnumerator ScShoOutOfWind(){

        yield return new WaitForEndOfFrame();

        Texture2D dstTexture = new Texture2D(cmrOutOfWind.targetTexture.width, cmrOutOfWind.targetTexture.height, TextureFormat.ARGB32, false, false);

        // 画面 -> RenderTexture

        cmrOutOfWind.enabled = true;
        //こうしないとCanvas上のUIが映らない(Screen Space -Camera)
        canvas.worldCamera = cmrOutOfWind;

        cmrOutOfWind.Render();

        cmrOutOfWind.enabled = false;
        canvas.worldCamera = Camera.main;

        // RenderTexture -> Texture2D
        
        RenderTexture tmp = RenderTexture.active;
        RenderTexture.active = cmrOutOfWind.targetTexture;
        dstTexture.ReadPixels(new Rect(0, 0, cmrOutOfWind.targetTexture.width, cmrOutOfWind.targetTexture.height), 0, 0);
        dstTexture.Apply();
        RenderTexture.active = tmp;

        onTaken?.Invoke(dstTexture);
    }
}
