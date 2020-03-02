using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScShoOutOfUIController : MonoBehaviour
{
    Camera cmrOutOfUI;

    [SerializeField] Texture2D product;

    static readonly int screenWidth = 1000;
    static readonly int screenHeight = 750;

    public Texture2D ScShoOutOfUI(){

        if(cmrOutOfUI == null){
            cmrOutOfUI = GetComponent<Camera>();
            cmrOutOfUI.targetTexture = new RenderTexture(screenWidth, screenHeight, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        }

        Texture2D dstTexture = new Texture2D(cmrOutOfUI.targetTexture.width, cmrOutOfUI.targetTexture.height, TextureFormat.ARGB32, false, false);

        gameObject.SetActive(true);
        cmrOutOfUI.Render();
        gameObject.SetActive(false);

        RenderTexture tmp = RenderTexture.active;
        RenderTexture.active = cmrOutOfUI.targetTexture;
        dstTexture.ReadPixels(new Rect(0, 0, cmrOutOfUI.targetTexture.width, cmrOutOfUI.targetTexture.height), 0, 0);
        dstTexture.Apply();
        RenderTexture.active = tmp;

        product = dstTexture;
        print("SCSHO");

        return dstTexture;
    }
}
