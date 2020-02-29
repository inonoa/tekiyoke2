using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpEvent : MonoBehaviour
{

    public string msg = "";
    public event EventHandler warpStart;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="HeroCenter"){

            if(msg=="XL" || msg=="YL"){
                //右から来たらイベント発生
                if(HeroDefiner.currentHero.velocity.x < 0) warpStart?.Invoke(msg, EventArgs.Empty);

            }else if(msg=="XR" || msg=="YR"){
                //左から来たらイベント発生
                if(HeroDefiner.currentHero.velocity.x > 0) warpStart?.Invoke(msg, EventArgs.Empty);

            }else{
                Debug.Log("WarpEventへの不明な衝突");
            }
        }
    }
}
