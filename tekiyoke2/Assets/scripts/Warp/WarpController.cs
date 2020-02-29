using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpController : MonoBehaviour
{

    (float x, float y) xPos;
    (float x, float y) yPos;

    new CameraController camera;

    static readonly int coolTime = 3;
    int coolTimeNow = 0;

    void Start()
    {
        camera = CameraController.CurrentCamera;

        foreach(WarpEvent w in GetComponentsInChildren<WarpEvent>()){
            w.warpStart += Warp;
            if(w.msg=="XL") xPos = (w.transform.position.x, w.transform.position.y);
            if(w.msg=="YL") yPos = (w.transform.position.x, w.transform.position.y);
        }
    }
    
    void Update()
    {
        if(coolTimeNow > 0) coolTimeNow --;
    }

    void Warp(object msgObj, EventArgs e){
        if(coolTimeNow==0){
            coolTimeNow = coolTime;
            camera.Freeze();

            string colliderStr = msgObj.ToString();
            switch(colliderStr){
                case "XL":
                    HeroDefiner.currentHero.WarpPos(yPos.x-3,yPos.y);
                    break;
                case "XR":
                    HeroDefiner.currentHero.WarpPos(yPos.x+3,yPos.y);
                    break;
                case "YL":
                    HeroDefiner.currentHero.WarpPos(xPos.x-3,xPos.y);
                    break;
                case "YR":
                    HeroDefiner.currentHero.WarpPos(xPos.x+3,xPos.y);
                    break;
            }
        }
    }
}
