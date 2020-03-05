using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScShoController : MonoBehaviour
{
    Action<Texture2D> onTaken;
    Camera cmr;
    void Start(){
        cmr = GetComponent<Camera>();
    }

    public void BeginScSho(Action<Texture2D> callcackOnTaken){
        onTaken = callcackOnTaken;
        StartCoroutine("ScSho");
    }

    IEnumerator ScSho(){
        yield return new WaitForEndOfFrame();

        Texture2D scsho = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        scsho.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        scsho.Apply();
        onTaken?.Invoke(scsho);
    }
}
