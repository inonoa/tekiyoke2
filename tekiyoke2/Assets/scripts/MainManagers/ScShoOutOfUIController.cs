using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScShoOutOfUIController : MonoBehaviour
{
    Camera cmrOutOfUI;

    Action<Texture2D> onTaken;

    public void Start(){
        cmrOutOfUI = GetComponent<Camera>();
        cmrOutOfUI.targetTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
    }

    public void BeginScShoOutOfUI(Action<Texture2D> callbackOnTaken){

        onTaken = callbackOnTaken;
        StartCoroutine("ScShoOutOfUI");
    }

    IEnumerator ScShoOutOfUI(){

        yield return new WaitForEndOfFrame();

        Texture2D dstTexture = new Texture2D(cmrOutOfUI.targetTexture.width, cmrOutOfUI.targetTexture.height, TextureFormat.ARGB32, false, false);

        // 画面 -> RenderTexture
        cmrOutOfUI.enabled = true;
        cmrOutOfUI.Render();
        cmrOutOfUI.enabled = false;

        // RenderTexture -> Texture2D
        RenderTexture tmp = RenderTexture.active;
        RenderTexture.active = cmrOutOfUI.targetTexture;
        dstTexture.ReadPixels(new Rect(0, 0, cmrOutOfUI.targetTexture.width, cmrOutOfUI.targetTexture.height), 0, 0);
        dstTexture.Apply();
        RenderTexture.active = tmp;

        onTaken?.Invoke(dstTexture);
    }
}
